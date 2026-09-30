namespace SunamoAps.Aps.Projs.Data.ItemGroup;

internal class PackageReferenceItemGroup : ItemGroupElement
{
    public string? Version { get; set; } = null;

    public static Type Type = typeof(PackageReferenceItemGroup);

    public PackageReferenceItemGroup(string fullCsprojPath) : base(fullCsprojPath)
    {
    }

    public PackageReferenceItemGroup(string fullCsprojPath, XmlNode xmlElement) : base(fullCsprojPath)
    {
        const string VersionAttributeName = "Version";
        Include = XmlHelper.Attr(xmlElement, "Include")!;
        Version = XmlHelper.InnerTextOfNode(xmlElement);
        if (Version == string.Empty)
        {
            Version = XmlHelper.Attr(xmlElement, VersionAttributeName)!;
        }
    }

    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        return null!;
    }
}