namespace SunamoDevCodeCore;

internal class FrameworkNameDetector
{
    public const string DefaultIdentifier = ".NETFramework";
    public const string DefaultFrameworkVersion = "v4.0";

    // Working only for .net fw, for sdk style return .net 4.0
    // Must return FrameworkName due to FubuCsProjFile
#pragma warning disable
    public static FrameworkName Detect(/*MSBuildProject*/ object project)
#pragma warning restore
    {
        throw new Exception(@"I don't even know where MSBuildProject comes from anymore.
I installed these packages:
Microsoft.Build
Microsoft.CodeAnalysis
Microsoft.CodeAnalysis.Common
Microsoft.CodeAnalysis.CSharp
Microsoft.CodeAnalysis.Workspaces.Common
and it's not in any of them. Moreover, there's also no mention from MS on the web. It was probably already obsolete when it was being used. Should do this from XML here.
Below is a piece of code:
var msb = new MSBuildProject();
        await msb.LoadAsync(path);
but still no mention on the web.
This just means it was garbage and nobody used it, since it wasn't used anywhere.");
        //var group = project.PropertyGroups.FirstOrDefault(x =>
        //    x.Properties.Any(p => p.Name.Contains("TargetFramework")));
        //var identifier = DefaultIdentifier;
        //var versionString = DefaultFrameworkVersion;
        //string profile = null;
        //if (group != null)
        //{
        //    // .NETFramework
        //    identifier = group.GetPropertyValue("TargetFrameworkIdentifier") ?? DefaultIdentifier;
        //    // 4.8
        //    versionString = group.GetPropertyValue("TargetFrameworkVersion") ?? DefaultFrameworkVersion;
        //    // SE
        //    profile = group.GetPropertyValue("TargetFrameworkProfile");
        //    var version = Version.Parse(versionString.Replace("v", "").Replace("V", ""));
        //    return new FrameworkName(identifier, version, profile);
        //}
        //return null; // DetectNetSdkVersion();
    }
}