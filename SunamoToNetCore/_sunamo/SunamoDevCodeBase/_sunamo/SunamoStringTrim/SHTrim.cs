namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringTrim;

internal class SHTrim
{
    /// <summary>
    /// Trim.
    /// </summary>
    internal static string Trim(string text, string valueToTrim)
    {
        text = TrimStart(text, valueToTrim);
        text = TrimEnd(text, valueToTrim);

        return text;
    }

    // EN: Method TrimLeadingNumbersAtStart was removed - inlined in ConstsManager.cs:110

    /// <summary>
    /// Trim end.
    /// </summary>
    internal static string TrimEnd(string text, string suffix)
    {
        while (text.EndsWith(suffix)) return text.Substring(0, text.Length - suffix.Length);

        return text;
    }

    // EN: Method TrimStartAndEnd was removed - inlined in XmlLocalisationInterchangeFileFormat2.cs:691

    /// <summary>
    /// Trim start.
    /// </summary>
    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix))
        {
            text = text.Substring(prefix.Length);
        }

        return text;
    }

}