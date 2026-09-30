namespace SunamoAps.Aps.Projs.Data.ItemGroup;

internal class NoneItemGroup : ItemGroupElement
{
    public NoneItemGroup() : base(string.Empty)
    {
    }

    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        var generator = new XmlGenerator();
        generator.WriteNonPairTagWithAttr(VsProjectItemTypes.None, "Include", Include);
        XmlNode node = XH.ReturnXmlNode(generator.ToString()!)!;
        return node;
    }
}