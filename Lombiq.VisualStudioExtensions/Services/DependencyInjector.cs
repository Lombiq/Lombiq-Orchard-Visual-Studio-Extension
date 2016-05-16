using EnvDTE;
using Lombiq.VisualStudioExtensions.Constants;
using Lombiq.VisualStudioExtensions.Exceptions;
using Lombiq.VisualStudioExtensions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lombiq.VisualStudioExtensions.Services
{
    /// <summary>
    /// Injects dependency to the constructor and inserts the necessary code lines.
    /// </summary>
    public interface IDependencyInjector
    {
        /// <summary>
        /// Injects the given dependency, creates the private readonly field and also inserts the assignment to the constructor.
        /// </summary>
        /// <param name="document">Visual Studio document containing the class where the dependency needs to be injected.</param>
        /// <param name="dependencyName">Name of the dependency (eg. <c>IOrchardServices</c>).</param>
        /// <param name="fieldName">Name of the private readonly field that needs to be created.</param>
        /// <returns>Result of the dependency injection.</returns>
        IResult Inject(Document document, string dependencyName, string fieldName);
    }


    public class DependencyInjector : IDependencyInjector
    {
        public IResult Inject(Document document, string dependecyName, string fieldName)
        {
            var correctFieldName = fieldName[0] == '_' ? fieldName : ("_" + fieldName);
            var context = new DependencyInjectionContext
            {
                FieldName = correctFieldName,
                VariableName = correctFieldName.Substring(1),
                DependencyName = dependecyName,
                ClassName = Path.GetFileNameWithoutExtension(document.FullName),
                Document = document
            };

            // Get code lines from the document.
            GetCodeLines(context);

            // Determine the brace styling.
            DetermineBraceStyling(context);

            // Find the initial line of the class.
            GetClassStartLineIndex(context);
            if (context.ClassStartLineIndex == -1)
            {
                return Result.FailedResult(DependencyInjectorErrorCodes.ClassNotFound);
            }

            // Find the initial line of the first constructor.
            GetConstructorLineIndex(context);
            if (context.ConstructorLineIndex == -1)
            {
                return Result.FailedResult(DependencyInjectorErrorCodes.ConstructorNotFound);
            }

            // Update inner code of the constructor first.
            InsertConstructorCodeLine(context);

            // Update the parameters of the constructor.
            InsertInjectionToConstructor(context);

            // Add the private field to the class.
            InsertPrivateField(context);

            // Finally update the editor window and select the class name to be able to add usings if necessary.
            UpdateCodeEditorAndSelectDependency(context);

            return Result.SuccessResult;
        }


        private void GetCodeLines(DependencyInjectionContext context)
        {
            var textDocument = context.Document.Object() as TextDocument;
            context.StartEditPoint = textDocument.StartPoint.CreateEditPoint();
            context.EndEditPoint = textDocument.EndPoint.CreateEditPoint();
            var codeText = context.StartEditPoint.GetText(context.EndEditPoint);

            context.CodeLines = new List<string>(codeText.Split(new[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None));
        }

        private static void DetermineBraceStyling(DependencyInjectionContext context)
        {
            // Set new line styling as default.
            context.BraceStyle = BraceStyles.OpenInNewLine;
            foreach (var line in context.CodeLines)
            {
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("{")) return;

                if (trimmedLine.EndsWith("{"))
                {
                    context.BraceStyle = BraceStyles.OpenInSameLine;

                    return;
                }
            }
        }

        private static void GetClassStartLineIndex(DependencyInjectionContext context)
        {
            var expectedClassDefinition = "class " + context.ClassName;
            context.ClassStartLineIndex = -1;

            for (int i = 0; i < context.CodeLines.Count; i++)
            {
                if (context.CodeLines[i].IndexOf(expectedClassDefinition, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    context.ClassStartLineIndex = i;

                    return;
                }
            }
        }

        private static void GetConstructorLineIndex(DependencyInjectionContext context)
        {
            var expectedConstructorStart = string.Format("public {0}(", context.ClassName);
            context.ConstructorLineIndex = -1;

            for (int i = 0; i < context.CodeLines.Count; i++)
            {
                var trimmedLine = context.CodeLines[i].Trim();
                if (context.CodeLines[i].Trim().StartsWith(expectedConstructorStart, StringComparison.OrdinalIgnoreCase))
                {
                    context.ConstructorLineIndex = i;

                    return;
                }
            }
        }

        private static void InsertConstructorCodeLine(DependencyInjectionContext context)
        {
            var constructorLine = context.CodeLines[context.ConstructorLineIndex];
            var constructorIndentSize = GetIndentSizeOfLine(constructorLine);
            var constructorCodeLine = new string(' ', Convert.ToInt32(constructorIndentSize * 1.5)) + context.FieldName + " = " + context.VariableName + ";";

            var constructorCodeStartIndex = -1;
            for (int i = context.ConstructorLineIndex; i < context.CodeLines.Count(); i++)
            {
                // Need to find the inner part of the constructor first.
                var trimmedLine = context.CodeLines[i].Trim();
                if (constructorCodeStartIndex == -1 && trimmedLine.Contains("{"))
                {
                    constructorCodeStartIndex = i + 1;

                    continue;
                }

                if (constructorCodeStartIndex == -1) continue;

                // If the first line is empty skip this because it is probably empty conventionally.
                if (constructorCodeStartIndex == i && string.IsNullOrEmpty(trimmedLine)) continue;
                
                // Insert the code line right after field assignments.
                var isItFieldAssignment = trimmedLine.Length > 0 && 
                    trimmedLine.Contains("=") && 
                    (trimmedLine.StartsWith("_") 
                    || char.IsLower(trimmedLine[0]));

                if (isItFieldAssignment) continue;

                context.CodeLines.Insert(i, constructorCodeLine);

                break;
            }
        }

        private static void InsertInjectionToConstructor(DependencyInjectionContext context)
        {
            var constructorLine = context.CodeLines[context.ConstructorLineIndex];
            var indentSize = GetIndentSizeOfLine(constructorLine);
            var injection = string.Format("{0} {1}", context.DependencyName, context.VariableName);

            // CASE 1: No parameters in constructor.
            if (constructorLine.Contains("()"))
            {
                context.CodeLines.RemoveAt(context.ConstructorLineIndex);
                context.CodeLines.Insert(context.ConstructorLineIndex, constructorLine.Replace("()", "(" + injection + ")"));
            }
            // CASE 2: Has parameters in the same line as the constructor name.
            else if (constructorLine.EndsWith(context.BraceStyle == BraceStyles.OpenInNewLine ? ")" : "{"))
            {
                context.CodeLines.RemoveAt(context.ConstructorLineIndex);
                context.CodeLines.Insert(context.ConstructorLineIndex, constructorLine.Replace(")", ", " + injection + ")"));
            }
            // CASE 3: Constructor has parameters in multiple lines.
            else if (constructorLine.EndsWith("("))
            {
                for (int i = context.ConstructorLineIndex; i < context.CodeLines.Count; i++)
                {
                    var indexOfClosing = context.CodeLines[i].IndexOf(')');

                    if (indexOfClosing < 0) continue;

                    var beforeClosing = "";
                    var afterClosing = "";
                    beforeClosing = context.CodeLines[i].Substring(0, indexOfClosing);
                    afterClosing = context.CodeLines[i].Substring(indexOfClosing);

                    context.CodeLines.RemoveAt(i);
                    context.CodeLines.Insert(i, new string(' ', Convert.ToInt32(indentSize * 1.5)) + injection + afterClosing);
                    context.CodeLines.Insert(i, beforeClosing + ",");

                    break;
                }
            }
        }

        private static void InsertPrivateField(DependencyInjectionContext context)
        {
            var classStartLine = context.CodeLines[context.ClassStartLineIndex];
            var indentSize = GetIndentSizeOfLine(classStartLine);
            var privateFieldLine = new string(' ', indentSize * 2) + "private readonly " + context.DependencyName + " " + context.FieldName + ";";

            for (int i = context.ClassStartLineIndex + (context.BraceStyle == BraceStyles.OpenInNewLine ? 2 : 1); i < context.CodeLines.Count; i++)
            {
                if (context.CodeLines[i].Trim().StartsWith("private readonly")) continue;

                context.CodeLines.Insert(i, privateFieldLine);

                break;
            }

        }

        private static void UpdateCodeEditorAndSelectDependency(DependencyInjectionContext context)
        {
            context.StartEditPoint.ReplaceText(context.EndEditPoint, string.Join(Environment.NewLine, context.CodeLines), 0);

            var textSelection = context.Document.Selection as TextSelection;
            textSelection.GotoLine(context.ClassStartLineIndex + 3);
            textSelection.FindPattern(context.DependencyName);
        }

        private static int GetIndentSizeOfLine(string codeLine)
        {
            var indentSize = 0;
            foreach (var codeChar in codeLine)
            {
                if (codeChar == ' ') indentSize++;
                else break;
            }

            return indentSize;
        }


        private enum BraceStyles
        {
            OpenInNewLine = 0,
            OpenInSameLine
        }


        private class DependencyInjectionContext
        {
            public Document Document { get; set; }
            public EditPoint StartEditPoint { get; set; }
            public EditPoint EndEditPoint { get; set; }
            public BraceStyles BraceStyle { get; set; }
            public IList<string> CodeLines { get; set; }
            public string FieldName { get; set; }
            public string VariableName { get; set; }
            public string DependencyName { get; set; }
            public string ClassName { get; set; }
            public int ClassStartLineIndex { get; set; }
            public int ConstructorLineIndex { get; set; }
        }
    }
}
