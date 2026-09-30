namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoString;

internal class SH
{
    #region SH.FirstCharUpper


    #endregion
    /// <summary>
    /// Is contained.
    /// </summary>
    internal static bool IsContained(string text, string pattern)
    {
        var (isNegated, actualPattern) = IsNegationTuple(pattern);
        pattern = actualPattern;

        if (isNegated && text.Contains(pattern))
            return false;
        if (!isNegated && !text.Contains(pattern)) return false;

        return true;
    }

    /// <summary>
    /// Is negation tuple.
    /// </summary>
    internal static (bool, string) IsNegationTuple(string pattern)
    {
        if (pattern[0] == '!')
        {
            pattern = pattern.Substring(1);
            return (true, pattern);
        }

        return (false, pattern);
    }

    /// <summary>
    /// Occurences of string in.
    /// </summary>
    internal static int OccurencesOfStringIn(string source, string searchText)
    {
        return source.Split(new[] { searchText }, StringSplitOptions.None).Length - 1;
    }

    /// <summary>
    /// Get text between simple.
    /// </summary>
    internal static string GetTextBetweenSimple(string text, string afterDelimiter, string beforeDelimiter, bool isThrowExceptionIfNotContains = true)
    {
        int foundIndex = int.MinValue;
        var result = GetTextBetween(text, afterDelimiter, beforeDelimiter, out foundIndex, 0, isThrowExceptionIfNotContains);
        return result!;
    }

    /// <summary>
    /// Get text between.
    /// </summary>
    internal static string? GetTextBetween(string text, string afterDelimiter, string beforeDelimiter, out int foundIndex, int startSearchingAt, bool isThrowExceptionIfNotContains = true)
    {
        string? result = null;
        foundIndex = text.IndexOf(afterDelimiter, startSearchingAt);
        int beforeIndex = text.IndexOf(beforeDelimiter, foundIndex + afterDelimiter.Length);
        bool isAfterFound = foundIndex != -1;
        bool isBeforeFound = beforeIndex != -1;
        if (isAfterFound && isBeforeFound)
        {
            foundIndex += afterDelimiter.Length;
            beforeIndex -= 1;
            // When I return between ( ), there must be +1
            var length = beforeIndex - foundIndex + 1;
            if (length < 1)
            {
                // EN: This was here before but logically it's nonsense
                // CZ: Takhle to tu bylo předtím ale logicky je to nesmysl
            }
            result = text.Substring(foundIndex, length).Trim();
        }
        else
        {
            if (isThrowExceptionIfNotContains)
            {
                ThrowEx.NotContains(text, afterDelimiter, beforeDelimiter);
            }
            else
            {
                // 24-1-21 return null instead of text
                return null;
                //result = text;
            }
        }

        return result;
    }

    /// <summary>
    /// Wrap with.
    /// </summary>
    internal static string WrapWith(string value, string wrapper)
    {
        return wrapper + value + wrapper;
    }

    /// <summary>
    /// Wrap with qm.
    /// </summary>
    internal static string WrapWithQm(string value)
    {
        var wrapper = "\"";
        return wrapper + value + wrapper;
    }

    #region SH.FirstCharUpper
    /// <summary>
    /// First char upper.
    /// </summary>
    internal static string FirstCharUpper(ref string text)
    {
        text = FirstCharUpper(text);
        return text;
    }


    /// <summary>
    /// First char upper.
    /// </summary>
    internal static string FirstCharUpper(string text)
    {
        if (text.Length == 1)
        {
            return text.ToUpper();
        }

        string rest = text.Substring(1);
        return text[0].ToString().ToUpper() + rest;
    }
    #endregion

    /// <summary>
    /// Match wildcard.
    /// </summary>
    internal static bool MatchWildcard(string name, string mask)
    {
        return IsMatchRegex(name, mask, '?', '*');
    }

    /// <summary>
    /// Is match regex.
    /// </summary>
    private static bool IsMatchRegex(string text, string pattern, char singleWildcard, char multipleWildcard)
    {
        // If I compared .vs with .vs, return false before
        if (text == pattern)
        {
            return true;
        }

        string escapedSingle = Regex.Escape(new string(singleWildcard, 1));
        string escapedMultiple = Regex.Escape(new string(multipleWildcard, 1));
        pattern = Regex.Escape(pattern);
        pattern = pattern.Replace(escapedSingle, ".");
        pattern = "^" + pattern.Replace(escapedMultiple, ".*") + "$";
        Regex regex = new(pattern);
        return regex.IsMatch(text);
    }


    /// <summary>
    /// Text without diacritic.
    /// </summary>
    internal static string TextWithoutDiacritic(string projName)
    {
        return projName.RemoveDiacritics();
    }

    /// <summary>
    /// Get text between.
    /// </summary>
    internal static string GetTextBetween(string content, string start, string end, bool throwExceptionIfNotContains = true)
    {
        return GetTextBetweenSimple(content, start, end, throwExceptionIfNotContains);
    }
}