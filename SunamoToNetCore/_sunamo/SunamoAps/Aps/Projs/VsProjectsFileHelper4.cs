namespace SunamoToNetCore._sunamo.SunamoAps;

internal partial class VsProjectsFileHelper
{
    /// <summary>
    /// Load xml.
    /// </summary>
    private static void LoadXml(ItemGroups itemGroups, XmlDocument xmlDocument, string content, out XmlNamespaceManager namespaceManager, out XmlNode itemGroup, out XmlNode parent, out XmlNode project)
    {
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(content);
        namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
        string wanted = itemGroups.ToString();
        itemGroup = xmlDocument.SelectSingleNode("//Project/ItemGroup", namespaceManager)!;
        parent = null!;
        project = xmlDocument.SelectSingleNode("//Project", namespaceManager)!;
    }
}