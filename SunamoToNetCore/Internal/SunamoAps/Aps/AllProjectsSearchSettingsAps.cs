namespace SunamoAps.Aps;

internal static partial class AllProjectsSearchSettings
{
    public static readonly string PathFileSettings = AppPaths.GetFileInStartupPath("settings.ini");

    #region sectionSearchFoldersChecked

    public static bool IsFolderSearchChecked()
    {
        return true;
    }

    public static void SetSearchFolderChecked()
    {
    }

    public static bool ExistsFolderSearchBySerie()
    {
        return false;
    }

    public static string GetSearchFolderNormalized()
    {
        return string.Empty;
    }

    public static int AddFolderSearch()
    {
        return 1;
    }

    #endregion
}