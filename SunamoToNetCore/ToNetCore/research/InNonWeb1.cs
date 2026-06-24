namespace SunamoToNetCore;

public partial class MoveToNet5
{
    private
    async Task
    ReplaceOrRemoveFile(Func<string, string>? additionalReplacements, string xmlElementName, List<string> referencesToModify, string csprojFilePath, string? replacementReference = null)
    {
        bool isReplacing = replacementReference != null;
        referenceClosingTag = "</" + xmlElementName + ">";
        var xmlDocumentResult =
    await
        XmlDocumentsCache.Get(csprojFilePath);
        if (MayExcHelper.MayExc(xmlDocumentResult.Exc))
        {
            return;
        }

        var originalXmlContent = xmlDocumentResult.Data.OuterXml;
        string modifiedXmlContent = originalXmlContent;
        int firstReplacementIndex = -1;
        int closingTagIndex = -1;
        bool isCombiningTags = false;
        foreach (var reference in referencesToModify)
        {
            isCombiningTags = false;
            firstReplacementIndex = -1;
            closingTagIndex = -1;
            if (isReplacing)
            {
                modifiedXmlContent = SHReplace.ReplaceWithIndex(modifiedXmlContent, "<" + xmlElementName + " Include=\"" + reference + "\" />", string.Empty, ref firstReplacementIndex);
                modifiedXmlContent = SHReplace.ReplaceWithIndex(modifiedXmlContent, "<" + xmlElementName + " Include=\"" + reference + "\"/>", string.Empty, ref firstReplacementIndex);
                modifiedXmlContent = SHReplace.ReplaceWithIndex(modifiedXmlContent, "<" + xmlElementName + " Include=\"" + reference + "\"></" + xmlElementName + ">", string.Empty, ref firstReplacementIndex);
            }
            else
            {
                modifiedXmlContent = modifiedXmlContent.Replace("<" + xmlElementName + " Include=\"" + reference + "\" />", string.Empty);
                modifiedXmlContent = modifiedXmlContent.Replace(GetReferenceShortest(xmlElementName, reference), string.Empty);
                modifiedXmlContent = modifiedXmlContent.Replace("<" + xmlElementName + " Include=\"" + reference + "\"></" + xmlElementName + ">", string.Empty);
            }

            if (firstReplacementIndex == -1)
            {
                string tagToFind = ReferenceLongest(xmlElementName, reference);
                firstReplacementIndex = modifiedXmlContent.IndexOf(tagToFind);
                isCombiningTags = true;
            }

            if (firstReplacementIndex != -1 && isCombiningTags)
            {
                modifiedXmlContent = modifiedXmlContent.Remove(firstReplacementIndex, ReferenceLongest(xmlElementName, reference).Length);
                closingTagIndex = modifiedXmlContent.IndexOf(referenceClosingTag, firstReplacementIndex);
                modifiedXmlContent = modifiedXmlContent.Remove(closingTagIndex, referenceClosingTag.Length);
            }

            if (firstReplacementIndex != -1)
            {
                if (isReplacing)
                {
                    var shortestReferenceTag = GetReferenceShortest(xmlElementName, replacementReference!);
                    modifiedXmlContent = modifiedXmlContent.Insert(firstReplacementIndex, shortestReferenceTag);
                }
            }
        }

        if (additionalReplacements != null)
        {
            modifiedXmlContent = additionalReplacements(modifiedXmlContent);
        }

        if (modifiedXmlContent != originalXmlContent)
        {
            await XmlDocumentsCache.Set(csprojFilePath, modifiedXmlContent);
            await
            TF.WriteAllText(csprojFilePath, modifiedXmlContent);
        }

        ThisApp.Check = false;
    }

    private static string ReferenceLongest(string xmlElementName, string reference) =>
        "<" + xmlElementName + " Include=\"" + reference + "\">";

    private static string GetReferenceShortest(string xmlElementName, string reference) =>
        "<" + xmlElementName + " Include=\"" + reference + "\"/>";

    // Don't run, wait whether will be really needed to run
    // it could do more harm than good
    private
    async Task
    CommentAssemblyInfoCsFiles(ILogger logger)
    {
        var projectsData = WebAndNonWebProjects(logger);
        var lineCommentPrefix = "//";
        foreach (var projectPath in projectsData.Item2)
        {
            var projectDirectory = FS.GetDirectoryName(projectPath);
            var assemblyInfoFiles = FSGetFiles.GetFiles(logger, projectDirectory, "AssemblyInfo.cs", true);
            foreach (var assemblyInfoFile in assemblyInfoFiles)
            {
                var fileLines =
    await
                TF.ReadAllLines(assemblyInfoFile);
                if (!fileLines!.All(line => line.StartsWith(lineCommentPrefix)))
                {
                    CA.StartingWith(lineCommentPrefix, fileLines!);
                    await TF.WriteAllLines(assemblyInfoFile, fileLines!);
                }
            }
        }
    }
}
