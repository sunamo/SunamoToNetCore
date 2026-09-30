namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoTextOutputGenerator;

internal class TextOutputGeneratorArgs
{
    internal bool HeaderWrappedEmptyLines = true;
    internal bool InsertCount = false;
    internal string WhenNoEntries = "No entries";
    internal string Delimiter = Environment.NewLine;
    /// <summary>
    /// Initializes a new instance of TextOutputGeneratorArgs.
    /// </summary>
    internal TextOutputGeneratorArgs()
    {
    }
    /// <summary>
    /// Initializes a new instance of TextOutputGeneratorArgs.
    /// </summary>
    internal TextOutputGeneratorArgs(bool headerWrappedEmptyLines, bool insertCount)
    {
        this.HeaderWrappedEmptyLines = headerWrappedEmptyLines;
        this.InsertCount = insertCount;
    }
}