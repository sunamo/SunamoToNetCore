namespace SunamoToNetCore._sunamo.SunamoAps.Aps.Projs;

internal partial class VsProjectsFileHelper
{
    // Adds an ItemGroup element to an SDK-style .csproj file.
    // Used in: MoveClassElementIntoSharedFileUC, AddFilesToCsproj.
    // Works with pure XML classes, primarily for Compile tags.
    /// <summary>
    /// Add item group sdk style.
    /// </summary>
    public static
            async Task
                AddItemGroupSdkStyle(string csprojPath, ItemGroups itemGroups, ItemGroupElement itemGroupElement, bool isWritingToStorage)
    {
        ResultWithException<XmlDocument> xmlDocumentResult = null!;
        await
XmlDocumentsCache.Get(csprojPath);
        if (MayExcHelper.MayExc(xmlDocumentResult.Exc!))
        {
            return;
        }
        if (xmlDocumentResult.Data == null)
        {
            return;
        }
        var xmlDocument = new XmlDocument();
        var content = xmlDocumentResult.Data.OuterXml;
        string from = "xmlns=\"";
        string to = "xmlns2=\"";
        content = content.Replace(from, to);
        XmlNamespaceManager namespaceManager;
        XmlNode itemGroup, parent, project;
        LoadXml(itemGroups, xmlDocument, content, out namespaceManager, out itemGroup, out parent, out project);
        if (itemGroup == null)
        {
            #region No ItemGroup, add new
            itemGroup = AddNewItemGroup(csprojPath, xmlDocument, namespaceManager, itemGroup!, project);
            #endregion
        }
        else
        {
            var itemGroupsString = itemGroups.ToString();
            #region Item group isnt null
            if (xmlDocument.SelectSingleNode(@"//Project/ItemGroup/" + itemGroupsString + "[@Include='" + itemGroupElement.Include + "']", namespaceManager) != null)
            {
                // Already Exists
                return;
            }
            parent = xmlDocument.SelectSingleNode(@"//Project/ItemGroup/" + itemGroupsString, namespaceManager)!;
            if (parent == null)
            {
                if (itemGroups == ItemGroups.Compile)
                {
                    // Is .net standard project which doesn't have any compile
                    return;
                }
            }
            if (parent != null)
            {
                itemGroup = parent.ParentNode!;
            }
            else
            {
                itemGroup = AddNewItemGroup(csprojPath, xmlDocument, namespaceManager, itemGroup!, project);
            }
            #endregion
        }
        var itemGroupElementType = itemGroupElement.GetType();
        if (itemGroupElementType == typeof(CompileItemGroup))
        {
            #region Add ItemGroup
            var compileItem = (CompileItemGroup)itemGroupElement;
            var xmlNode = compileItem.ToXml(xmlDocument);
            xmlNode = xmlDocument.ImportNode(xmlNode, true);
            itemGroup!.PrependChild(xmlNode);
            #endregion
        }
        else if (itemGroupElementType == ProjectReferenceItemGroup.Type)
        {
            var projectReferenceItem = (ProjectReferenceItemGroup)itemGroupElement;
            var xmlNode = projectReferenceItem.ToXml(xmlDocument);
            xmlNode = xmlDocument.ImportNode(xmlNode, true);
            itemGroup!.PrependChild(xmlNode);
        }
        else if (itemGroupElementType == ReferenceItemGroup.Type)
        {
            var referenceItem = (ReferenceItemGroup)itemGroupElement;
            var xmlNode = referenceItem.ToXml(xmlDocument);
            xmlNode = xmlDocument.ImportNode(xmlNode, true);
            itemGroup!.PrependChild(xmlNode);
        }
        else
        {
            ThrowEx.NotImplementedCase(itemGroupElementType);
        }
        await XmlDocumentsCache.Set(csprojPath, xmlDocument, isWritingToStorage);
    }
}