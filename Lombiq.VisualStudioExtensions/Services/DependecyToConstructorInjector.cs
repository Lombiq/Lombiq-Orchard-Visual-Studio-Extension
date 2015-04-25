using EnvDTE;
using Lombiq.VisualStudioExtensions.Exceptions;
using Lombiq.VisualStudioExtensions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lombiq.VisualStudioExtensions.Services
{
    public interface IDependencyToConstructorInjector
    {
        IResult Inject(Document document, string dependencyName, string fieldName);
    }


    public class DependecyToConstructorInjector : IDependencyToConstructorInjector
    {
        public IResult Inject(Document document, string dependecyName, string fieldName)
        {
            if (fieldName[0] != '_') fieldName = "_" + fieldName;

            var fileName = document.FullName;
            var className = Path.GetFileNameWithoutExtension(fileName);
            var variableName = fieldName.Substring(1);

            var textDocument = document.Object() as TextDocument;
            EditPoint startEditPoint = (EditPoint)textDocument.StartPoint.CreateEditPoint();
            EditPoint endEditPoint = (EditPoint)textDocument.EndPoint.CreateEditPoint();
            var codeText = startEditPoint.GetText(endEditPoint);

            var codeLines = new List<string>(codeText.Split(new[] { Environment.NewLine }, StringSplitOptions.None));

            var classStartIndex = GetClassStartLineIndex(className, codeLines);
            if (classStartIndex == -1) return Result.FailedResult("Could not insert depencency because no class found in this file.");

            var constructorIndex = GetConstructorLineIndex(className, codeLines);
            if (constructorIndex == -1) return Result.FailedResult("Could not insert depencency because the constructor not found.");

            InsertConstructorCodeLine(constructorIndex, fieldName, variableName, codeLines);

            InsertInjection(constructorIndex, dependecyName, variableName, codeLines);

            InsertPrivateField(classStartIndex, dependecyName, fieldName, codeLines);

            startEditPoint.ReplaceText(endEditPoint, string.Join(Environment.NewLine, codeLines), 0);

            return Result.SuccessResult;
        }


        private int GetConstructorLineIndex(string className, IList<string> codeLines)
        {
            var expectedConstructorStart = string.Format("public {0}(", className);

            var indexOfConstructor = 0;

            while (indexOfConstructor < codeLines.Count)
            {
                if (codeLines[indexOfConstructor].Trim().StartsWith(expectedConstructorStart, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                indexOfConstructor++;
            }

            return indexOfConstructor < codeLines.Count ? indexOfConstructor : -1;
        }

        private int GetClassStartLineIndex(string className, IList<string> codeLines)
        {
            var expectedClassDefinition = string.Format("class {0}", className);

            var indexOfStartLine = 0;

            while (indexOfStartLine < codeLines.Count)
            {
                if (codeLines[indexOfStartLine].IndexOf(expectedClassDefinition, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    break;
                }

                indexOfStartLine++;
            }

            return indexOfStartLine < codeLines.Count ? indexOfStartLine : -1;
        }

        private void InsertPrivateField(int classStartLineIndex, string dependency, string fieldName, IList<string> codeLines)
        {
            var classStartLine = codeLines[classStartLineIndex];
            var indentSize = GetIndentSizeOfLine(classStartLine);
            var privateFieldLine = string.Format("{0}private readonly {1} {2};", new string(' ', indentSize * 2), dependency, fieldName);

            codeLines.Insert(classStartLineIndex + 2, privateFieldLine);
        }

        private void InsertConstructorCodeLine(int constructorLineIndex, string fieldName, string variableName, IList<string> codeLines)
        {
            var constructorLine = codeLines[constructorLineIndex];
            var indentSize = GetIndentSizeOfLine(constructorLine);
            var constructorCodeLine = new string(' ', Convert.ToInt32(indentSize * 1.5)) + fieldName + " = " + variableName + ";";

            var insertionIndex = constructorLineIndex + 1;
            foreach (var codeLine in codeLines.Skip(constructorLineIndex))
            {
                if (codeLine.Contains("{")) break;
                insertionIndex++;
            }

            codeLines.Insert(insertionIndex, constructorCodeLine);
        }

        private void InsertInjection(int constructorLineIndex, string dependency, string variable, IList<string> codeLines)
        {
            var constructorLine = codeLines[constructorLineIndex];
            var indentSize = GetIndentSizeOfLine(constructorLine);
            var injection = string.Format("{0} {1}", dependency, variable);

            // CASE 1: No parameters in constructor.
            if (constructorLine.EndsWith("()"))
            {
                codeLines.RemoveAt(constructorLineIndex);
                codeLines.Insert(constructorLineIndex, constructorLine.Replace("()", string.Format("({0})", injection)));
            }
            // CASE 2: Has parameters in the same line as the constructor name.
            else if (constructorLine.EndsWith(")"))
            {
                codeLines.RemoveAt(constructorLineIndex);
                codeLines.Insert(constructorLineIndex, constructorLine.Replace("(", string.Format("({0}, ", injection)));
            }
            // CASE 3: Constructor has parameters in multiple lines.
            else if (constructorLine.EndsWith("("))
            {
                codeLines.Insert(constructorLineIndex + 1, new string(' ', Convert.ToInt32(indentSize * 1.5)) + injection + ",");
            }
        }

        private int GetIndentSizeOfLine(string codeLine)
        {
            var indentSize = 0;
            foreach (var codeChar in codeLine)
            {
                if (codeChar == ' ') indentSize++;
                else break;
            }

            return indentSize;
        }
    }
}
