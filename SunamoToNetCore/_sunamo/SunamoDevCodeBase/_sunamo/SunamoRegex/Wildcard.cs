namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoRegex;

// Represents a wildcard running on the System.Text.RegularExpressions engine.
internal class Wildcard : Regex
{

    /// <summary>
    /// Initializes a new instance of Wildcard.
    /// </summary>
    internal Wildcard(string pattern)
    : base(WildcardToRegex(pattern))
    {
    }

    /// <summary>
    /// Initializes a new instance of Wildcard.
    /// </summary>
    internal Wildcard(string pattern, RegexOptions options)
    : base(WildcardToRegex(pattern), options)
    {
    }

    /// <summary>
    /// Wildcard to regex.
    /// </summary>
    internal static string WildcardToRegex(string pattern) =>
        "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
}