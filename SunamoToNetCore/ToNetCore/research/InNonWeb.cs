namespace SunamoDevCode.ToNetCore.research;

/// <summary>
/// Provides methods for migrating .NET Framework projects to .NET 5+ including reference replacement, platform target changes, and cleanup operations.
/// </summary>
public partial class MoveToNet5
{
    /// <summary>
    /// Singleton instance of MoveToNet5.
    /// </summary>
    public static MoveToNet5 Instance { get; } = new MoveToNet5();
    private MoveToNet5()
    {
    }

    /// <summary>
    /// Pravděpodobně to kurví csproj
    /// A1 can be
    ///
    /// Tohle asi nemělo příliš smysl. Když jsem to změnil celé v jakémkoliv řešení z AnyCPU na x86 tak jsem měl u plno projektů modrou ikonku.
    /// Navíc s tímto nastavením když jsem spustil EveryLine a ač sln bylo nastavené AnyCPu, hned po spuštení že nemůže načíst EveryLine. Když jsem jeho csproj změnil na AnyCPU, chybu začalo hlásit zase u desktop.
    ///
    /// A1 can be x86,x64.AnyCPU
    /// </summary>
    public 
#if ASYNC
    async Task
#else
    void 
#endif
    ChangeConvertNonWebPlatformTargetTo(ILogger logger, string replacementPlatformTarget)
    {
        var projectsData = WebAndNonWebProjects(logger);
        var nonWebProjects = projectsData.Item2;
        replacementPlatformTarget =
#if ASYNC
    await
#endif
        Shared.PlatformTargetTo(replacementPlatformTarget, nonWebProjects);
    }

    /// <summary>
    /// Type information for the MoveToNet5 class.
    /// </summary>
    public static Type TypeInfo { get; } = typeof(MoveToNet5);
    /// <summary>
    /// Vyčistí od dočasných souborů z NonWeb
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Folder path for temporary content (without trailing backslash).</param>
    public void ClearUnnecessaryFromNonWeb(ILogger logger, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        Console.WriteLine("ClearUnnecessaryFromNonWeb");
        var solutionsData = WebAndNonWebSlns();
        Console.WriteLine("solutionsData.Item2.Count: " + solutionsData.Item2.Count);
        foreach (var solutionPath in solutionsData.Item2)
        {
            DeleteTemporaryFilesFromSolution.ClearSolution(logger, solutionPath, true, folderWithTemporaryMovedContentWithoutBackslash);
        }
    }

    /// <summary>
    /// Zakomentuje importy se kterými nemůže být převedeno na .net 5
    /// </summary>
    private 
#if ASYNC
    async Task
#else
    void 
#endif
    ReplaceUnneedUsings(string vsProjectsFolder)
    {
        var linesToCommented = @"//using System.Data;";
        var linesToComment = SHGetLines.GetLines(linesToCommented);
        List<string> singleCommentedLines = new List<string>(linesToComment.Count);
        List<string> doubleCommentedLines = new List<string>(linesToComment.Count);
        const string commentPrefix = "//";
        foreach (var line in linesToComment)
        {
            singleCommentedLines.Add(commentPrefix + line);
            doubleCommentedLines.Add(commentPrefix + commentPrefix + line);
        }

        var csFiles = Directory.GetFiles(vsProjectsFolder, "*.cs", SearchOption.AllDirectories);
        List<string> dontReplaceUsingSystemDataIn = new List<string>()
        {
            ".web",
            "SunamoSqlServer",
            "SunamoSqlite",
            "SunamoCsv"
        };
        foreach (var csFilePath in csFiles)
        {
            if (!CA.ContainsAnyFromElementBool(csFilePath, dontReplaceUsingSystemDataIn))
            {
                var originalFileContent =
#if ASYNC
    await
#endif
                TF.ReadAllText(csFilePath);
                string modifiedFileContent = originalFileContent!;
                for (int i = 0; i < linesToComment.Count; i++)
                {
                    modifiedFileContent = modifiedFileContent!.Replace(linesToComment[i], singleCommentedLines[i]);
                }

                for (int i = 0; i < linesToComment.Count; i++)
                {
                    modifiedFileContent = modifiedFileContent!.Replace(doubleCommentedLines[i], singleCommentedLines[i]);
                }

                if (modifiedFileContent != originalFileContent)
                {
                    await TF.WriteAllText(csFilePath, modifiedFileContent!);
                }
            }
        }
    }

    // 25-9-2022 Protože mi opět něco smazalo assembly se *.sunamo.cz csproj - .Web.Services, Data, .Web => commented, dole náhrada.
    //     const string refToRemove = @"Microsoft.CSharp
    // System.ComponentModel.Composition
    // System.Core
    // System.Data
    // System.Data.DataSetExtensions
    // System.Deployment
    // System.Design
    // System.Net.Http
    // System.Net.Http.WebRequest
    // System.Web
    // System.Web.Extensions
    // System.Web.ApplicationServices
    // System.Web.DynamicData
    // System.Web.Entity
    // System.Web.Services
    // System.Windows
    // System.Windows.Presentation
    // System.Xml
    // System.Xml.Ling
    // sunamoPortable
    // swf
    // System.Device";
    const string refToRemove = @"sunamoPortable
swf";
    /// <summary>
    /// odstraní reference z c# které
    /// </summary>
    /// <param name="csprojPath">Path to the csproj file to process.</param>
    public async Task ReplaceUnneedReferencesInCsprojs(string csprojPath)
    {
        var referencesToRemove = SHGetLines.GetLines(refToRemove);
        await ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, referencesToRemove, csprojPath);
    }

    private
#if ASYNC
    async Task
#else
    void
#endif
    ReplaceUnneedReferencesInCsprojs(string dontReplaceReferencesInPath, string vsProjectsFolder)
    {
        var referencesToRemove = SHGetLines.GetLines(refToRemove);
        var csprojFiles = Directory.GetFiles(vsProjectsFolder, "*.csproj", SearchOption.AllDirectories);
        List<string> dontReplaceReferencesIn = (
#if ASYNC
    await
#endif
        TF.ReadAllLines(dontReplaceReferencesInPath))!.ToList();
        foreach (var csprojFilePath in csprojFiles)
        {
            if (!CA.ContainsAnyFromElementBool(csprojFilePath, dontReplaceReferencesIn))
            {
#if ASYNC
                await
#endif
                ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, referencesToRemove, csprojFilePath);
            }
        }
    }

    private string referenceClosingTag = "</Reference>";
}