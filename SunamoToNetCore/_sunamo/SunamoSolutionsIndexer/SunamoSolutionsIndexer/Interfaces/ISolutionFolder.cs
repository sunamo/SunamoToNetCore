namespace SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer.Interfaces;

internal interface ISolutionFolder
{
    List<string> ProjectsGetCsprojs { get; set; }
    List<string> ProjectsInSolution { get; set; }
    ProjectsTypes TypeProjectFolder { get; set; }

    /// <summary>
    /// Exe to release.
    /// </summary>
    string? ExeToRelease(SolutionFolder sln, string projectDistinction, bool isStandaloneSlnForProject, bool isAddingProtectedWhenSelling = false, bool isPublishing = false);
    /// <summary>
    /// Have git folder.
    /// </summary>
    bool HaveGitFolder();
    /// <summary>
    /// To string.
    /// </summary>
    string ToString();
    /// <summary>
    /// Update modules.
    /// </summary>
    void UpdateModules(ILogger logger, PpkOnDriveDC toSelling);
}