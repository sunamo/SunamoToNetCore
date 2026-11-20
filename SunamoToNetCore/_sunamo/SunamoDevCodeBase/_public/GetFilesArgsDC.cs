namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._public;

// TODO: Should this inherit from GetFoldersEveryFolderArgs? In vs2 it does
internal class GetFilesArgsDC : GetFilesBaseArgsDC
{
    // TODO: This class has issues - clean up what should and shouldn't be here
    internal bool TrimExt { get; set; } = false;
    internal new bool TrimA1AndLeadingBs { get; set; } = false;
    internal List<string> ExcludeFromLocationsContains { get; set; } = new List<string>();
    internal bool DontIncludeNewest { get; set; } = false;
    // Method to exclude files, e.g. SunamoDevCodeHelper.RemoveTemporaryFilesVS
    internal Action<List<string>>? ExcludeWithMethod { get; set; } = null;
    internal bool ByDateOfLastModifiedAsc { get; set; } = false;
    internal Func<string, DateTime?>? LastModifiedFromFunc { get; set; }
    // Changed to false on 1-7-2020, still forgot to mention and method is problematic
    internal bool UseMascFromExtension { get; set; } = false;
    internal bool Wildcard { get; set; } = false;
}