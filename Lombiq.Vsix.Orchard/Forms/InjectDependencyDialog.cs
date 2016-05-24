using System;
using System.Linq;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard.Forms
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
            
            return "_" + char.ToLower(cleanedDependency[0]) + cleanedDependency.Substring(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
