using Lombiq.VisualStudioExtensions.Exceptions;
using System;
using System.IO;
using System.Linq;

namespace Lombiq.VisualStudioExtensions.Services
{
    public class DependecyToConstructorInjector : IDependencyToConstructorInjector
    {
        public string Inject(string dependecyName, string code, string className)
        {
            var lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var expectedConstructorStart = string.Format("public {0}(", className);

            var indexOfConstructor = 0;

            while (indexOfConstructor < lines.Length)
            {
                if (lines[indexOfConstructor].Trim().StartsWith(expectedConstructorStart))
                {
                    break;
                }

                indexOfConstructor++;
            }

            if (indexOfConstructor < lines.Length)
            {
                var lineList = lines.ToList();
                var modifiedConstrucorLine = lines[indexOfConstructor];

                var injectedParameter = string.Format("{0}{1}", char.ToLowerInvariant(dependecyName[1]), dependecyName.Substring(2));


                // CASE 1: No parameters in constructor.
                if (lines[indexOfConstructor].EndsWith("()"))
                {
                    modifiedConstrucorLine = lineList[indexOfConstructor].Insert(lineList[indexOfConstructor].IndexOf('(') + 1, string.Format("{0} {1}", dependecyName, injectedParameter));
                }
                // CASE 2: Has parameters in the same line as the constructor name.
                else if (lines[indexOfConstructor].EndsWith(")"))
                {
                    modifiedConstrucorLine = lineList[indexOfConstructor].Insert(lines[indexOfConstructor].IndexOf('(') + 1, string.Format("{0} {1}, ", dependecyName, injectedParameter));
                }
                // CASE 3: Constructor has parameters in multiple lines.
                else if (lines[indexOfConstructor].EndsWith("("))
                {
                    var firstParameter = lineList[indexOfConstructor + 1];
                    var newParameter = firstParameter.Replace(firstParameter.Trim(), string.Format("{0} {1},", dependecyName, injectedParameter));
                    lineList.Insert(indexOfConstructor + 1, newParameter);
                    lines = lineList.ToArray();
                }
                lineList[indexOfConstructor] = modifiedConstrucorLine;


                var firstIndexOfConstructorCode = GetNthIndex(lines.Select(line => line.Trim()).ToArray(), "{", 3) + 1;

                var indentSizeOfConstructorCode = Convert.ToInt32((lines[firstIndexOfConstructorCode - 1].IndexOf("{")) * 1.5);
                lineList.Insert(firstIndexOfConstructorCode, string.Format("{0}_{1} = {1};", new string(' ', indentSizeOfConstructorCode), injectedParameter));


                var firstIndexOfReadonlyParameters = GetNthIndex(lines.Select(line => line.Trim()).ToArray(), "{", 2) + 1;

                if (lines[firstIndexOfReadonlyParameters].Trim().StartsWith(expectedConstructorStart))
                {
                    lineList.Insert(firstIndexOfReadonlyParameters, String.Empty);
                    lineList.Insert(firstIndexOfReadonlyParameters, String.Empty);
                }

                var indentSizeOfReadonlyParameters = (lines[firstIndexOfReadonlyParameters - 1].IndexOf("{")) * 2;
                lineList.Insert(firstIndexOfReadonlyParameters, string.Format("{0}private readonly {1} _{2};", new string(' ', indentSizeOfReadonlyParameters), dependecyName, injectedParameter));

                return string.Join(Environment.NewLine, lineList);
            }
            else
            {
                throw new DependencyToConstructorInjectorException(DependencyToConstructorIjectorErrorCodes.ConstructorNotFound, "Constructor not found.");
            }
        }


        public static int GetNthIndex(string[] s, string t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
