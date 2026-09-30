namespace SunamoToNetCore._sunamo.SunamoSolutionsIndexer;

internal interface ISolutionFolderSerialize
{
    string DisplayedText { get; set; }
    string FullPathFolder { get; set; }
    string LongName { get; }
    string NameSolution { get; }
    string RunOne { get; }
    string ShortName { get; }

    /// <summary>
    /// To string.
    /// </summary>
    string ToString();
}