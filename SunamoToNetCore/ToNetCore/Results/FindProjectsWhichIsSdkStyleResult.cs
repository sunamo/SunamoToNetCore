namespace SunamoToNetCore.ToNetCore.Results;

/// <summary>
/// Represents the find projects which is sdk style result type.
/// </summary>
public class FindProjectsWhichIsSdkStyleResult
{
    /// <summary>
    /// Gets or sets the csproj sdk style list.
    /// </summary>
    public List<string> CsprojSdkStyleList { get; set; } = null!;

    /// <summary>
    /// Gets or sets the netstandard list.
    /// </summary>
    public List<string> NetstandardList { get; set; } = null!;

    /// <summary>
    /// Gets or sets the non csproj sdk style list.
    /// </summary>
    public List<string> NonCsprojSdkStyleList { get; set; } = null!;
}