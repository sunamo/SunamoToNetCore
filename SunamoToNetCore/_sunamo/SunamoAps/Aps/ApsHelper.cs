namespace SunamoToNetCore._sunamo.SunamoAps;

internal partial class ApsHelper : ApsPluginStatic
{
    // Never create new instance, just call method
    public PushSolutionsData pushSolutionsData = new PushSolutionsData();
    string? typed = null;
    bool cmd = false;
    GitBashBuilder gitPullVps = new GitBashBuilder(new TextBuilderDC());
    GitBashBuilder gitPushVps = new GitBashBuilder(new TextBuilderDC());
    // Separates all projects into web and non-web categories. Do not use XmlDocumentsCache.
    /// <summary>
    /// Web and non web projects.
    /// </summary>
    public static Tuple<List<string>, List<string>> WebAndNonWebProjects(ILogger logger, bool withCsprojs = true)
    {
        List<string> webProjects = new List<string>();
        List<string> notWebProjects = new List<string>();
        foreach (var item in FoldersWithSolutions.Fwss)
        {
            var solutions = item.GetSolutions(RepositoryLocal.Vs17);
            foreach (var sln in solutions)
            {
                SolutionFolder.GetCsprojs(logger, sln);
                foreach (var projectPath in sln.ProjectsGetCsprojs)
                {
                    var finalProjectPath = withCsprojs ? projectPath : FS.GetDirectoryName(projectPath);
                    if (IsWeb(projectPath))
                    {
                        webProjects.Add(finalProjectPath);
                    }
                    else
                    {
                        notWebProjects.Add(finalProjectPath);
                    }
                }
            }
        }
        return new Tuple<List<string>, List<string>>(webProjects, notWebProjects);
    }

    /// <summary>
    /// Is web.
    /// </summary>
    public static bool IsWeb(string projectPath)
    {
        return CA.ContainsAnyFromElementBool(projectPath, AllProjectsSearchSettings.DontReplaceReferencesIn!);
    }
}