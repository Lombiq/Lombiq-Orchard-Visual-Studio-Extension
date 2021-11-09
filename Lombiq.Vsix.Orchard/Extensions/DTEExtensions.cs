namespace EnvDTE
{
    public static class DteExtensions
    {
        public static bool SolutionIsOpen(this DTE dte) => dte.Solution.IsOpen;
    }
}
