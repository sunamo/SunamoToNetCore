namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoEnumsHelper;

internal class EnumHelper
{
    /// <summary>
    /// Parse.
    /// </summary>
    internal static T Parse<T>(string text, T defaultValue, bool isReturningDefaultIfNull = false)
        where T : struct
    {
        if (isReturningDefaultIfNull)
        {
            return defaultValue;
        }
        if (Enum.TryParse<T>(text, true, out var result))
        {
            return result;
        }

        return defaultValue;
    }
}