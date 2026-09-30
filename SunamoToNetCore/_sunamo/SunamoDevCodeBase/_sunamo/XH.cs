namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal class XH
{
    /// <summary>
    /// Return xml node.
    /// </summary>
    internal static XmlNode ReturnXmlNode(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(xml);
        return xmlDocument.FirstChild!;
    }
}
