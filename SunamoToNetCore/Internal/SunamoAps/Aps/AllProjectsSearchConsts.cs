namespace SunamoAps.Aps;

internal partial class AllProjectsSearchConsts
{
    public const string FnLatestClearTempFiles = "LatestClearTempFiles";

    private List<string> foldersIgnore = new List<string> { "Addins", "Settings", "Templates", "Visualizers",
    "ArchitectureExplorer", "Backup Files",  "Code Snippets", "StartPages", "Assemblies", "Blend" };

    public static List<string> ExtCodeElements { get; set; } = new List<string> { ".cs", ".ts", ".dart" };

    public static List<string> ExtFiles2 { get; set; } = new List<string> { ".ashx", ".aspx", ".xaml", ".cshtml" };

    public const byte ForApsPlugins = 9;
}