using System;
using System.Linq;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions.Forms
{
    public partial class InjectDependencyDialog : Form
    {
        public string DependencyName { get { return textBox1.Text; } }

        public string PrivateFieldName { get { return textBox2.Text; } }


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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (DependencyName.Length == 0)
            {
                textBox2.Text = string.Empty;
            }
            else
            {
                textBox2.Text = GenerateFieldName(DependencyName, checkBox1.Checked);
            }
        }

        public static string GenerateFieldName(string dependency, bool useShortName = false)
        {
            if (dependency.Length < 2) return "_" + dependency.ToLowerInvariant();

            var cleanedDependency = dependency.Length > 1 && dependency.StartsWith("I") && char.IsUpper(dependency[1]) ? dependency.Substring(1) : string.Copy(dependency);

            if (dependency.Length < 2) return "_" + dependency.ToLowerInvariant();

            if (useShortName)
            {
                var upperCasedLetters = cleanedDependency.Where(letter => char.IsUpper(letter));

                return upperCasedLetters.Any() ? ("_" + new string(upperCasedLetters.ToArray())).ToLowerInvariant() : "_" + cleanedDependency[0];
            }

            return string.Format("_{0}{1}", char.ToLower(cleanedDependency[0]), cleanedDependency.Substring(1));
        }
    }
}
