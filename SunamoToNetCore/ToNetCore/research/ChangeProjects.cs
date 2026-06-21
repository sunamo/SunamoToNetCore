namespace SunamoDevCode.ToNetCore.research;

/// <summary>
/// Provides methods for changing target framework monikers in .csproj project files.
/// </summary>
public class ChangeProjects
{
    /// <summary>
    /// Opening XML tag for TargetFramework element in csproj files.
    /// </summary>
    public const string start = "<TargetFramework>";
    /// <summary>
    /// Closing XML tag for TargetFramework element in csproj files.
    /// </summary>
    public const string end = "</TargetFramework>";
    /// <summary>
    /// Target framework moniker for .NET Standard 2.0.
    /// </summary>
    public const string netstandard20 = "netstandard2.0";
    /// <summary>
    /// Target framework moniker for .NET Framework 4.8.
    /// </summary>
    public const string net48 = "net48";
    /// <summary>
    /// Target framework moniker for .NET 6.0.
    /// </summary>
    public const string net60 = "net6.0";
    /// <summary>
    /// Na mém PC bude mít vše net7.0-windows
    /// Bylo by to velmi náročné dělat průběžné změny mezi net7.0 argument net7.0-windows
    /// </summary>
    public const string net70Windows = "net7.0-windows";
    /// <summary>
    /// Target framework moniker for .NET Standard 2.1.
    /// </summary>
    public const string netstandard21 = "netstandard2.1";
    /// <summary>
    /// Target framework moniker for .NET 5.0 UWP/UAP Windows platform.
    /// </summary>
    public const string net5Uap = "net5.0-windows10.0.19041.0";
    /// <summary>
    /// Target framework moniker for .NET Core App 1.1.
    /// </summary>
    public const string netcoreapp = "netcoreapp1.1";
    /// <summary>
    /// Target framework moniker for .NET Framework 4.7.2.
    /// </summary>
    public const string net472 = "net472";
    /// <summary>
    /// Target framework moniker for .NET 7.0 Windows platform.
    /// </summary>
    public const string net7Windows = "net7.0-windows";
    /// <summary>
    /// Tests the IsNetCore5UpMoniker method with various target framework monikers.
    /// </summary>
    public static void Test()
    {
        var argument = ChangeProjects.IsNetCore5UpMoniker(netstandard20);
        var builder = ChangeProjects.IsNetCore5UpMoniker(netcoreapp);
        var count = ChangeProjects.IsNetCore5UpMoniker(net70Windows);
        var data = ChangeProjects.IsNetCore5UpMoniker(net7Windows);
    }
    //public static void ChangeProjectsTo(ILogger logger, string to2, bool web)
    //{
    //    var text = MoveToNet5.Instance.WebAndNonWebProjects(logger, true);
    //    ChangeProjectsTo( to2, web);
    //}
    /// <summary>
    /// Parses a target framework moniker to determine if it is a .NET 5+ style moniker (e.g., net5.0, net7.0-windows).
    /// </summary>
    /// <param name="moniker">Target framework moniker string to parse.</param>
    /// <returns>Parsed result with framework version and platform TFM, or null if not a .NET 5+ moniker.</returns>
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
            // Inlined from SHSubstring.SubstringIfAvailableStart - vytváří substring pokud je dostupný
            var platformTfm = moniker.Length > 6 ? moniker.Substring(6) : moniker;
            return new IsNetCore5UpMonikerResult { TargetFramework = moniker.Substring(0, 6), PlatformTfm = platformTfm };
        }
        return null;
    }
    /// <summary>
    /// Changes target framework in all project files from the list to the specified framework moniker.
    /// </summary>
    /// <param name="to2">Target framework moniker to change to.</param>
    /// <param name="l">List of project items containing content and path.</param>
    public static async Task ChangeProjectsTo(string to2, List<TWithStringDC<string>> l)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in l)
        {
            var content = item.t;
            var path = item.path;
            await ChangeProjectTo(to2, path, parsedMonikerTo);
        }
    }

    /// <summary>
    /// Replace only between TargetFramework
    /// </summary>
    /// <param name="to2">Target framework moniker to change to.</param>
    /// <param name="path">Path to the csproj file.</param>
    /// <param name="parsedMonikerTo">Parsed target moniker result.</param>
    /// <param name="dontChangeIfSourceIs">If the current framework matches this value, skip the change.</param>
    public static
#if ASYNC
    async Task
#else
    void
#endif
 ChangeProjectTo(string to2, string path, IsNetCore5UpMonikerResult? parsedMonikerTo, string? dontChangeIfSourceIs = null)
    {
#if DEBUG
        //if (path.Contains("ExCSS2.csproj"))
        //{
        //}
        //if (path == @"E:\vs\Mono_Projects\monoConsoleSqlClient\consoleSqlClient\monoConsoleSqlClient.csproj")
        //{
        //    //net7.0 
        //}
        //if (path == @"E:\vs\Projects\_ut2\AllProjectsSearch.Cmd.Tests\Runner\Runner.csproj")
        //{
        //    // net6.0-windows
        //}
        //if (path == @"E:\vs\Mono_Projects\monoConsoleSqlClient\consoleSqlClient\monoConsoleSqlClient.csproj")
        //{
        //    //net7.0
        //}
#endif
        var xd =
#if ASYNC
    await
#endif
 XmlDocumentsCache.Get(path);
        if (MayExcHelper.MayExc(xd.Exc))
        {
            return;
        }
        var content = xd.Data.OuterXml;
        var tf = SH.GetTextBetween(content, start, end, false);
        if (tf == null)
        {
            // Může se stát když to není v non sdk style
            return;
        }
        if (dontChangeIfSourceIs != null && dontChangeIfSourceIs == tf)
        {
            return;
        }
        var parsedMonikerFrom = IsNetCore5UpMoniker(tf);
        // už nechci, nestačí aby byly stejné targetFramework, musí být stejné i TFM. Vše na mém kompu bude -windows
        //if (parsedMonikerFrom?.TargetFramework == parsedMonikerTo?.TargetFramework)
        //{
        //    return;
        //}
        if (tf != to2)
        {
            string? from = null;
            string? to = null;
            from = start + tf + end;
            if (parsedMonikerFrom == null || parsedMonikerTo == null)
            {
                // není to net core, můžu to nahradit za cokoliv
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
#if ASYNC
            await
#endif
         TF.WriteAllText(path, content);
        }
    }
    /// <summary>
    /// Changes target framework in all project files at the specified paths to the given framework moniker.
    /// </summary>
    /// <param name="to2">Target framework moniker to change to.</param>
    /// <param name="vs">List of csproj file paths to update.</param>
    public static async Task ChangeProjectsTo(string to2, List<string> vs)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in vs)
        {
            await ChangeProjectTo(to2, item, parsedMonikerTo);
        }
    }
}