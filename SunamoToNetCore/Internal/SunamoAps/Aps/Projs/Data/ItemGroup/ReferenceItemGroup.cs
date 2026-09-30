namespace SunamoAps.Aps.Projs.Data.ItemGroup;

internal class ReferenceItemGroup : ItemGroupElement
{
    public static readonly Type Type = typeof(ReferenceItemGroup);

    public Dictionary<string, string> IncludeParts { get; set; } = new Dictionary<string, string>();

    // Relative path from project folder to DLL used as HintPath.
    // Used later in AddItemGroup2 as HintPath.
    private string? relativePathDllToProjectFolderHintPath = null;

    public ReferenceItemGroup(string fullPathDllInclude, string fullPathCsproj, string relativePathDllToProjectFolderHintPath) : base(fullPathCsproj)
    {
        Include = fullPathDllInclude;
        this.relativePathDllToProjectFolderHintPath = relativePathDllToProjectFolderHintPath;
    }

    public ReferenceItemGroup(string fullPathCsproj, XmlNode xmlElement) : base(fullPathCsproj)
    {
        Include = XmlHelper.Attr(xmlElement, "Include")!;
        if (Include!.Contains(","))
        {
            var includeParts = SHSplit.Split(Include, ",");
            CA.Trim(includeParts);
            foreach (var item in includeParts)
            {
                if (item.Contains("="))
                {
                    var parts = SHSplit.Split(item, "=");
                    IncludeParts.Add(parts[0], parts[1]);
                }
            }
        }
    }

    public string Version => FromInclude("Version");

    private string FromInclude(string key)
    {
        if (IncludeParts.ContainsKey(key))
        {
            return IncludeParts[key];
        }
        return string.Empty;
    }

    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        var generator = new XmlGenerator();
        generator.WriteTagWithAttrsCheckNull(VsProjectItemTypes.Reference, "Include", Include);
        generator.TerminateTag(VsProjectItemTypes.Reference);
        XmlNode node = XH.ReturnXmlNode(generator.ToString()!);
        return node;
    }
}