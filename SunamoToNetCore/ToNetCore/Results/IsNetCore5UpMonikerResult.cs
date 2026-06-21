namespace SunamoDevCode.ToNetCore.Results;

/// <summary>
/// Result class for .NET Core 5+ target framework moniker parsing.
/// Reference: https://learn.microsoft.com/en-us/dotnet/standard/frameworks
/// </summary>
public class IsNetCore5UpMonikerResult
{
    /// <summary>
    /// Gets or sets the target framework (e.g., "net5.0", "net6.0", "net7.0").
    /// </summary>
    public string TargetFramework { get; set; } = null!;

    /// <summary>
    /// Gets or sets the platform-specific target framework moniker (e.g., "-windows", "-android").
    /// </summary>
    public string PlatformTfm { get; set; } = null!;

    /// <summary>
    /// Returns the combined target framework and platform TFM as a string.
    /// </summary>
    /// <returns>Combined target framework moniker string.</returns>
    public override string ToString()
    {
        return TargetFramework + PlatformTfm;
    }
}