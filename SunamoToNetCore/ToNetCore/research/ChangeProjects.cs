namespace SunamoToNetCore;

public class ChangeProjects
{
    public const string start = "<TargetFramework>";
    public const string end = "</TargetFramework>";
    public const string netstandard20 = "netstandard2.0";
    public const string net48 = "net48";
    public const string net60 = "net6.0";
    // Na mém PC bude mít vše net7.0-windows
    // Bylo by to velmi náročné dělat průběžné změny mezi net7.0 argument net7.0-windows
    public const string net70Windows = "net7.0-windows";
    public const string netstandard21 = "netstandard2.1";
    public const string net5Uap = "net5.0-windows10.0.19041.0";
    public const string netcoreapp = "netcoreapp1.1";
    public const string net472 = "net472";
    public const string net7Windows = "net7.0-windows";

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

    // Replace only between TargetFramework
    public static
    async Task
 ChangeProjectTo(string to2, string path, IsNetCore5UpMonikerResult? parsedMonikerTo, string? dontChangeIfSourceIs = null)
    {
        var xd =
    await
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
            await
         TF.WriteAllText(path, content);
        }
    }

    public static async Task ChangeProjectsTo(string to2, List<string> vs)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in vs)
        {
            await ChangeProjectTo(to2, item, parsedMonikerTo);
        }
    }
}
