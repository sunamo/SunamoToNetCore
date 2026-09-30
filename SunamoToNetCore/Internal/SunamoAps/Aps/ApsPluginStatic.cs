namespace SunamoAps.Aps;

internal class ApsPluginStatic
{
    public bool Cmd => ApsMainWindow.Instance.cmd;

    public static FoldersWithSolutionsList Fwss => FoldersWithSolutions.Fwss;

    public static RepositoryLocal UsedRepository
    {
        set => FoldersWithSolutions.UsedRepository = value;
        get => FoldersWithSolutions.UsedRepository;
    }
}