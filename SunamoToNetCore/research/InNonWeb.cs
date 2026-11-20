namespace SunamoToNetCore.research;

public partial class MoveToNet5
{
    public static MoveToNet5 Instance { get; } = new MoveToNet5();
    private MoveToNet5()
    {
    }

    // It probably breaks the csproj
    // A1 can be
    //
    // This probably did not make much sense. When I changed it everywhere in any solution from AnyCPU to x86, I got a blue icon on many projects.
    // Moreover, with this setting, when I ran EveryLine and although the sln was set to AnyCPU, right at startup it said it could not load EveryLine. When I changed its csproj to AnyCPU, the error started to be reported again for desktop.
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

    // Cleans up temporary files from NonWeb
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

    // Comments out imports that cannot be converted to .net 5
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
                for (int index = 0; index < linesToComment.Count; index++)
                {
                    modifiedFileContent = modifiedFileContent!.Replace(linesToComment[index], singleCommentedLines[index]);
                }

                for (int commentIndex = 0; commentIndex < linesToComment.Count; commentIndex++)
                {
                    modifiedFileContent = modifiedFileContent!.Replace(doubleCommentedLines[commentIndex], singleCommentedLines[commentIndex]);
                }

                if (modifiedFileContent != originalFileContent)
                {
                    await TF.WriteAllText(csFilePath, modifiedFileContent!);
                }
            }
        }
    }

    // 25-9-2022 Because something deleted the assembly with *.sunamo.cz csproj again - .Web.Services, Data, .Web => commented, replacement below.
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
    const string refToRemove = @"sunamoPortable
swf";

    // removes references from c# which
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