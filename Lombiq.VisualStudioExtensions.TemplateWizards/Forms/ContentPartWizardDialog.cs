using Lombiq.VisualStudioExtensions.TemplateWizards.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions.TemplateWizards.Forms
{
    public partial class ContentPartWizardDialog : Form
    {
        public BindingList<PropertyItem> PropertyItems { get; set; }
        public bool UpdatePlacementInfoIfExists { get; set; }


        public ContentPartWizardDialog()
        {
            InitializeComponent();

            PropertyItems = new BindingList<PropertyItem>();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var source = new BindingSource(PropertyItems, null);

            dataGridView1.DataSource = source;

        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult = DialogResult.OK;

                Close();
            }
        }

        private void updatePlacementCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePlacementInfoIfExists = updatePlacementCheckBox.Checked;
        }
    }
}
