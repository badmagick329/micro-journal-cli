namespace NotesCli.Console.Common;

static class StringExtensions
{
    public static string TitleCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return text.Split(' ')
            .Select(word => word[..1].ToUpper() + word[1..].ToLower())
            .Aggregate((current, next) => current + " " + next);
    }
}
