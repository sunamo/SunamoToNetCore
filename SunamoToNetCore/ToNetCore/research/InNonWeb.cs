namespace SunamoToNetCore;

public partial class MoveToNet5
{
    public static MoveToNet5 Instance { get; } = new MoveToNet5();
    private MoveToNet5()
    {
    }

    // Pravděpodobně to kurví csproj
    // A1 can be
    //
    // Tohle asi nemělo příliš smysl. Když jsem to změnil celé v jakémkoliv řešení z AnyCPU na x86 tak jsem měl u plno projektů modrou ikonku.
    // Navíc s tímto nastavením když jsem spustil EveryLine a ač sln bylo nastavené AnyCPu, hned po spuštení že nemůže načíst EveryLine. Když jsem jeho csproj změnil na AnyCPU, chybu začalo hlásit zase u desktop.
    //
    // A1 can be x86,x64.AnyCPU
    public
    async Task
    ChangeConvertNonWebPlatformTargetTo(ILogger logger, string replacementPlatformTarget)
    {
        var projectsData = WebAndNonWebProjects(logger);
        var nonWebProjects = projectsData.Item2;
        replacementPlatformTarget =
    await
        Shared.PlatformTargetTo(replacementPlatformTarget, nonWebProjects);
    }

    public static Type TypeInfo { get; } = typeof(MoveToNet5);

    // Vyčistí od dočasných souborů z NonWeb
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

    // Zakomentuje importy se kterými nemůže být převedeno na .net 5
    private
    async Task
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
    await
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

    // odstraní reference z c# které
    public async Task ReplaceUnneedReferencesInCsprojs(string csprojPath)
    {
        var referencesToRemove = SHGetLines.GetLines(refToRemove);
        await ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, referencesToRemove, csprojPath);
    }

    private
    async Task
    ReplaceUnneedReferencesInCsprojs(string dontReplaceReferencesInPath, string vsProjectsFolder)
    {
        var referencesToRemove = SHGetLines.GetLines(refToRemove);
        var csprojFiles = Directory.GetFiles(vsProjectsFolder, "*.csproj", SearchOption.AllDirectories);
        List<string> dontReplaceReferencesIn = (
    await
        TF.ReadAllLines(dontReplaceReferencesInPath))!.ToList();
        foreach (var csprojFilePath in csprojFiles)
        {
            if (!CA.ContainsAnyFromElementBool(csprojFilePath, dontReplaceReferencesIn))
            {
                await
                ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, referencesToRemove, csprojFilePath);
            }
        }
    }

    private string referenceClosingTag = "</Reference>";
}
