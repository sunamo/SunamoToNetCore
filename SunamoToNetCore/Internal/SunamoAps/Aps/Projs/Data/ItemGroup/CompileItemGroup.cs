namespace SunamoAps.Aps.Projs.Data.ItemGroup;

internal class CompileItemGroup : ItemGroupElement
{
    public string? Link = null;

    // Full path to the file - use in checking whether exists already, not for simple adding
    public string? FullPathFile = null;

    // A1 - To csproj. Must exists. From it and relative structure is filled other properties
    public CompileItemGroup(string fullPath) : base(fullPath)
    {
    }

    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        throw new Exception("Make it with ");
    }
}