namespace SunamoToNetCore._sunamo.SunamoAps.Aps.Helpers;

internal class VersionHelper
{
    /// <summary>
    /// Remove parts which is zero.
    /// </summary>
    public static string RemovePartsWhichIsZero(Version version)
    {
        const string dotZero = ".0";

        var text = version.ToString();
        while (text.EndsWith(dotZero))
        {
            text = SHTrim.Trim(text, dotZero);
        }
        return text;
    }
}