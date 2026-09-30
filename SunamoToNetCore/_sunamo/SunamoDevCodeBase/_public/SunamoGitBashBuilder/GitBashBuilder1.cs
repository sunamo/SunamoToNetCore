namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class GitBashBuilder : IGitBashBuilder
{

#pragma warning restore
    /// <summary>
    /// Cd.
    /// </summary>
    public void Cd(string key)
    {
        StringBuilder.AppendLine("cd " + SH.WrapWith(key, "\""));
    }

    /// <summary>
    /// Clear.
    /// </summary>
    public void Clear()
    {
        StringBuilder.Clear();
    }

    /// <summary>
    /// Append.
    /// </summary>
    public void Append(string text)
    {
        StringBuilder.Append(text + " ");
    }

    /// <summary>
    /// Append line.
    /// </summary>
    public void AppendLine(string text)
    {
        StringBuilder.AppendLine(text);
    }

    /// <summary>
    /// Append line.
    /// </summary>
    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    /// <summary>
    /// To string.
    /// </summary>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}
