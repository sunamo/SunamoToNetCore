namespace SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer;

/// <summary>
/// Represents the folders with solutions list type.
/// </summary>
public class FoldersWithSolutionsList : List<FoldersWithSolutions>
{
    /// <summary>
    /// Add.
    /// </summary>
    public new void Add(FoldersWithSolutions foldersWithSolutions)
    {
        base.Add(foldersWithSolutions);
    }
}