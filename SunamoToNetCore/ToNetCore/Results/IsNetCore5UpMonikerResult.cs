namespace SunamoToNetCore.ToNetCore.Results;

// Reference: https://learn.microsoft.com/en-us/dotnet/standard/frameworks
/// <summary>
/// Represents the is net core 5 up moniker result type.
/// </summary>
public class IsNetCore5UpMonikerResult
{
    /// <summary>
    /// Gets or sets the target framework.
    /// </summary>
    public string TargetFramework { get; set; } = null!;

    /// <summary>
    /// Gets or sets the platform tfm.
    /// </summary>
    public string PlatformTfm { get; set; } = null!;

    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString() => TargetFramework + PlatformTfm;
}