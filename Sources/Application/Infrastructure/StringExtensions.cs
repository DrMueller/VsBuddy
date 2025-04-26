namespace VsBuddy.Infrastructure
{
    public static class StringExtensions
    {
        public static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            {
                return input;
            }

            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}