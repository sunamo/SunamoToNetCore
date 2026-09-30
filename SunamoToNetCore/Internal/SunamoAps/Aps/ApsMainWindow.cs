namespace SunamoAps.Aps;

internal class ApsMainWindow : IMainWindowCsFileFilter, IAbstractCatalog<string, string>
{
    #region IAbstractCatalog
    internal static AbstractCatalog<string, string>? AbstractCatalogInstance = null;

    internal AbstractCatalog<string, string>? ac => AbstractCatalogInstance;
    #endregion

    #region IFoldersWithSolutions
    public static IFoldersWithSolutions? FoldersWithSolutionsInstance = null;

    public FoldersWithSolutionsList Fwss => FoldersWithSolutions.Fwss;
    #endregion

    public static ApsMainWindow Instance = new ApsMainWindow();

    public ApsMainWindow()
    {
    }

    public static IMainWindowAps? MainWindowAps = null;

    public bool cmd { get { return MainWindowAps == null ? true : MainWindowAps.Cmd; } set { if (MainWindowAps != null) MainWindowAps.Cmd = value; } }

    #region ICsFileFilter
    public static CsFileFilter? CsFileFilterInstance = null;

    public CsFileFilter CsFileFilter { get => CsFileFilterInstance!; set => CsFileFilterInstance = value; }
    #endregion
}