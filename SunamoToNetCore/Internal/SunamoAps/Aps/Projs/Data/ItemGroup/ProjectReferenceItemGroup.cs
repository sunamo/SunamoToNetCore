namespace SunamoAps.Aps.Projs.Data.ItemGroup;

internal class ProjectReferenceItemGroup : ItemGroupElement
{
    public static Type Type = typeof(ProjectReferenceItemGroup);

    public string? Project { get; set; } = null;

    public string? Name { get; set; } = null;

    public ProjectReferenceItemGroup(string fullPath) : base(fullPath)
    {
    }

    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        // .NET Framework also adds Name and ProjectGuid elements, but they are not required and it works the same without them. Tested with rebuild.
        var xmlContent = "<" + ItemGroups.ProjectReference + " Include=\"" + Include + "\" />";
        XmlNode node = XH.ReturnXmlNode(xmlContent.ToString());
        return node;
    }
}