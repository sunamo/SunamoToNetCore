namespace SunamoAps.Aps.Data;

internal class SolutionFolderDetailedInfo : SolutionFolder
{
    // TODO: whole was moved to Aps.Statistic. use interface and to initialize delegate

    public int OverallFiles = 0;

    public SolutionFolderDetailedInfo(SolutionFolder solutionFolder)
    {
        CountOfImages = solutionFolder.CountOfImages;
        DisplayedText = solutionFolder.DisplayedText;
        FullPathFolder = solutionFolder.FullPathFolder;
        NameSolutionWithoutDiacritic = solutionFolder.NameSolutionWithoutDiacritic;

        DisplayedText += " (" + OverallFiles.ToString() + ")";
    }
}

internal class SolutionFolderDetailedInfoComparerBySourceFiles : IComparer<SolutionFolderDetailedInfo>
{
    public int Compare(SolutionFolderDetailedInfo? first, SolutionFolderDetailedInfo? second)
    {
        if (first == null || second == null) return 0;
        return first.OverallFiles.CompareTo(second.OverallFiles) * -1;
    }
}