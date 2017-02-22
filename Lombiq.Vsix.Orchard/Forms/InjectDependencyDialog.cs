using Lombiq.Vsix.Orchard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard.Forms
{
    public partial class InjectDependencyDialog : Form
    {
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;


        public string DependencyName { get { return dependencyNameTextBox.Text; } }
        public string PrivateFieldName { get { return fieldNameTextBox.Text; } }


        public InjectDependencyDialog(IEnumerable<IFieldNameFromDependencyGenerator> fieldNameGenerators)
        {
            _fieldNameGenerators = fieldNameGenerators;

            InitializeComponent();
        }


        public string GenerateFieldName(string dependency, bool useShortName = false)
        {
            var fieldNameGenerator = _fieldNameGenerators
                .OrderByDescending(service => service.Priority)
                .First(service => service.CanGenerate(dependency));

            return fieldNameGenerator.Generate(dependency, useShortName);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ActiveControl = dependencyNameTextBox;
        }


        private void dependencyNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (DependencyName.Length == 0)
            {
                fieldNameTextBox.Text = string.Empty;
            }
            else
            {
                fieldNameTextBox.Text = GenerateFieldName(DependencyName, generateShortFieldNameCheckBox.Checked);
            }
        }
    }
}
