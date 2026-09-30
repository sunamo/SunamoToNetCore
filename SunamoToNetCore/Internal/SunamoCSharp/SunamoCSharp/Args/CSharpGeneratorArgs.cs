namespace SunamoCSharp.Args;

internal class CSharpGeneratorArgs
{
    public bool AddHyphens { get; set; } = false;

    public bool CreateInstance { get; set; } = false;

    public bool AlsoField { get; set; } = false;

    public bool AddingValue { get; set; } = false;

    public bool UseCA { get; set; } = true;

    public string? SplitKeyWith { get; set; } = null;

    public bool CheckForNull { get; set; } = false;
}
