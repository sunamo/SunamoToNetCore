namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal class BTS
{
    /// <summary>
    /// Replace.
    /// </summary>
    internal static string Replace(ref string text, bool isReplacingCommaForDot)
    {
        if (isReplacingCommaForDot) text = text.Replace(",", ".");

        return text;
    }

    internal static int LastInt = -1;
    internal static float LastFloat = -1;

    /// <summary>
    /// Is float.
    /// </summary>
    internal static bool IsFloat(string text, bool isReplacing = false)
    {
        if (text == null) return false;

        Replace(ref text, isReplacing);
        return float.TryParse(text.Replace(",", "."), out LastFloat);
    }

    /// <summary>
    /// Is int.
    /// </summary>
    internal static bool IsInt(string text, bool isThrowingExceptionIfIsFloat = false, bool isReplacingCommaForDot = false)
    {
        if (text == null) return false;

        text = text.Replace(" ", "");
        Replace(ref text, isReplacingCommaForDot);

        var result = int.TryParse(text, out LastInt);
        if (!result)
            if (IsFloat(text))
                if (isThrowingExceptionIfIsFloat)
                    throw new Exception(text + " is float but is calling IsInt");

        return result;
    }
}
