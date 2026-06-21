namespace SunamoDevCode.ToNetCore.research;

public partial class MoveToNet5
{
    /// <summary>
    /// Compares web and non-web projects to find which ones exist in both categories.
    /// Projects used in both need to stay on netstandard.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Always returns false (incomplete implementation).</returns>
    public bool TableWebAndNonWeb(ILogger logger)
    {
        var value = WebAndNonWebProjects(logger);
        var web = value.Item1;
        var nonWeb = value.Item2;

        var webFn = FS.OnlyNamesWithoutExtensionCopy(web);
        var nonWebFn = FS.OnlyNamesWithoutExtensionCopy(nonWeb);

        CA.Replace(webFn, ".web", string.Empty);

        var both = CAG.CompareList(webFn, nonWebFn);

        var desktopInWeb = webFn.Where(projectName => projectName.Contains("desktop"));
        var desktopInNonWeb = nonWebFn.Where(projectName => projectName.Contains("desktop"));

        int webIndex, nonWebIndex;
        //List<Tuple<ProjFw, ProjFw>> toTable = new List<Tuple<ProjFw, ProjFw>>();

        foreach (var projectName in both)
        {
            webIndex = webFn.IndexOf(projectName);
            nonWebIndex = nonWebFn.IndexOf(projectName);

            var webProjectPath = web[webIndex];
            var nonWebProjectPath = nonWeb[nonWebIndex];

            //ProjFw pfWeb = new ProjFw { path = f1, temp = FrameworkNameDetector.Detect(f1). };

        }

        return false;
    }

    /// <summary>
    /// Restores files from their .old backup copies.
    /// </summary>
    /// <param name="filePaths">List of file paths to restore from backup.</param>
    public
#if ASYNC
    async Task
#else
    void
#endif
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

    /// <summary>
    /// Gets a formatted list of all web and non-web projects.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Text output with web and non-web project lists.</returns>
    public string ListOfAllWebAndNonWeb(ILogger logger)
    {
        var projectsData = WebAndNonWebProjects(logger, false);
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(projectsData.Item1, "Web");
        tog.List(projectsData.Item2, "NonWeb");

        return tog.ToString();
    }

    /// <summary>
    /// Replaces unnecessary references in csproj files that are not SDK style and generates try-convert commands.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="isWeb">If true, processes web projects; otherwise non-web projects.</param>
    /// <returns>Generated try-convert commands for each processed project.</returns>
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
 ReplaceUnneedReferencesInCsprojsNotSdKStyle(ILogger logger, bool isWeb = true)
    {
        StringBuilder stringBuilder = new StringBuilder();
        var sdkStyleResult =
#if ASYNC
    await
#endif
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

    /// <summary>
    /// Generates a try-convert CLI command for converting a csproj to .NET 5.
    /// </summary>
    /// <param name="csprojPath">Path to the csproj file to convert.</param>
    /// <returns>try-convert command string.</returns>
    public string GenerateTryConvert(string csprojPath)
    {
        // poslední verze try-convert je 0.9.232202 a ta funguje na .NET 5. Proto musím přidávat ty 2 parametry
        return @"try-convert  --target-framework net5.0 -m 'C:\Program Files\dotnet\sdk\5.0.100\' -w '" + csprojPath + "'";
    }

    /// <summary>
    /// Gets all unique TargetFramework values across non-web projects and lists projects with multiple TargetFramework elements.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Text output with target frameworks grouped by value and list of multi-framework projects.</returns>
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
 GetAllTargetFrameworks(ILogger logger)
    {
        List<string> hasMoreTargetFrameworkElements = new List<string>();

        Dictionary<string, List<string>> ls = new Dictionary<string, List<string>>();

        var data = WebAndNonWebProjects(logger, true);

        foreach (var item in data.Item2)
        {
            var count =
#if ASYNC
    await
#endif
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

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.Dictionary(ls);
        tog.List(hasMoreTargetFrameworkElements, "hasMoreTargetFrameworkElements");

        return tog.ToString();
    }
}