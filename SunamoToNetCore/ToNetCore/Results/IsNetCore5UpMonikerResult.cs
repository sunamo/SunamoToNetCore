namespace SunamoToNetCore;

// Reference: https://learn.microsoft.com/en-us/dotnet/standard/frameworks
public class IsNetCore5UpMonikerResult
{
    public string TargetFramework { get; set; } = null!;

    public string PlatformTfm { get; set; } = null!;

    public override string ToString() => TargetFramework + PlatformTfm;
}