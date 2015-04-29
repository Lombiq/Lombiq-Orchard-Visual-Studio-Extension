using Lombiq.VisualStudioExtensions.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lombiq.VisualStudioExtensions.Services
{
    public interface IOrchardProjectGenerator
    {
        IResult GenerateModule(CreateModuleContext createModuleContext);
    }


    public class OrchardProjectGenerator : IOrchardProjectGenerator
    {
        public IResult GenerateModule(CreateModuleContext createModuleContext)
        {
            try
            {
                var solutionPath = Path.GetDirectoryName(createModuleContext.Solution.FullName);
                var newModulePath = Path.Combine(solutionPath, "Orchard.Web", "Modules", createModuleContext.ProjectName);

                var newModuleProject = createModuleContext.SolutionFolder.AddFromTemplate(createModuleContext.TemplatePath, newModulePath, createModuleContext.ProjectName);

                ReplaceTokens(Path.Combine(newModulePath, createModuleContext.ProjectName + ".csproj"),
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("$guid$", Guid.NewGuid().ToString()),
                        new KeyValuePair<string, string>("$name$", createModuleContext.ProjectName)
                    });

                newModuleProject.Properties.Item("AssemblyName").Value = createModuleContext.ProjectName;

                ReplaceTokens(Path.Combine(newModulePath, "Module.txt"),
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("$name$", createModuleContext.ProjectName),
                        new KeyValuePair<string, string>("$author$", createModuleContext.Author),
                        new KeyValuePair<string, string>("$website$", createModuleContext.Website),
                        new KeyValuePair<string, string>("$description$", string.IsNullOrEmpty(createModuleContext.Description) ? "Description for module " + createModuleContext.ProjectName : createModuleContext.Description)
                    });

                ReplaceTokens(Path.Combine(newModulePath, "Properties", "AssemblyInfo.cs"),
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("$guid$", Guid.NewGuid().ToString()),
                        new KeyValuePair<string, string>("$name$", createModuleContext.ProjectName)
                    });

                newModuleProject.Save();
            }
            catch (Exception ex)
            {
                return Result.FailedResult(ex.Message);
            }

            return Result.SuccessResult;
        }


        private void ReplaceTokens(string templateFile, IEnumerable<KeyValuePair<string, string>> replacements)
        {
            string fileContent = "";
            using (var fileStream = new FileStream(templateFile, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream))
            {
                fileContent = streamReader.ReadToEnd();

                foreach (var replacer in replacements)
                {
                    fileContent = fileContent.Replace(replacer.Key, replacer.Value);
                }
            }

            using (var fileStream = new FileStream(templateFile, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(fileContent);
            }
        }
    }
}
