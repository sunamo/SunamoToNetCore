namespace SunamoToNetCore;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class MoveToNet5
{
    // Pravděpodobně to kurví csproj
    // A1 can be x86,x64.AnyCPU
    public
    async Task
    PlatformTargetToWeb(ILogger logger, string replaceFor)
    {
        var temp = WebAndNonWebProjects(logger);
        var tt = temp.Item1;
        replaceFor = 
    await
        Shared.PlatformTargetTo(replaceFor, tt);
    }

    // 25-9-2022 Protože mi opět něco smazalo assembly se *.sunamo.cz csproj - .Web.Services, Data, .Web => commented, dole náhrada.
    //     const string neededWebReferences = @"System.Web
    // System.Web.Services
    // System.Data";
    const string neededWebReferences = "";
    public
    async Task
    AddEssentialWebReferencesToAllWebProjects(ILogger logger)
    {
        _ = SHGetLines.GetLines(neededWebReferences);
        var temp = 
    await
        FindProjectsWhichIsSdkStyle(logger, false);
        StringBuilder stringBuilder = new();
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
                var rig = new ReferenceItemGroup(item2, item, null!);
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }
    }

    public
    async Task
    ChangeProjectsToNetStandard(ILogger logger)
    {
        var list = 
    await
        FindProjectsWhichIsSdkStyle(logger, false);
        await ChangeProjects.ChangeProjectsTo(ChangeProjects.netstandard20, list.CsprojSdkStyleList);
    }

    public
    async Task<string>
    WebProjectsWhichIsNotSdkStyleFindTheirBackup(ILogger logger)
    {
        List<string> haveBackupSdkStyle = new();
        List<string> dontHaveBackupSdkStyle = new();
        List<string> dontHaveBackup = new();
        var list = 
    await
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

    public
    async Task<string>
    DetectFrameworkForWebProjectsOnlySupported(ILogger logger)
    {
        TextOutputGenerator tog = new();
        Dictionary<SupportedNetFw, StringBuilder> stringBuilder = new();
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            var name = 
                await
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

    public async Task ConvertAlLWebNetStandardProjectsToNet48(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            await ChangeProjects.ChangeProjectTo(ChangeProjects.net48, item, null, ChangeProjects.netstandard20);
        }
    }

    public string WebProjectsWhichNotEndWithDotEnd(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger, true);
        StringBuilder stringBuilder = new();
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

    // 1 = sdk style, not netstandard2.0
    // 2 = sdk style, netstandard2.0
    public
    async Task<Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>>
    DetectFrameworkForWebProjects(ILogger logger, bool appendHeader)
    {
        List<TWithStringDC<string>> list = new();
        List<TWithStringDC<string>> l2 = new();
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
    await
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
        //ProgramShared.Output = tog.ToString();
        //ProgramShared.OutputOpen();
        return tog.ToString();
    }
}
