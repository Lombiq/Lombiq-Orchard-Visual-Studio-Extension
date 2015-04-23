using Lombiq.VisualStudioExtensions.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lombiq.VisualStudioExtensions.Services
{
    public interface IOrchardProjectGenerator
    {
        IResult GenerateModule(CreateProjectContext createProjectContext);
    }


    public class OrchardProjectGenerator : IOrchardProjectGenerator
    {
        public IResult GenerateModule(CreateProjectContext createProjectContext)
        {
            try
            {
                var solutionPath = Path.GetDirectoryName(createProjectContext.Solution.FullName);
                var newModulePath = Path.Combine(solutionPath, "Orchard.Web", "Modules", createProjectContext.ProjectName);

                var newModuleProject = createProjectContext.SolutionFolder.AddFromTemplate(createProjectContext.TemplatePath, newModulePath, createProjectContext.ProjectName);


                ReplaceTokens(Path.Combine(newModulePath, createProjectContext.ProjectName + ".csproj"),
                    new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("$guid$", Guid.NewGuid().ToString()),
                    new KeyValuePair<string, string>("$name$", createProjectContext.ProjectName)
                });

                newModuleProject.Properties.Item("AssemblyName").Value = createProjectContext.ProjectName;

                ReplaceTokens(Path.Combine(newModulePath, "Module.txt"),
                    new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("$guid$", Guid.NewGuid().ToString()),
                    new KeyValuePair<string, string>("$name$", createProjectContext.ProjectName)
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
