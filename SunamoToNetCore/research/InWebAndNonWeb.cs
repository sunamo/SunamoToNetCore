namespace SunamoToNetCore.research;

public partial class MoveToNet5
{
    // Compares web and non-web projects to find which ones exist in both categories.
    // Projects used in both need to stay on netstandard.
    /// <summary>
    /// Table web and non web.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The table web and non web result.</returns>
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

        foreach (var projectName in both)
        {
            webIndex = webFn.IndexOf(projectName);
            nonWebIndex = nonWebFn.IndexOf(projectName);

            var webProjectPath = web[webIndex];
            var nonWebProjectPath = nonWeb[nonWebIndex];


        }

        return false;
    }

    /// <summary>
    /// Restore from backup.
    /// </summary>
    /// <param name="filePaths">The file paths.</param>
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

    /// <summary>
    /// List of all web and non web.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The list of all web and non web result.</returns>
    public string ListOfAllWebAndNonWeb(ILogger logger)
    {
        var projectsData = WebAndNonWebProjects(logger, false);
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(projectsData.Item1, "Web");
        tog.List(projectsData.Item2, "NonWeb");

        return tog.ToString();
    }

    /// <summary>
    /// Replace unneed references in csprojs not sd k style.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="isWeb">The is web.</param>
    /// <returns>The replace unneed references in csprojs not sd k style result.</returns>
    public
    async Task<string>
 ReplaceUnneedReferencesInCsprojsNotSdKStyle(ILogger logger, bool isWeb = true)
    {
        StringBuilder stringBuilder = new StringBuilder();
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

    /// <summary>
    /// Generate try convert.
    /// </summary>
    /// <param name="csprojPath">The csproj path.</param>
    /// <returns>The generate try convert result.</returns>
    public string GenerateTryConvert(string csprojPath)
    {
        // the latest version of try-convert is 0.9.232202 and it works on .NET 5. That is why I have to add those 2 parameters
        return @"try-convert  --target-framework net5.0 -m 'C:\Program Files\dotnet\sdk\5.0.100\' -w '" + csprojPath + "'";
    }

    /// <summary>
    /// Get all target frameworks.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The get all target frameworks result.</returns>
    public
    async Task<string>
 GetAllTargetFrameworks(ILogger logger)
    {
        List<string> hasMoreTargetFrameworkElements = new List<string>();

        Dictionary<string, List<string>> targetFrameworksByProject = new Dictionary<string, List<string>>();

        var data = WebAndNonWebProjects(logger, true);

        foreach (var item in data.Item2)
        {
            var count =
    await
 TF.ReadAllText(item);

            var text = SH.GetTextBetweenSimple(count!, ChangeProjects.start, ChangeProjects.end, false);

            if (text != null)
            {
                DictionaryHelper.AddOrCreate(targetFrameworksByProject, text, item);
            }

            if (SH.OccurencesOfStringIn(count!, ChangeProjects.start) > 1)
            {
                hasMoreTargetFrameworkElements.Add(item);
            }
        }

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.Dictionary(targetFrameworksByProject);
        tog.List(hasMoreTargetFrameworkElements, "hasMoreTargetFrameworkElements");

        return tog.ToString();
    }
}