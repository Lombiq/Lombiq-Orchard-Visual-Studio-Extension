using System;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions.Forms
{
    public partial class InjectDependencyDialog : Form
    {
        public string DependencyName { get { return textBox1.Text; } }


        public InjectDependencyDialog()
        {
            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ActiveControl = textBox1;    
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
