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


        public string DependencyName => dependencyNameTextBox.Text;
        public string PrivateFieldName => fieldNameTextBox.Text;


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

            ActiveControl = dependencyNameTextBox;
        }


        private void DependencyNameTextBoxTextChanged(object sender, EventArgs e) =>
            fieldNameTextBox.Text = DependencyName.Length == 0 ? 
                string.Empty : 
                GenerateFieldName(DependencyName, generateShortFieldNameCheckBox.Checked);
    }
}
