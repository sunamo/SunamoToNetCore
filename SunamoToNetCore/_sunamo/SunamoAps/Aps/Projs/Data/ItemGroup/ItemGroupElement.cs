namespace SunamoToNetCore._sunamo.SunamoAps.Aps.Projs.Data.ItemGroup;

internal abstract class ItemGroupElement
{
    /// <summary>
    /// Initializes a new instance of ItemGroupElement.
    /// </summary>
    public ItemGroupElement(string fullPath)
    {
        this.FullPath = fullPath;
    }

    // Must exist. From it and relative structure, other properties are filled.
    public string FullPath { get; set; }

    public string Include { get; set; } = null!;

    /// <summary>
    /// To xml.
    /// </summary>
    public abstract XmlNode ToXml(XmlDocument xmlDocument);
}