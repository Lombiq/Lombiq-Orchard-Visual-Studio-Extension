namespace System
{
    internal static class StringExtensions
    {
        public static string TrimEnd(this string input, string suffixToRemove) =>
            !string.IsNullOrEmpty(input) &&
            !string.IsNullOrEmpty(suffixToRemove) &&
            input.EndsWith(suffixToRemove, StringComparison.InvariantCulture)
                ? input.Substring(0, input.Length - suffixToRemove.Length)
                : input;
    }
}
