using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    /// <summary>
    /// Injects dependency into the constructor and inserts the necessary code lines.
    /// </summary>
    public interface IDependencyInjector
    {
        /// <summary>
        /// Injects the given dependency, creates the private readonly field and also inserts the assignment into the
        /// constructor.
        /// </summary>
        /// <param name="document">Visual Studio document containing the class where the dependency needs to be
        /// injected.</param>
        /// <param name="injectedDependency">Field and constructor parameter type and name to be added to the
        /// code.</param>
        /// <returns>Result of the dependency injection.</returns>
        IResult Inject(Document document, DependencyInjectionData dependencyInjectionData);

        /// <summary>
        /// Returns the expected class name where the dependency needs to be injected.
        /// </summary>
        /// <param name="document">Visual Studio document containing the class where the dependency needs to be injected.</param>
        /// <returns>Expected class name.</returns>
        string GetExpectedClassName(Document document);
    }

    public class DependencyInjector : IDependencyInjector
    {
        public IResult Inject(Document document, DependencyInjectionData dependencyInjectionData)
        {
            var context = new DependencyInjectionContext
            {
                FieldName = dependencyInjectionData.FieldName,
                VariableName = dependencyInjectionData.ConstructorParameterName,
                FieldType = dependencyInjectionData.FieldType,
                VariableType = dependencyInjectionData.ConstructorParameterType,
                ClassName = GetExpectedClassName(document),
                Document = document,
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

            // Find the initial line of the first constructor. If it hasn't been created yet then create it.
            GetConstructorLineIndex(context);
            if (context.ConstructorLineIndex == -1)
            {
                CreateConstructor(context);
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

        public string GetExpectedClassName(Document document) =>
            Path.GetFileNameWithoutExtension(document.FullName);

        private static void GetCodeLines(DependencyInjectionContext context)
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

                if (trimmedLine.StartsWith("{", StringComparison.InvariantCulture)) return;

                if (trimmedLine.EndsWith("{", StringComparison.InvariantCulture))
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
            var expectedConstructorStart = "public " + context.ClassName + "(";
            context.ConstructorLineIndex = -1;

            for (int i = 0; i < context.CodeLines.Count; i++)
            {
                if (context.CodeLines[i].Trim().StartsWith(expectedConstructorStart, StringComparison.OrdinalIgnoreCase))
                {
                    context.ConstructorLineIndex = i;

                    return;
                }
            }
        }

        private static void CreateConstructor(DependencyInjectionContext context)
        {
            var classStartIndentSize = GetIndentSizeOfLine(context.CodeLines[context.ClassStartLineIndex]);
            var createConstructorFromIndex = context.ClassStartLineIndex + (context.BraceStyle == BraceStyles.OpenInNewLine ? 2 : 1);

            // Add two empty lines before and after to separate the constructor from the field and the other parts of the code.
            var constructorCodeLines = context.BraceStyle == BraceStyles.OpenInNewLine ?
                new[]
                {
                    string.Empty,
                    IndentText(classStartIndentSize, 2, "public " + context.ClassName + "()"),
                    IndentText(classStartIndentSize, 2, "{"),
                    IndentText(classStartIndentSize, 2, "}"),
                    string.Empty,
                }
                :
                new[]
                {
                    string.Empty,
                    IndentText(classStartIndentSize, 2, "public " + context.ClassName + "() {"),
                    IndentText(classStartIndentSize, 2, "}"),
                    string.Empty,
                };

            for (int i = 0; i < constructorCodeLines.Length; i++)
            {
                context.CodeLines.Insert(createConstructorFromIndex + i, constructorCodeLines[i]);
            }

            context.ConstructorLineIndex = createConstructorFromIndex + 1;
        }

        private static void InsertConstructorCodeLine(DependencyInjectionContext context)
        {
            var constructorLine = context.CodeLines[context.ConstructorLineIndex];
            var constructorIndentSize = GetIndentSizeOfLine(constructorLine);
            var constructorCodeLine = IndentText(constructorIndentSize, 1.5, context.FieldName + " = " + context.VariableName + ";");

            var constructorCodeLineInserted = false;
            var i = context.ConstructorLineIndex - 1;
            var constructorCodeStartIndex = -1;
            while (i < context.CodeLines.Count && !constructorCodeLineInserted)
            {
                i++;
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
                    (trimmedLine.StartsWith("_", StringComparison.InvariantCulture)
                    || char.IsLower(trimmedLine[0]));

                if (isItFieldAssignment) continue;

                context.CodeLines.Insert(i, constructorCodeLine);
                constructorCodeLineInserted = true;
            }
        }

        private static void InsertInjectionToConstructor(DependencyInjectionContext context)
        {
            var constructorLine = context.CodeLines[context.ConstructorLineIndex];
            var indentSize = GetIndentSizeOfLine(constructorLine);
            var injection = context.VariableType + " " + context.VariableName;

            // CASE 1: No parameters in constructor.
            if (constructorLine.Contains("()"))
            {
                context.CodeLines.RemoveAt(context.ConstructorLineIndex);
                context.CodeLines.Insert(context.ConstructorLineIndex, constructorLine.Replace("()", "(" + injection + ")"));
            }

            // CASE 2: Has parameters in the same line as the constructor name.
            else if (constructorLine.EndsWith(context.BraceStyle == BraceStyles.OpenInNewLine ? ")" : "{", StringComparison.InvariantCulture))
            {
                context.CodeLines.RemoveAt(context.ConstructorLineIndex);
                context.CodeLines.Insert(context.ConstructorLineIndex, constructorLine.Replace(")", ", " + injection + ")"));
            }

            // CASE 3: Constructor has parameters in multiple lines.
            else if (constructorLine.EndsWith("(", StringComparison.InvariantCulture))
            {
                var i = context.ConstructorLineIndex - 1;
                var injectionInserted = false;
                while (i < context.CodeLines.Count && !injectionInserted)
                {
                    i++;
                    var indexOfClosing = context.CodeLines[i].IndexOf(')');

                    if (indexOfClosing < 0) continue;

                    var beforeClosing = context.CodeLines[i].Substring(0, indexOfClosing);
                    var afterClosing = context.CodeLines[i].Substring(indexOfClosing);

                    context.CodeLines.RemoveAt(i);
                    context.CodeLines.Insert(i, IndentText(indentSize, 1.5, injection + afterClosing));
                    context.CodeLines.Insert(i, beforeClosing + ",");
                    injectionInserted = true;
                }
            }
        }

        private static void InsertPrivateField(DependencyInjectionContext context)
        {
            var classStartLine = context.CodeLines[context.ClassStartLineIndex];
            var indentSize = GetIndentSizeOfLine(classStartLine);
            var privateFieldLine = new string(' ', indentSize * 2) + "private readonly " + context.FieldType + " " + context.FieldName + ";";

            for (int i = context.ClassStartLineIndex + (context.BraceStyle == BraceStyles.OpenInNewLine ? 2 : 1); i < context.CodeLines.Count; i++)
            {
                if (context.CodeLines[i].Trim().StartsWith("private readonly", StringComparison.InvariantCulture)) continue;

                context.CodeLines.Insert(i, privateFieldLine);

                break;
            }
        }

        private static void UpdateCodeEditorAndSelectDependency(DependencyInjectionContext context)
        {
            context.StartEditPoint.ReplaceText(context.EndEditPoint, string.Join(Environment.NewLine, context.CodeLines), 0);

            var textSelection = context.Document.Selection as TextSelection;
            textSelection.GotoLine(context.ClassStartLineIndex + 3);
            textSelection.FindPattern(context.FieldType);
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

        private static string IndentText(int baseIndentSize, double indentSizeMultiplier, string text) =>
            new string(' ', Convert.ToInt32(baseIndentSize * indentSizeMultiplier)) + text;

        private enum BraceStyles
        {
            OpenInNewLine = 0,
            OpenInSameLine,
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
            public string VariableType { get; set; }
            public string FieldType { get; set; }
            public string ClassName { get; set; }
            public int ClassStartLineIndex { get; set; }
            public int ConstructorLineIndex { get; set; }
        }
    }
}
