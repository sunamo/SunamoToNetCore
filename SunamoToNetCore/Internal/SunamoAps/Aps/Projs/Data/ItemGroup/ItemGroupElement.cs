namespace SunamoAps.Aps.Projs.Data.ItemGroup;

internal abstract class ItemGroupElement
{
    public ItemGroupElement(string fullPath)
    {
        this.FullPath = fullPath;
    }

    // Must exist. From it and relative structure, other properties are filled.
    public string FullPath { get; set; }

    public string Include { get; set; } = null!;

    public abstract XmlNode ToXml(XmlDocument xmlDocument);
}