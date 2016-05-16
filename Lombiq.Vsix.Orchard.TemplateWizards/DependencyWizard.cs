using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard.TemplateWizards
{
    public class DependencyWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                var dependencyClassName = string.Empty;
                replacementsDictionary.TryGetValue("$safeitemname$", out dependencyClassName);

                if (!string.IsNullOrEmpty(dependencyClassName) && 
                    dependencyClassName.StartsWith("I") && 
                    dependencyClassName.Length > 1 && 
                    char.IsUpper(dependencyClassName[1]))
                {
                    dependencyClassName = dependencyClassName.Substring(1);
                }

                // Add custom parameters.
                replacementsDictionary.Add("$dependencyclassname$", dependencyClassName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
