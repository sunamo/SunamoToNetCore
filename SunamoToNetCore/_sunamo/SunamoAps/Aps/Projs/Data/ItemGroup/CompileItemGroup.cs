namespace SunamoToNetCore._sunamo.SunamoAps;

internal class CompileItemGroup : ItemGroupElement
{
    public string? Link = null;

    // Full path to the file - use in checking whether exists already, not for simple adding
    public string? FullPathFile = null;

    // A1 - To csproj. Must exists. From it and relative structure is filled other properties
    /// <summary>
    /// Initializes a new instance of CompileItemGroup.
    /// </summary>
    public CompileItemGroup(string fullPath) : base(fullPath)
    {
    }

    /// <summary>
    /// To xml.
    /// </summary>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        throw new Exception("Make it with ");
    }
}