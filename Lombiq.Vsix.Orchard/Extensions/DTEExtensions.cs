namespace EnvDTE
{
    public static class DteExtensions
    {
        public static bool SolutionIsOpen(this DTE dte)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return dte.Solution.IsOpen;
        }
    }
}
