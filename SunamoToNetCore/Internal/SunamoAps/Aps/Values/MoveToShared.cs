namespace SunamoAps.Aps.Values;

internal class MoveToShared : IMoveToShared
{
    public string Folder { get; set; } = null!;

    public string FolderSunamo { get; set; } = null!;

    public string FolderSunamoWeb { get; set; } = null!;

    public string Postfix { get; set; } = "web";

    public string SlnFrom { get; set; } = "sunamo";

    public MoveToShared(string baseFolder)
    {
        this.Folder = baseFolder;

        // Projects\\ must be here for sunamo.cz
        FolderSunamo = baseFolder + "Projects\\" + SlnFrom + "\\";
        FolderSunamoWeb = baseFolder + "Projects\\" + "sunamo." + Postfix + "\\";
    }
}