namespace SunamoDevCodeCore;

// Druhá část je v ToNugets
// Vložit sem jak toto šílenství s nugety skončí
internal class GlobalUsingsHelper
{
    public const string globalUsing = "global using ";
    public const string global = "global ";

    public static ParseGlobalUsingsResult Parse(List<string> list)
    {
        var result = new ParseGlobalUsingsResult();
        foreach (var item in list)
            if (item.StartsWith(globalUsing))
            {
                result.GlobalUsings.Add(item.Replace(globalUsing, "").Replace(";", ""));
            }
            else if (item.StartsWith(global) && item.Contains("&"))
            {
                var kvp = ParseSymbol(item);
                result.GlobalSymbols.Add(kvp.Key, kvp.Value);
            }

        return result;
    }

    private static KeyValuePair<string, string> ParseSymbol(string item)
    {
        item = item.TrimEnd(';');
        var parameter = item.Split('=');
        return new KeyValuePair<string, string>(parameter[0].Replace(global, ""), parameter[1]);
    }
}