namespace SunamoDevCode.ToNetCore.research;

public partial class MoveToNet5
{
    /// <summary>
    /// EN: Replaces or removes XML elements (like PackageReference, Reference) in a .csproj file
    /// CZ: Nahrazuje nebo odstraňuje XML elementy (jako PackageReference, Reference) v .csproj souboru
    /// </summary>
    /// <param name="additionalReplacements">
    /// EN: Optional function to perform additional string replacements on the XML content after the main operation
    /// CZ: Volitelná funkce pro provedení dalších textových nahrazení v XML obsahu po hlavní operaci
    /// </param>
    /// <param name="xmlElementName">
    /// EN: Name of the XML element to modify (e.g., "Reference", "PackageReference", "ProjectReference")
    /// CZ: Název XML elementu k úpravě (např. "Reference", "PackageReference", "ProjectReference")
    /// </param>
    /// <param name="referencesToModify">
    /// EN: List of reference Include values to replace or remove (e.g., package names, DLL names)
    /// CZ: Seznam hodnot atributu Include k nahrazení nebo odstranění (např. názvy balíčků, DLL)
    /// </param>
    /// <param name="csprojFilePath">
    /// EN: Full path to the .csproj file to modify
    /// CZ: Úplná cesta k .csproj souboru k úpravě
    /// </param>
    /// <param name="replacementReference">
    /// EN: New reference value to replace with (null = remove, non-null = replace). Must be the Include value only.
    /// CZ: Nová hodnota reference pro nahrazení (null = odstranit, nenulová = nahradit). Musí být pouze hodnota Include.
    /// </param>
    /// <example>
    /// EN: Example - Replace old package with new one:
    ///     ReplaceOrRemoveFile(null, "PackageReference", new List&lt;string&gt; { "OldPackage" }, @"C:\Project.csproj", "NewPackage")
    /// CZ: Příklad - Nahradit starý balíček novým:
    ///     ReplaceOrRemoveFile(null, "PackageReference", new List&lt;string&gt; { "OldPackage" }, @"C:\Project.csproj", "NewPackage")
    /// </example>
    private
#if ASYNC
    async Task
#else
    void
#endif
    ReplaceOrRemoveFile(Func<string, string>? additionalReplacements, string xmlElementName, List<string> referencesToModify, string csprojFilePath, string? replacementReference = null)
    {
        bool isReplacing = replacementReference != null;
        referenceClosingTag = "</" + xmlElementName + ">";
#if DEBUG
        if (csprojFilePath.EndsWith(@"sunamo.web.csproj"))
        {
            ThisApp.Check = true;
        }
#endif
        var xmlDocumentResult =
#if ASYNC
    await
#endif
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
#if DEBUG
            var originalFileLines =
#if ASYNC
    await
#endif
 TF.ReadAllLines(csprojFilePath);
            var backupFilePath = FS.InsertBetweenFileNameAndExtension(csprojFilePath, "_b");

#if ASYNC
            await
#endif
            TF.WriteAllLines(backupFilePath, originalFileLines!);
#endif
            await XmlDocumentsCache.Set(csprojFilePath, modifiedXmlContent);
#if ASYNC
            await
#endif
            TF.WriteAllText(csprojFilePath, modifiedXmlContent);
        }

        ThisApp.Check = false;
    }

    private static string ReferenceLongest(string xmlElementName, string reference)
    {
        return "<" + xmlElementName + " Include=\"" + reference + "\">";
    }

    private static string GetReferenceShortest(string xmlElementName, string reference)
    {
        return "<" + xmlElementName + " Include=\"" + reference + "\"/>";
    }

    /// <summary>
    /// Don't run, wait whether will be really needed to run
    /// it could do more harm than good
    /// </summary>
    private 
#if ASYNC
    async Task
#else
    void 
#endif
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
#if ASYNC
    await
#endif
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