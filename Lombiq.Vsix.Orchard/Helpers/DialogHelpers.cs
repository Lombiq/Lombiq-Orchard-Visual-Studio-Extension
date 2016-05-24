using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard.Helpers
{
    internal static class DialogHelpers
    {
        public static void Information(string message, string caption = null)
        {
            MessageBox.Show(message, caption ?? "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Error(string message, string caption = null)
        {
            MessageBox.Show(message, caption ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Warning(string message, string caption = null)
        {
            MessageBox.Show(message, caption ?? "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
