namespace SunamoDevCode.ToNetCore.research;

public partial class MoveToNet5
{
    /// <summary>
    /// Categorizes projects into SDK style, netstandard, and non-SDK style groups.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appendHeaderForWeb">If true, adds a header label for web SDK style projects.</param>
    /// <param name="isWeb">If true, processes web projects; otherwise processes non-web projects.</param>
    /// <returns>Result containing three categorized lists of project paths.</returns>
    public
#if ASYNC
    async Task<FindProjectsWhichIsSdkStyleResult>
#else
    FindProjectsWhichIsSdkStyleResult 
#endif
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
#if ASYNC
    await
#endif
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
    /// Gets a newline-separated list of web projects that are not SDK style.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Newline-separated list of non-SDK style web project paths.</returns>
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
    WebProjectsWhichIsNotSdkStyle(ILogger logger)
    {
        var sdkStyleResult =
#if ASYNC
    await
#endif
        FindProjectsWhichIsSdkStyle(logger, true);
        return SHJoin.JoinNL(sdkStyleResult.NonCsprojSdkStyleList);
    }

    string? nameProject = null;
    /// <summary>
    /// Replaces project references in web projects to point to the web-specific version.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="projectName">Name of the project to replace references for.</param>
    /// <param name="projectNamespace">Namespace of the project.</param>
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
            //DebugLogger.Instance.WriteLine(webProjectPath);
            //var dx =
            await ReplaceOrRemoveFile(WithWebEnd, ElementsItemGroup.ProjectReference, [oldProjectReference], webProjectPath, newProjectReference);
        //if (dx != -1)
        //{
        //    //break;
        //}
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