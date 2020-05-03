namespace EnvDTE
{
    public static class DTEExtensions
    {
        public static bool SolutionIsOpen(this DTE dte) => dte.Solution.IsOpen;
    }
}
