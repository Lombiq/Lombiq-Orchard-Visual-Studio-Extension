using EnvDTE;
using Lombiq.Vsix.Orchard.TemplateWizards.Forms;
using Lombiq.Vsix.Orchard.TemplateWizards.Models;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Lombiq.Vsix.Orchard.TemplateWizards
{
    public class ContentPartWizard : IWizard
    {
        private const bool DefaultUpdatePlacementIfExists = false;


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


        private ContentPartWizardContext _context;


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
                // Create and initialize the context first.
                _context = new ContentPartWizardContext();

                var dte = automationObject as DTE;
                if (dte != null)
                {
                    var activeProjects = (Array)dte.ActiveSolutionProjects;

                    if (activeProjects.Length > 0)
                    {
                        var activeProject = (Project)activeProjects.GetValue(0);

                        _context.TargetProjectPath = Path.GetDirectoryName(activeProject.FullName);
                    }
                }


                // Gather additional information from the user.
                using (var dialog = new ContentPartWizardDialog())
                {
                    var dialogResult = dialog.ShowDialog();

                    // The form has been filled.
                    if (dialogResult == DialogResult.OK)
                    {
                        _context.PropertyItems = dialog.PropertyItems;
                        _context.UpdatePlacementInfoIfExists = dialog.UpdatePlacementInfoIfExists;
                    }
                    // It was cancelled so don't create any file in the project.
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        _context.Cancelled = true;

                        return;
                    }
                    // It was skipped so use default values.
                    else
                    {
                        _context.UpdatePlacementInfoIfExists = DefaultUpdatePlacementIfExists;
                    }
                }


                // Create the necessary replacements.
                var contentPartNameWithoutSuffix = string.Empty;
                replacementsDictionary.TryGetValue("$safeitemname$", out contentPartNameWithoutSuffix);

                _context.ContentPartNameWithoutSuffix = contentPartNameWithoutSuffix.TrimEnd("Part");
                _context.FullContentPartName = _context.ContentPartNameWithoutSuffix + "Part";

                var settingsPartName = _context.ContentPartNameWithoutSuffix.TrimEnd("Settings");

                // Add custom parameters.
                replacementsDictionary.Add("$contentpartname$", contentPartNameWithoutSuffix);
                replacementsDictionary.Add("$settingscontentpartname$", settingsPartName);

                replacementsDictionary.Add("$infosetproperties$", BuildInfosetPropertiesReplacement(_context));
                replacementsDictionary.Add("$virtualproperties$", BuildVirtualPropertiesReplacement(_context));
                replacementsDictionary.Add("$shapepropertyeditors$", BuildShapePropertyEditorsReplacement(_context));
                replacementsDictionary.Add("$shapepropertydisplays$", BuildShapePropertyDisplaysReplacement(_context));
                replacementsDictionary.Add("$migrationsrecordproperties$", BuildMigrationsRecordPropertiesReplacement(_context));
                replacementsDictionary.Add("$migrationsrecordindexes$", BuildMigrationsRecordIndexesReplacement(_context));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            if (_context.Cancelled) return false;

            if (Path.GetFileName(filePath) == "Placement.info.template")
            {
                try
                {
                    var placementInfoPath = Path.Combine(_context.TargetProjectPath, "Placement.info");
                    if (File.Exists(placementInfoPath))
                    {
                        if (!_context.UpdatePlacementInfoIfExists) return false;

                        var placementInfo = XDocument.Load(placementInfoPath);

                        var displayShapePlaceAttribute = new XAttribute("Parts_" + _context.ContentPartNameWithoutSuffix, "Content: 5");
                        var editorShapePlaceAttribute = new XAttribute("Parts_" + _context.ContentPartNameWithoutSuffix + "_Edit", "Content: 5");

                        if (placementInfo.Root == null)
                        {
                            placementInfo.Add(new XElement("Placement", new XElement("Place")));
                        }

                        var firstPlaceTag = placementInfo.Root.Element("Place");
                        if (firstPlaceTag != null)
                        {
                            firstPlaceTag.Add(displayShapePlaceAttribute, editorShapePlaceAttribute);
                        }
                        else
                        {
                            placementInfo.Root.Add(new XElement("Place", displayShapePlaceAttribute, editorShapePlaceAttribute));
                        }

                        placementInfo.Save(placementInfoPath, SaveOptions.None);

                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return true;
        }


        private static string BuildInfosetPropertiesReplacement(ContentPartWizardContext context)
        {
            if (!context.PropertyItems.Any())
                return "";

            var infosetPropertyTemplatePath = Path.Combine(CodeTemplateLocation, "infoset.template");
            var hybridInfosetPropertyTemplatePath = Path.Combine(CodeTemplateLocation, "hybridinfoset.template");

            var infosetPropertyTemplate = File.Exists(infosetPropertyTemplatePath) ? File.ReadAllText(infosetPropertyTemplatePath) : "";
            var hybridInfosetPropertyTemplate = File.Exists(hybridInfosetPropertyTemplatePath) ? File.ReadAllText(hybridInfosetPropertyTemplatePath) : "";

            var finalPropertiesList = new List<string>();
            foreach (var item in context.PropertyItems)
            {
                var finalProperty = item.HybridInfoset ? hybridInfosetPropertyTemplate.Replace("#propertyname#", item.Name) : infosetPropertyTemplate.Replace("#propertyname#", item.Name);
                finalProperty = finalProperty.Replace("#propertytype#", item.Type);

                finalPropertiesList.Add(finalProperty);
            }

            return string.Join(Environment.NewLine + Environment.NewLine, finalPropertiesList);
        }

        private static string BuildVirtualPropertiesReplacement(ContentPartWizardContext context)
        {
            if (!context.PropertyItems.Any(property => property.HybridInfoset))
                return "";

            var virtualPropertyTemplatePath = Path.Combine(CodeTemplateLocation, "virtualproperty.template");

            var virtualPropertyTemplate = File.Exists(virtualPropertyTemplatePath) ? File.ReadAllText(virtualPropertyTemplatePath) : "";

            var finalPropertiesList = new List<string>();
            foreach (var item in context.PropertyItems.Where(property => property.HybridInfoset))
            {
                var finalProperty = virtualPropertyTemplate
                    .Replace("#propertyname#", item.Name)
                    .Replace("#propertytype#", item.Type);

                finalPropertiesList.Add(finalProperty);
            }

            return string.Join(Environment.NewLine, finalPropertiesList);
        }

        private static string BuildShapePropertyEditorsReplacement(ContentPartWizardContext context)
        {
            if (!context.PropertyItems.Any(property => !property.SkipFromShapeTemplate))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "shapepropertyeditor.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in context.PropertyItems.Where(property => !property.SkipFromShapeTemplate))
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
                    finalReplacement = finalReplacement.Replace("#editortype#", "Editor");
                }

                finalReplacementsList.Add(finalReplacement);
            }

            return string.Join(Environment.NewLine, finalReplacementsList);
        }

        private static string BuildShapePropertyDisplaysReplacement(ContentPartWizardContext context)
        {
            if (!context.PropertyItems.Any(property => !property.SkipFromShapeTemplate))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "shapepropertydisplay.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in context.PropertyItems.Where(property => !property.SkipFromShapeTemplate))
            {
                var finalReplacement = template.Replace("#propertyname#", item.Name);

                finalReplacementsList.Add(finalReplacement);
            }

            return string.Join(Environment.NewLine, finalReplacementsList);
        }

        private static string BuildMigrationsRecordPropertiesReplacement(ContentPartWizardContext context)
        {
            if (!context.PropertyItems.Any(property => property.HybridInfoset))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "migrationsrecordproperty.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in context.PropertyItems.Where(property => property.HybridInfoset))
            {
                var finalReplacement = template
                    .Replace("#propertyname#", item.Name)
                    .Replace("#propertytype#", item.Type);

                finalReplacementsList.Add(finalReplacement);
            }

            return Environment.NewLine + string.Join(Environment.NewLine, finalReplacementsList);
        }

        private static string BuildMigrationsRecordIndexesReplacement(ContentPartWizardContext context)
        {
            if (!context.PropertyItems.Any(property => property.HybridInfoset))
                return "";

            var templatePath = Path.Combine(CodeTemplateLocation, "migrationsrecordindex.template");

            var template = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "";

            var finalReplacementsList = new List<string>();
            foreach (var item in context.PropertyItems.Where(property => property.HybridInfoset))
            {
                var finalReplacement = template
                    .Replace("#propertyname#", item.Name)
                    .Replace("#recordname#", context.FullContentPartName + "Record");

                finalReplacementsList.Add(finalReplacement);
            }

            return string.Join(Environment.NewLine, finalReplacementsList);
        }


        private class ContentPartWizardContext
        {
            public string FullContentPartName { get; set; }
            public string ContentPartNameWithoutSuffix { get; set; }
            public string TargetProjectPath { get; set; }
            public IEnumerable<PropertyItem> PropertyItems { get; set; }
            public bool UpdatePlacementInfoIfExists { get; set; }
            public bool Cancelled { get; set; }


            public ContentPartWizardContext()
            {
                PropertyItems = Enumerable.Empty<PropertyItem>();
            }
        }
    }
}
