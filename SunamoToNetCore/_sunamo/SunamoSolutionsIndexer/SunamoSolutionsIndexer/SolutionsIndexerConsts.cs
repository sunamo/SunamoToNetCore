namespace SunamoToNetCore._sunamo.SunamoSolutionsIndexer;

internal class SolutionsIndexerConsts
{
    public const string BitBucket = "BitBucket";
    public const string ProjectsFolderName = "Projects";
    public const string VisualStudio = "Visual Studio";

    public static List<string> SolutionsExcludeWhileWorkingOnSourceCode = new List<string>(["AllProjectsSearch", "sunamo", "CodeBoxControl"]);
}
