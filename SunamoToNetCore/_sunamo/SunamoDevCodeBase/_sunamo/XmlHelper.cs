namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo;

internal class XmlHelper
{

    /// <summary>
    /// Attr.
    /// </summary>
    internal static string? Attr(XmlNode node, string attributeName)
    {
        var argument = GetAttributeWithName(node, attributeName);
        if (argument != null)
        {
            return argument.Value;
        }
        return null;
    }

    internal static XmlAttribute? FoundedNode = null;

    /// <summary>
    /// Get attribute with name.
    /// </summary>
    internal static XmlNode? GetAttributeWithName(XmlNode node, string attributeName)
    {
        foreach (XmlAttribute attribute in node.Attributes!)
        {
            if (attribute.Name == attributeName)
            {
                FoundedNode = attribute;
                return attribute;
            }
        }
        return null;
    }
}