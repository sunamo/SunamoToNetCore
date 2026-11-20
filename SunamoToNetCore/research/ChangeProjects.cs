namespace SunamoToNetCore.research;

/// <summary>
/// Represents the change projects type.
/// </summary>
public class ChangeProjects
{
    /// <summary>
    /// The start constant.
    /// </summary>
    public const string start = "<TargetFramework>";
    /// <summary>
    /// The end constant.
    /// </summary>
    public const string end = "</TargetFramework>";
    /// <summary>
    /// The netstandard 20 constant.
    /// </summary>
    public const string netstandard20 = "netstandard2.0";
    /// <summary>
    /// The net 48 constant.
    /// </summary>
    public const string net48 = "net48";
    /// <summary>
    /// The net 60 constant.
    /// </summary>
    public const string net60 = "net6.0";
    // On my PC everything will have net7.0-windows
    // It would be very demanding to make continuous changes between net7.0 and net7.0-windows
    /// <summary>
    /// The net 70 windows constant.
    /// </summary>
    public const string net70Windows = "net7.0-windows";
    /// <summary>
    /// The netstandard 21 constant.
    /// </summary>
    public const string netstandard21 = "netstandard2.1";
    /// <summary>
    /// The net 5 uap constant.
    /// </summary>
    public const string net5Uap = "net5.0-windows10.0.19041.0";
    /// <summary>
    /// The netcoreapp constant.
    /// </summary>
    public const string netcoreapp = "netcoreapp1.1";
    /// <summary>
    /// The net 472 constant.
    /// </summary>
    public const string net472 = "net472";
    /// <summary>
    /// The net 7 windows constant.
    /// </summary>
    public const string net7Windows = "net7.0-windows";

    /// <summary>
    /// Test.
    /// </summary>
    public static void Test()
    {
        var argument = ChangeProjects.IsNetCore5UpMoniker(netstandard20);
        var builder = ChangeProjects.IsNetCore5UpMoniker(netcoreapp);
        var count = ChangeProjects.IsNetCore5UpMoniker(net70Windows);
        var data = ChangeProjects.IsNetCore5UpMoniker(net7Windows);
    }

    /// <summary>
    /// Is net core 5 up moniker.
    /// </summary>
    /// <param name="moniker">The moniker.</param>
    /// <returns>The is net core 5 up moniker result.</returns>
    public static IsNetCore5UpMonikerResult? IsNetCore5UpMoniker(string moniker)
    {
        if (!moniker.StartsWith("net"))
        {
            return null;
        }
        if (moniker.Length < 6)
        {
            return null;
        }
        if (BTS.IsInt(moniker[3].ToString()) && moniker[4] == '.' && BTS.IsInt(moniker[5].ToString()))
        {
            // Inlined from SHSubstring.SubstringIfAvailableStart - creates a substring if available
            var platformTfm = moniker.Length > 6 ? moniker.Substring(6) : moniker;
            return new IsNetCore5UpMonikerResult { TargetFramework = moniker.Substring(0, 6), PlatformTfm = platformTfm };
        }
        return null;
    }

    /// <summary>
    /// Change projects to.
    /// </summary>
    /// <returns>The change projects to result.</returns>
    public static async Task ChangeProjectsTo(string to2, List<TWithStringDC<string>> projects)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in projects)
        {
            var content = item.t;
            var path = item.path;
            await ChangeProjectTo(to2, path, parsedMonikerTo);
        }
    }

    // Replace only between TargetFramework
    /// <summary>
    /// Change project to.
    /// </summary>
    /// <param name="to2">The to 2.</param>
    /// <param name="path">The path.</param>
    /// <param name="parsedMonikerTo">The parsed moniker to.</param>
    /// <param name="dontChangeIfSourceIs">The dont change if source is.</param>
    public static
    async Task
 ChangeProjectTo(string to2, string path, IsNetCore5UpMonikerResult? parsedMonikerTo, string? dontChangeIfSourceIs = null)
    {
        var documentResult =
    await
 XmlDocumentsCache.Get(path);
        if (MayExcHelper.MayExc(documentResult.Exc))
        {
            return;
        }
        var content = documentResult.Data.OuterXml;
        var targetFramework = SH.GetTextBetween(content, start, end, false);
        if (targetFramework == null)
        {
            // It can happen when it is not in non-sdk style
            return;
        }
        if (dontChangeIfSourceIs != null && dontChangeIfSourceIs == targetFramework)
        {
            return;
        }
        var parsedMonikerFrom = IsNetCore5UpMoniker(targetFramework);
        // I do not want it anymore, it is not enough for targetFramework to be the same, the TFM must be the same too. Everything on my computer will be -windows
        if (targetFramework != to2)
        {
            string? from = null;
            string? to = null;
            from = start + targetFramework + end;
            if (parsedMonikerFrom == null || parsedMonikerTo == null)
            {
                // it is not net core, I can replace it with anything
                to = start + to2 + end;
            }
            else
            {
                IsNetCore5UpMonikerResult? monikerTo = null;
                if (parsedMonikerFrom.PlatformTfm != "")
                {
                    monikerTo = new IsNetCore5UpMonikerResult()
                    {
                        TargetFramework = parsedMonikerTo.TargetFramework,
                        PlatformTfm = parsedMonikerFrom.PlatformTfm
                    };
                }
                else
                {
                    monikerTo = parsedMonikerTo;
                }
                to = start + monikerTo.ToString() + end;
            }
            content = content.Replace(from, to);
            await
         TF.WriteAllText(path, content);
        }
    }

    /// <summary>
    /// Initializes a new instance of the change projects to class.
    /// </summary>
    /// <param name="!">The !.</param>
    public static async Task ChangeProjectsTo(string to2, List<string> projectPaths)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in projectPaths)
        {
            await ChangeProjectTo(to2, item, parsedMonikerTo);
        }
    }
}