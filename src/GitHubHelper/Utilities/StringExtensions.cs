namespace GitHubHelper.Utilities
{
    public static class StringExtensions
    {
        public static string MakeSafeName(this string fileName)
        {
            return fileName
                .Trim()
                .ToLower()
                .Replace(' ', '-')
                .Replace('/', '-')
                .Replace('.', '-')
                .Replace('\'', '-')
                .Replace(',', '-')
                .Replace('\\', '-')
                .Replace('#', '-')
                .Replace("---", "-");
        }
    }
}