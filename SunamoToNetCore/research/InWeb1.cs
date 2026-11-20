namespace SunamoToNetCore.research;

public partial class MoveToNet5
{
    /// <summary>
    /// Find projects which is sdk style.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="appendHeaderForWeb">The append header for web.</param>
    /// <param name="isWeb">The is web.</param>
    /// <returns>The find projects which is sdk style result.</returns>
    public
    async Task<FindProjectsWhichIsSdkStyleResult>
    FindProjectsWhichIsSdkStyle(ILogger logger, bool appendHeaderForWeb, bool isWeb = true)
    {
        List<string> csprojSdkStyleList = new List<string>();
        List<string> netstandardList = new List<string>();
        List<string> nonCsprojSdkStyleList = new List<string>();
        if (appendHeaderForWeb)
        {
            csprojSdkStyleList.Add("Web but in SDK style:");
        }

        var projectsData = WebAndNonWebProjects(logger);
        List<string>? projectPaths = null;
        if (isWeb)
        {
            projectPaths = projectsData.Item1;
        }
        else
        {
            projectPaths = projectsData.Item2;
        }

        foreach (var projectPath in projectPaths)
        {
            if (Ignored.IsIgnored(projectPath))
            {
                continue;
            }

            var projectStyleResult =
    await
            SunamoCsprojHelper.IsProjectCsprojSdkStyleIsCore( /*ref*/projectPath);
            if (projectStyleResult!.IsProjectCsprojSdkStyleIsCore)
            {
                if (projectStyleResult.IsNetstandard)
                {
                    netstandardList.Add(projectPath);
                }
                else
                {
                    csprojSdkStyleList.Add(projectPath);
                }
            }
            else
            {
                nonCsprojSdkStyleList.Add(projectPath);
            }
        }

        return new FindProjectsWhichIsSdkStyleResult
        {
            CsprojSdkStyleList = csprojSdkStyleList,
            NetstandardList = netstandardList,
            NonCsprojSdkStyleList = nonCsprojSdkStyleList
        };
    }

    /// <summary>
    /// Web projects which is not sdk style.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The web projects which is not sdk style result.</returns>
    public
    async Task<string>
    WebProjectsWhichIsNotSdkStyle(ILogger logger)
    {
        var sdkStyleResult =
    await
        FindProjectsWhichIsSdkStyle(logger, true);
        return SHJoin.JoinNL(sdkStyleResult.NonCsprojSdkStyleList);
    }

    string? nameProject = null;

    /// <summary>
    /// Replace project reference for web.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="projectName">The project name.</param>
    /// <param name="projectNamespace">The project namespace.</param>
    public async void ReplaceProjectReferenceForWeb(ILogger logger, string projectName, string projectNamespace)
    {
        Console.WriteLine("Solution old & new must be in same root folder");
        projectName = SHTrim.TrimEnd(projectName, ".web");
        projectNamespace = SHTrim.TrimEnd(projectNamespace, ".web");
        nameProject = projectName;
        string oldProjectReference = @"..\..\" + projectNamespace + @"\" + projectName + @"\" + projectName + ".csproj";
        string newProjectReference = @"..\..\" + SolutionNameFor(projectNamespace) + @"\" + projectName + @".web\" + projectName + ".web.csproj";
        var projectsData = WebAndNonWebProjects(logger);
        foreach (var webProjectPath in projectsData.Item1)
        {
            Console.WriteLine(webProjectPath);
            await ReplaceOrRemoveFile(WithWebEnd, ElementsItemGroup.ProjectReference, [oldProjectReference], webProjectPath, newProjectReference);
        }
    }

    string WithWebEnd(string text)
    {
        return text.Replace("<Name>" + nameProject + "</Name>", "<Name>" + nameProject + ".web</Name>");
    }

    private string SolutionNameFor(string namespaceName)
    {
        if (namespaceName == "PlatformIndependentNuGetPackages")
        {
            return "sunamo.webWithoutDep";
        }

        return namespaceName + ".web";
    }
}