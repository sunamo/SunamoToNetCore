namespace SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer;

public partial class FoldersWithSolutions
{
    /// <summary>
    /// Add projects folder.
    /// </summary>
    void AddProjectsFolder(List<string> projects, string folder)
    {
        List<string> specialFolders, normalFolders;
        ReturnNormalAndSpecialFolders(folder, out specialFolders, out normalFolders);
        normalFolders = CA.EnsureBackslash(normalFolders);
        projects.AddRange(normalFolders);
        foreach (string specialFolder in specialFolders)
        {
            AddProjectsFolder(projects, specialFolder);
        }
    }
}