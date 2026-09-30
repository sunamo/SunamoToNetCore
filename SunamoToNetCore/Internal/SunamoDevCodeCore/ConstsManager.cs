namespace SunamoDevCodeCore;

internal class ConstsManager
{
    public string PathXlfKeys { get; }

    #region Work with consts in XlfKeys

    // Add to XlfKeys.cs from xlf
    // Must manually call XlfResourcesH.SaveResouresToRL(BasePathsHelper.sunamoProject) before
    // Called externally from MiAddTranslationWhichIsntInKeys_Click
    public
        async Task
        AddConsts(List<string> keysAll, List<string>? valuesAll = null)
    {
        var keys =
            await
                GetConsts();

        AddKeysConsts(keysAll, keys.Item2, keys.Item3, valuesAll);
    }


    private static void AddConst(CSharpGenerator generator, string constantName, string constantValue)
    {
        generator.Field(1, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", constantName, true, constantValue);
    }


    // Get consts which exists in XlfKeys.cs
    public
        async Task<OutRef3DC<List<string>, int, List<string>>>
        GetConsts()
    {
        var firstLineIndex = -1;

        var lines = SHGetLines.GetLines(
            await
                FileAsync.ReadAllTextAsync(PathXlfKeys)).ToList();

        var keys = CSharpParser.ParseConsts(lines, out firstLineIndex);
        return new OutRef3DC<List<string>, int, List<string>>(keys, firstLineIndex, lines);
    }

    public ConstsManager(string pathXlfKeys, Func<string, bool> shouldIncludeInXlfKeys)
    {
        PathXlfKeys = pathXlfKeys;
        this.shouldIncludeInXlfKeys = shouldIncludeInXlfKeys;
    }

    private readonly Func<string, bool> shouldIncludeInXlfKeys;

    public void AddKeysConsts(List<string> keysAll, int first, List<string> lines, List<string>? valuesAll = null)
    {
        var generator = new CSharpGenerator();

        var prefix = string.Empty;

        if (valuesAll == null)
        {
            valuesAll = keysAll;
        }
        else
        {
            if (valuesAll.Count != keysAll.Count)
                ThrowEx.DifferentCountInLists("keysAll", keysAll, "valuesAll", valuesAll);
        }

        for (var i = 0; i < keysAll.Count; i++)
        {
            var key = keysAll[i];
            var constantValue = valuesAll[i];

            if (shouldIncludeInXlfKeys(key))
            {
                prefix = string.Empty;

                if (char.IsDigit(key[0])) prefix = "_";

                // Inline from SHTrim.TrimLeadingNumbersAtStart - removes digits from start of string
                var trimmedKey = key;
                while (trimmedKey.Length > 0 && char.IsDigit(trimmedKey[0]))
                {
                    trimmedKey = trimmedKey.Substring(1);
                }
                AddConst(generator, prefix + trimmedKey, constantValue);
            }
        }

        lines.Insert(first, generator.ToString());


        CA.RemoveStringsEmpty2(lines);

        _ = FileAsync.WriteAllLinesAsync(PathXlfKeys, lines);
    }

    #endregion
}