using EnvDTE;
using Lombiq.VisualStudioExtensions.TemplateWizards.Forms;
using Lombiq.VisualStudioExtensions.TemplateWizards.Models;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;

namespace Lombiq.VisualStudioExtensions.TemplateWizards
{
    public class ContentPartWizard : IWizard
    {
        private static string _codeTemplateLocation;

        public static string CodeTemplateLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_codeTemplateLocation))
                    _codeTemplateLocation = Path.Combine(Path.GetDirectoryName(typeof(ContentPartWizard).Assembly.Location), "CodeTemplates");

                return _codeTemplateLocation;
            }
        }


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
                replacementsDictionary.TryGetValue("$safeitemname$", out contentPartName);

                if (!string.IsNullOrEmpty(contentPartName) && contentPartName.EndsWith("Part"))
                {
                    contentPartName = contentPartName.Substring(0, contentPartName.Length - 4);
                }

                var settingsPartName = contentPartName;

                if (!string.IsNullOrEmpty(settingsPartName) && settingsPartName.EndsWith("Settings"))
                {
                    settingsPartName = settingsPartName.Substring(0, settingsPartName.Length - 8);
                }

                // Add custom parameters.
                replacementsDictionary.Add("$contentpartname$", contentPartName);
                replacementsDictionary.Add("$settingscontentpartname$", settingsPartName);

                var addPropertiesDialog = new AddPropertiesDialog();
                addPropertiesDialog.ShowDialog();

                replacementsDictionary.Add("$infosetproperties$", GenerateInfosetProperties(addPropertiesDialog.PropertyItems));
                replacementsDictionary.Add("$virtualproperties$", GenerateVirtualProperties(addPropertiesDialog.PropertyItems));
                replacementsDictionary.Add("$shapepropertyeditors$", GenerateShapePropertyEditors(addPropertiesDialog.PropertyItems));
                replacementsDictionary.Add("$shapepropertydisplays$", GenerateShapePropertyDisplays(addPropertiesDialog.PropertyItems));
                replacementsDictionary.Add("$migrationsrecordproperties$", GenerateMigrationsRecordProperties(addPropertiesDialog.PropertyItems));
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


        private string GenerateInfosetProperties(IList<PropertyItem> properties)
        {
            if (!properties.Any())
                return "";

            var infosetPropertyTemplatePath = Path.Combine(CodeTemplateLocation, "infoset.template");
            var hybridInfosetPropertyTemplatePath = Path.Combine(CodeTemplateLocation, "hybridinfoset.template");

            var infosetPropertyTemplate = File.Exists(infosetPropertyTemplatePath) ? File.ReadAllText(infosetPropertyTemplatePath) : "";
            var hybridInfosetPropertyTemplate = File.Exists(hybridInfosetPropertyTemplatePath) ? File.ReadAllText(hybridInfosetPropertyTemplatePath) : "";

            var finalPropertiesList = new List<string>();
            foreach (var item in properties)
            {
                var finalProperty = item.HybridInfoset ? hybridInfosetPropertyTemplate.Replace("#propertyname#", item.Name) : infosetPropertyTemplate.Replace("#propertyname#", item.Name);
                finalProperty = finalProperty.Replace("#propertytype#", item.Type);

                finalPropertiesList.Add(finalProperty);
            }

            return string.Join(Environment.NewLine + Environment.NewLine, finalPropertiesList);
        }

        private string GenerateVirtualProperties(IList<PropertyItem> properties)
        {
            if (!properties.Any(property => property.HybridInfoset))
                return "";

            var virtualPropertyTemplatePath = Path.Combine(CodeTemplateLocation, "virtualproperty.template");

            var virtualPropertyTemplate = File.Exists(virtualPropertyTemplatePath) ? File.ReadAllText(virtualPropertyTemplatePath) : "";

            var finalPropertiesList = new List<string>();
            foreach (var item in properties.Where(property => property.HybridInfoset))
            {
                var finalProperty = virtualPropertyTemplate.Replace("#propertyname#", item.Name);
                finalProperty = finalProperty.Replace("#propertytype#", item.Type);

                finalPropertiesList.Add(finalProperty);
            }

            return string.Join(Environment.NewLine, finalPropertiesList);
        }

        private string GenerateShapePropertyEditors(IList<PropertyItem> items)
        {
            if (!items.Any(property => !property.SkipFromShapeTemplate))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "shapepropertyeditor.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in items.Where(property => !property.SkipFromShapeTemplate))
            {
                var finalReplacement = template.Replace("#propertyname#", item.Name);
                if (item.Type == "bool")
                {
                    finalReplacement = finalReplacement.Replace("#editortype#", "CheckBox");
                }
                else if (item.Type == "string")
                {
                    finalReplacement = finalReplacement.Replace("#editortype#", "TextBox");
                }
                else
                {
                    finalReplacement = finalReplacement.Replace("#editortype#", "Input");
                }

                finalReplacementsList.Add(finalReplacement);
            }

            return string.Join(Environment.NewLine, finalReplacementsList);
        }

        private string GenerateShapePropertyDisplays(IList<PropertyItem> items)
        {
            if (!items.Any(property => !property.SkipFromShapeTemplate))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "shapepropertydisplay.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in items.Where(property => !property.SkipFromShapeTemplate))
            {
                var finalReplacement = template.Replace("#propertyname#", item.Name);

                finalReplacementsList.Add(finalReplacement);
            }

            return string.Join(Environment.NewLine, finalReplacementsList);
        }

        private string GenerateMigrationsRecordProperties(IList<PropertyItem> items)
        {
            if (!items.Any(property => property.HybridInfoset))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "migrationsrecordproperty.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in items.Where(property => property.HybridInfoset))
            {
                var finalReplacement = template.Replace("#propertyname#", item.Name);
                finalReplacement = finalReplacement.Replace("#propertytype#", item.Type);

                finalReplacementsList.Add(finalReplacement);
            }

            return Environment.NewLine + string.Join(Environment.NewLine, finalReplacementsList);
        }
    }
}
