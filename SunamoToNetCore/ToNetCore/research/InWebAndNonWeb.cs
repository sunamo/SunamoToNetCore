namespace SunamoToNetCore;

public partial class MoveToNet5
{
    public bool TableWebAndNonWeb(ILogger logger)
    {
        var value = WebAndNonWebProjects(logger);
        var web = value.Item1;
        var nonWeb = value.Item2;

        var webFn = FS.OnlyNamesWithoutExtensionCopy(web);
        var nonWebFn = FS.OnlyNamesWithoutExtensionCopy(nonWeb);

        CA.Replace(webFn, ".web", string.Empty);

        var both = CAG.CompareList(webFn, nonWebFn);

        _ = webFn.Where(projectName => projectName.Contains("desktop"));
        _ = nonWebFn.Where(projectName => projectName.Contains("desktop"));

        int webIndex, nonWebIndex;
        //List<Tuple<ProjFw, ProjFw>> toTable = new List<Tuple<ProjFw, ProjFw>>();

        foreach (var projectName in both)
        {
            webIndex = webFn.IndexOf(projectName);
            nonWebIndex = nonWebFn.IndexOf(projectName);

            _ = web[webIndex];
            _ = nonWeb[nonWebIndex];

            //ProjFw pfWeb = new ProjFw { path = f1, temp = FrameworkNameDetector.Detect(f1). };

        }

        return false;
    }

    public
    async Task
 RestoreFromBackup(List<string> filePaths)
    {
        foreach (var filePath in filePaths)
        {
            var backupFilePath = filePath + AllExtensions.OldExtension;
            if (FS.ExistsFile(backupFilePath))
            {
                FS.MoveFile(backupFilePath, filePath, FileMoveCollisionOptionDC.Overwrite);
            }
            else
            {
                Console.WriteLine("Doesn't exists: " + backupFilePath);
            }
        }
    }

    public string ListOfAllWebAndNonWeb(ILogger logger)
    {
        var projectsData = WebAndNonWebProjects(logger, false);
        TextOutputGenerator tog = new();
        tog.List(projectsData.Item1, "Web");
        tog.List(projectsData.Item2, "NonWeb");

        return tog.ToString();
    }

    public
    async Task<string>
 ReplaceUnneedReferencesInCsprojsNotSdKStyle(ILogger logger, bool isWeb = true)
    {
        StringBuilder stringBuilder = new();
        var sdkStyleResult =
    await
 FindProjectsWhichIsSdkStyle(logger, false, isWeb);
        foreach (var projectPath in sdkStyleResult.NonCsprojSdkStyleList)
        {
            if (projectPath.EndsWith("_b.csproj"))
            {
                continue;
            }
            await ReplaceUnneedReferencesInCsprojs(projectPath);
            stringBuilder.AppendLine(GenerateTryConvert(projectPath));
        }
        var result = stringBuilder.ToString();
        return result;
    }

    public string GenerateTryConvert(string csprojPath)
    {
        // poslední verze try-convert je 0.9.232202 a ta funguje na .NET 5. Proto musím přidávat ty 2 parametry
        return @"try-convert  --target-framework net5.0 -m 'C:\Program Files\dotnet\sdk\5.0.100\' -w '" + csprojPath + "'";
    }

    public
    async Task<string>
 GetAllTargetFrameworks(ILogger logger)
    {
        List<string> hasMoreTargetFrameworkElements = new();

        Dictionary<string, List<string>> ls = new();

        var data = WebAndNonWebProjects(logger, true);

        foreach (var item in data.Item2)
        {
            var count =
    await
 TF.ReadAllText(item);

            var text = SH.GetTextBetweenSimple(count!, ChangeProjects.start, ChangeProjects.end, false);

            if (text != null)
            {
                DictionaryHelper.AddOrCreate(ls, text, item);
            }

            if (SH.OccurencesOfStringIn(count!, ChangeProjects.start) > 1)
            {
                hasMoreTargetFrameworkElements.Add(item);
            }
        }

        TextOutputGenerator tog = new();
        tog.Dictionary(ls);
        tog.List(hasMoreTargetFrameworkElements, "hasMoreTargetFrameworkElements");

        return tog.ToString();
    }
}
