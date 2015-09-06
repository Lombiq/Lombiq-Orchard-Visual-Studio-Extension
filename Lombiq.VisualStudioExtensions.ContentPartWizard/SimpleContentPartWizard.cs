using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions.ContentPartWizard
{
    public class SimpleContentPartWizard : IWizard
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
                var contentPartName = string.Empty;
                replacementsDictionary.TryGetValue("$rootname$", out contentPartName);

                if (!string.IsNullOrEmpty(contentPartName) && contentPartName.EndsWith("Part"))
                {
                    contentPartName = contentPartName.Substring(0, contentPartName.Length - 4);
                }

                // Add custom parameters.
                replacementsDictionary.Add("$contentpartname$", contentPartName);
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
