namespace SunamoToNetCore.research;

public partial class MoveToNet5
{
    // It probably breaks the csproj
    // A1 can be
    //
    // This probably did not make much sense. When I changed it everywhere in any solution from AnyCPU to x86, I got a blue icon on many projects.
    // Moreover text with this setting, when I ran EveryLine and although the sln was set to AnyCPU, right at startup it said it could not load EveryLine. When I changed its csproj to AnyCPU, the error started to be reported again for desktop.
    //
    // A1 can be x86,x64.AnyCPU
    /// <summary>
    /// Platform target to web.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="replaceFor">The replace for.</param>
    public
    async Task
    PlatformTargetToWeb(ILogger logger, string replaceFor)
    {
        var temp = WebAndNonWebProjects(logger);
        var webProjects = temp.Item1;
        replaceFor =
    await
        Shared.PlatformTargetTo(replaceFor, webProjects);
    }

    // 25-9-2022 Because something deleted the assembly with *.sunamo.cz csproj again - .Web.Services, Data, .Web => commented, replacement below.
    // System.Web.Services
    const string neededWebReferences = "";

    /// <summary>
    /// Add essential web references to all web projects.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public
    async Task
    AddEssentialWebReferencesToAllWebProjects(ILogger logger)
    {
        var list = SHGetLines.GetLines(neededWebReferences);
        var temp =
    await
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

        var neededReferences = SHGetLines.GetLines(neededWebReferences);
        foreach (var item in temp.CsprojSdkStyleList)
        {
            foreach (var item2 in neededReferences)
            {
                var rig = new ReferenceItemGroup(item2, item, null!);
                // This apparently had to be here because of using AddItemGroupNoSdkStyle. Now it causes problems because .dll does not belong there
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }

        foreach (var item in temp.NonCsprojSdkStyleList)
        {
            foreach (var item2 in neededReferences)
            {
                var rig = new ReferenceItemGroup(item2, item, null!);
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }
    }

    /// <summary>
    /// Change projects to net standard.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public
    async Task
    ChangeProjectsToNetStandard(ILogger logger)
    {
        var list =
    await
        FindProjectsWhichIsSdkStyle(logger, false);
        await ChangeProjects.ChangeProjectsTo(ChangeProjects.netstandard20, list.CsprojSdkStyleList);
    }

    /// <summary>
    /// Web projects which is not sdk style find their backup.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The web projects which is not sdk style find their backup result.</returns>
    public
    async Task<string>
    WebProjectsWhichIsNotSdkStyleFindTheirBackup(ILogger logger)
    {
        List<string> haveBackupSdkStyle = new List<string>();
        List<string> dontHaveBackupSdkStyle = new List<string>();
        List<string> dontHaveBackup = new List<string>();
        var list =
    await
        FindProjectsWhichIsSdkStyle(logger, false);
        foreach (var item in list.CsprojSdkStyleList)
        {
            throw new Exception("žádné koncovky old v csproj tu nemám. tak tedy nevím co jsem tu chtěl dělat ");
        }

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(haveBackupSdkStyle, nameof(haveBackupSdkStyle));
        tog.List(dontHaveBackupSdkStyle, nameof(dontHaveBackupSdkStyle));
        tog.List(dontHaveBackup, nameof(dontHaveBackup));
        return tog.ToString();
    }

    /// <summary>
    /// Detect framework for web projects only supported.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The detect framework for web projects only supported result.</returns>
    public
    async Task<string>
    DetectFrameworkForWebProjectsOnlySupported(ILogger logger)
    {
        TextOutputGenerator tog = new TextOutputGenerator();
        Dictionary<SupportedNetFw, StringBuilder> stringBuilder = new Dictionary<SupportedNetFw, StringBuilder>();
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            var name =
                await
            SunamoCsprojHelper.DetectNetVersion2(item);
            // Inlined from DictionaryHelper.AppendLineOrCreate - appends a line to a StringBuilder or creates a new one
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

        return tog.ToString();
    }

    /// <summary>
    /// Convert al l web net standard projects to net 48.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public async Task ConvertAlLWebNetStandardProjectsToNet48(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            await ChangeProjects.ChangeProjectTo(ChangeProjects.net48, item, null, ChangeProjects.netstandard20);
        }
    }

    /// <summary>
    /// Web projects which not end with dot end.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The web projects which not end with dot end result.</returns>
    public string WebProjectsWhichNotEndWithDotEnd(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger, true);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in temp.Item1)
        {
            // Everything here must be without a trailing slash so that I also catch the .Tests postfix
            if (!item.EndsWith(".web.csproj") /*&& !item.Contains(".web64") && !item.Contains(".web5") && !item.Contains(@"\sunamo.cz") && !item.Contains(@"\sunamo.cz-old") && !item.Contains(@"\sunamo.cz64") && !item.Contains(@"\sunamo.web")*/)
            {
                stringBuilder.AppendLine(item);
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Detect framework for web projects.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="appendHeader">The append header.</param>
    /// <returns>The detect framework for web projects result.</returns>
    public
    async Task<Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>>
    DetectFrameworkForWebProjects(ILogger logger, bool appendHeader)
    {
        List<TWithStringDC<string>> list = new List<TWithStringDC<string>>();
        List<TWithStringDC<string>> netStandardList = new List<TWithStringDC<string>>();
        if (appendHeader)
        {
            list.Add(new TWithStringDC<string>("", "Web but in SDK style:"));
        }

        bool netstandard = false;
        var temp = WebAndNonWebProjects(logger);
        Tuple<bool, string>? netVersion = null;
        foreach (var item2 in temp.Item1)
        {
            netVersion =
    await
            SunamoCsprojHelper.DetectNetVersion(item2);
            if (netVersion != null)
            {
                if (netVersion.Item1)
                {
                    if (netstandard)
                    {
                        netStandardList.Add(new TWithStringDC<string>(item2, netVersion.Item2));
                    }
                    else
                    {
                        list.Add(new TWithStringDC<string>(item2, netVersion.Item2));
                    }
                }
            }
        }

        return new Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>(list, netStandardList);
    }

    /// <summary>
    /// Find projects which is sdk style list.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="appendHeaderForWeb">The append header for web.</param>
    /// <param name="web">The web.</param>
    /// <returns>The find projects which is sdk style list result.</returns>
    public
    async Task<string>
    FindProjectsWhichIsSdkStyleList(ILogger logger, bool appendHeaderForWeb, bool web = true)
    {
        var result =
    await
        FindProjectsWhichIsSdkStyle(logger, appendHeaderForWeb, web);
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(result.CsprojSdkStyleList, nameof(result.CsprojSdkStyleList));
        tog.List(result.NetstandardList, nameof(result.NetstandardList));
        tog.List(result.NonCsprojSdkStyleList, nameof(result.NonCsprojSdkStyleList));
        return tog.ToString();
    }
}