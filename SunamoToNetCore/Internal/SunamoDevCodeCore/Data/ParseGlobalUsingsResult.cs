namespace SunamoDevCodeCore.Data;

internal class ParseGlobalUsingsResult
{
    public List<string> GlobalUsings { get; set; } = new List<string>();

    public Dictionary<string, string> GlobalSymbols { get; set; } = new();
}