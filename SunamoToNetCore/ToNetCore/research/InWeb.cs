namespace SunamoDevCode.ToNetCore.research;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class MoveToNet5
{
    /// <summary>
    /// Pravděpodobně to kurví csproj
    /// A1 can be
    ///
    /// Tohle asi nemělo příliš smysl. Když jsem to změnil celé v jakémkoliv řešení z AnyCPU na x86 tak jsem měl u plno projektů modrou ikonku.
    /// Navíc text tímto nastavením když jsem spustil EveryLine a ač sln bylo nastavené AnyCPu, hned po spuštení že nemůže načíst EveryLine. Když jsem jeho csproj změnil na AnyCPU, chybu začalo hlásit zase u desktop.
    ///
    /// A1 can be x86,x64.AnyCPU
    /// </summary>
    /// <summary>
    /// Changes the PlatformTarget for all web projects.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="replaceFor">Target platform value to set (x86, x64, AnyCPU).</param>
    public
#if ASYNC
    async Task
#else
    void
#endif
    PlatformTargetToWeb(ILogger logger, string replaceFor)
    {
        var temp = WebAndNonWebProjects(logger);
        var tt = temp.Item1;
        replaceFor = 
#if ASYNC
    await
#endif
        Shared.PlatformTargetTo(replaceFor, tt);
    }

    // 25-9-2022 Protože mi opět něco smazalo assembly se *.sunamo.cz csproj - .Web.Services, Data, .Web => commented, dole náhrada.
    //     const string neededWebReferences = @"System.Web
    // System.Web.Services
    // System.Data";
    const string neededWebReferences = "";
    /// <summary>
    /// Adds essential web references (System.Web, etc.) to all web projects that are missing them.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public
#if ASYNC
    async Task
#else
    void
#endif
    AddEssentialWebReferencesToAllWebProjects(ILogger logger)
    {
        var list = SHGetLines.GetLines(neededWebReferences);
        var temp = 
#if ASYNC
    await
#endif
        FindProjectsWhichIsSdkStyle(logger, false);
        StringBuilder stringBuilder = new StringBuilder();
        if (temp.NetstandardList.Count > 0)
        {
            stringBuilder.AppendLine("Web projects which is in web standard");
            foreach (var item in temp.NetstandardList)
            {
                stringBuilder.AppendLine(item);
            }
        }

        var l2 = SHGetLines.GetLines(neededWebReferences);
        //CA.PostfixIfNotEnding(".dll", l2);
        foreach (var item in temp.CsprojSdkStyleList)
        {
            foreach (var item2 in l2)
            {
                var rig = new ReferenceItemGroup(item2, item, null!);
                // Toto tu muselo být zřejmě kvůli užívání AddItemGroupNoSdkStyle. Teď mi to dělá problémy protože .dll tam nepatří
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }

        // 1 = sdk style, not netstandard2.0
        // 2 = sdk style, netstandard2.0
        // 3 = non sdk style
        foreach (var item in temp.NonCsprojSdkStyleList)
        {
            foreach (var item2 in l2)
            {
#if DEBUG
                if (item.Contains("dart.sunamo.cz") && item2.Contains("System.Web"))
                {
                }
#endif
                var rig = new ReferenceItemGroup(item2, item, null!);
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }
    }

    /// <summary>
    /// Changes all SDK-style web projects to target netstandard2.0.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public
#if ASYNC
    async Task
#else
    void
#endif
    ChangeProjectsToNetStandard(ILogger logger)
    {
        var list = 
#if ASYNC
    await
#endif
        FindProjectsWhichIsSdkStyle(logger, false);
        await ChangeProjects.ChangeProjectsTo(ChangeProjects.netstandard20, list.CsprojSdkStyleList);
    }

    /// <summary>
    /// Finds backup files for web projects that are not SDK-style and categorizes them.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Report of backup status for each project.</returns>
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
    WebProjectsWhichIsNotSdkStyleFindTheirBackup(ILogger logger)
    {
        List<string> haveBackupSdkStyle = new List<string>();
        List<string> dontHaveBackupSdkStyle = new List<string>();
        List<string> dontHaveBackup = new List<string>();
        var list = 
#if ASYNC
    await
#endif
        FindProjectsWhichIsSdkStyle(logger, false);
        foreach (var item in list.CsprojSdkStyleList)
        {
            throw new Exception("žádné koncovky old v csproj tu nemám. tak tedy nevím co jsem tu chtěl dělat ");
        //var old = item + ".old";
        //if (FS.ExistsFile(old))
        //{
        //    if (SunamoCsprojHelper.IsProjectCsprojSdkStyleIsCore(old, false))
        //    {
        //        haveBackupSdkStyle.Add(item);
        //    }
        //    else
        //    {
        //        dontHaveBackupSdkStyle.Add(item);
        //    }
        //}
        //else
        //{
        //    dontHaveBackup.Add(item);
        //}
        }

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(haveBackupSdkStyle, nameof(haveBackupSdkStyle));
        tog.List(dontHaveBackupSdkStyle, nameof(dontHaveBackupSdkStyle));
        tog.List(dontHaveBackup, nameof(dontHaveBackup));
        return tog.ToString();
    }

    /// <summary>
    /// Detects the .NET framework version for all web projects and groups them by version.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Report grouped by framework version.</returns>
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
    DetectFrameworkForWebProjectsOnlySupported(ILogger logger)
    {
        TextOutputGenerator tog = new TextOutputGenerator();
        Dictionary<SupportedNetFw, StringBuilder> stringBuilder = new Dictionary<SupportedNetFw, StringBuilder>();
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            var name = 
#if ASYNC
                await
#endif
            SunamoCsprojHelper.DetectNetVersion2(item);
            // Inlined from DictionaryHelper.AppendLineOrCreate - přidává řádek do StringBuilderu nebo vytváří nový
            if (stringBuilder.ContainsKey(name))
            {
                stringBuilder[name].AppendLine(item);
            }
            else
            {
                var sb2 = new StringBuilder();
                sb2.AppendLine(item);
                stringBuilder.Add(name, sb2);
            }
        }

        foreach (var item in stringBuilder)
        {
            tog.ListSB(item.Value, item.Key.ToString());
        }

        //Output = tog.ToString();
        //OutputOpen();
        return tog.ToString();
    }

    /// <summary>
    /// Converts all web projects targeting netstandard2.0 to target net4.8.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public async Task ConvertAlLWebNetStandardProjectsToNet48(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            await ChangeProjects.ChangeProjectTo(ChangeProjects.net48, item, null, ChangeProjects.netstandard20);
        }
    }

    /// <summary>
    /// Lists web projects whose csproj files do not end with .web.csproj.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <returns>List of non-standard web project paths.</returns>
    public string WebProjectsWhichNotEndWithDotEnd(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger, true);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in temp.Item1)
        {
            // Vše zde musí být bez koncového lomítka abych podchytil i .Tests postfix
            if (!item.EndsWith(".web.csproj") /*&& !item.Contains(".web64") && !item.Contains(".web5") && !item.Contains(@"\sunamo.cz") && !item.Contains(@"\sunamo.cz-old") && !item.Contains(@"\sunamo.cz64") && !item.Contains(@"\sunamo.web")*/)
            {
                stringBuilder.AppendLine(item);
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 1 = sdk style, not netstandard2.0
    /// 2 = sdk style, netstandard2.0
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appendHeader">Whether to append a header line to the output.</param>
    /// <returns>Tuple of (SDK-style web projects, netstandard web projects) with version info.</returns>
    public 
#if ASYNC
    async Task<Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>>
#else
    Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>> 
#endif
    DetectFrameworkForWebProjects(ILogger logger, bool appendHeader)
    {
        List<TWithStringDC<string>> list = new List<TWithStringDC<string>>();
        List<TWithStringDC<string>> l2 = new List<TWithStringDC<string>>();
        if (appendHeader)
        {
            list.Add(new TWithStringDC<string>("", "Web but in SDK style:"));
        }

        bool netstandard = false;
        var temp = WebAndNonWebProjects(logger);
        Tuple<bool, string>? t3 = null;
        foreach (var item2 in temp.Item1)
        {
            t3 = 
#if ASYNC
    await
#endif
            SunamoCsprojHelper.DetectNetVersion(item2);
            if (t3 != null)
            {
                if (t3.Item1)
                {
                    if (netstandard)
                    {
                        l2.Add(new TWithStringDC<string>(item2, t3.Item2));
                    }
                    else
                    {
                        list.Add(new TWithStringDC<string>(item2, t3.Item2));
                    }
                }
            }
        }

        return new Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>(list, l2);
    }

    /// <summary>
    /// Finds all SDK-style projects and returns a formatted list report.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appendHeaderForWeb">Whether to append a header for web projects.</param>
    /// <param name="web">Whether to include web projects.</param>
    /// <returns>Formatted text report of SDK-style projects.</returns>
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
    FindProjectsWhichIsSdkStyleList(ILogger logger, bool appendHeaderForWeb, bool web = true)
    {
        var result = 
#if ASYNC
    await
#endif
        FindProjectsWhichIsSdkStyle(logger, appendHeaderForWeb, web);
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(result.CsprojSdkStyleList, nameof(result.CsprojSdkStyleList));
        tog.List(result.NetstandardList, nameof(result.NetstandardList));
        tog.List(result.NonCsprojSdkStyleList, nameof(result.NonCsprojSdkStyleList));
        //ProgramShared.Output = tog.ToString();
        //ProgramShared.OutputOpen();
        return tog.ToString();
    }
}