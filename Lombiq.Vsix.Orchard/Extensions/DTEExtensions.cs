namespace EnvDTE
{
    public static class DteExtensions
    {
        public static bool SolutionIsOpen(this DTE dte)
        {
            // We should never get an exception here. This is just to ensure we access DTE on the main thread and get rid of the VSTHRD010 violation.
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return dte.Solution.IsOpen;
        }
    }
}
