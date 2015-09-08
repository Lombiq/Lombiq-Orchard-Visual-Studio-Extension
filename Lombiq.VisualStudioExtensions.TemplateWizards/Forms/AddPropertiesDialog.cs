using Lombiq.VisualStudioExtensions.TemplateWizards.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions.TemplateWizards.Forms
{
    public partial class AddPropertiesDialog : Form
    {
        public BindingList<PropertyItem> PropertyItems { get; set; }


        public AddPropertiesDialog()
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
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                this.Close();
            }
        }
    }
}
