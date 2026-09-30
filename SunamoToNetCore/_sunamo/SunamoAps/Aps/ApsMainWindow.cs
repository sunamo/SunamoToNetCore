namespace SunamoToNetCore._sunamo.SunamoAps;

internal class ApsMainWindow : IMainWindowCsFileFilter, IAbstractCatalog<string, string>
{
    #region IAbstractCatalog
    internal static AbstractCatalog<string, string>? AbstractCatalogInstance = null;

    internal AbstractCatalog<string, string>? ac => AbstractCatalogInstance;

    public FoldersWithSolutionsList Fwss => FoldersWithSolutions.Fwss;
    #endregion

    public static ApsMainWindow Instance = new ApsMainWindow();

    /// <summary>
    /// Initializes a new instance of ApsMainWindow.
    /// </summary>
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