namespace System
{
    static class StringExtensions
    {
        public static string TrimEnd(this string input, string suffixToRemove)
        {
            return !string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(suffixToRemove) && input.EndsWith(suffixToRemove) ?
                input.Substring(0, input.Length - suffixToRemove.Length) :
                input;
        }
    }
}
