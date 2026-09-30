namespace SunamoAps.Aps.Data;

internal class AsyncPushSolutions : AsyncLoadingBaseDC<AsyncPushSolutions, IProgressBarDC>
{
    public List<SolutionFolder> FoldersWithSolutions { get; set; } = null!;
    public bool Release { get; set; }
    public GitBashBuilder GitBashBuilder { get; set; } = null!;
    public string PushArgs { get; set; } = null!;
    public string? CommitMessage { get; set; }
    public PushSolutionsData PushSolutionsData { get; set; } = null!;
    public GitBashBuilder GitStatus { get; set; } = null!;
}