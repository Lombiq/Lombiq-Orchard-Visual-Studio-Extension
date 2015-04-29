using System;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions.Forms
{
    public partial class AddModuleDialog : Form
    {
        public string ProjectName { get { return textBox1.Text; } set { textBox1.Text = value; } }

        public string Author { get { return textBox2.Text; } set { textBox2.Text = value; } }

        public string Website { get { return textBox3.Text; } set { textBox3.Text = value; } }

        public string Description { get { return textBox4.Text; } set { textBox4.Text = value; } }


        public AddModuleDialog()
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
