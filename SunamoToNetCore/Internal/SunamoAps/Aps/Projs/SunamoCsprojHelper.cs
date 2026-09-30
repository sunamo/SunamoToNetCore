namespace SunamoAps.Aps.Projs;

internal partial class SunamoCsprojHelper
{
    // Must have postfix 2 because IsProjectCsprojSdkStyleIsCore has also a same-argument method.
    private static
    async Task<bool>
    IsProjectCsprojSdkStyleIsCore(string fileOrContent, bool onlyContentIsPassed)
    {
        if (!onlyContentIsPassed)
        {
            if (!fileOrContent.StartsWith("<") && FS.ExistsFile(fileOrContent))
            {
                var readContent =
    await
                TF.ReadAllText(fileOrContent);
                fileOrContent = readContent!;
            }
        }

        // bez ukončení závorkama protože může být i <Project Sdk="Microsoft.NET.Sdk.WindowsDesktop"> atd.
        return fileOrContent!.Contains("Sdk=\"Microsoft.NET.Sdk");
    }

    public static
    async Task<IsProjectCsprojSdkStyleResult?>
    IsProjectCsprojSdkStyleIsCore(string fileOrContent)
    {
        bool netstandard = false;
        if (!fileOrContent.StartsWith("<") && FS.ExistsFile(fileOrContent))
        {
            var xd =
            await
            XmlDocumentsCache.Get(fileOrContent);
            if (xd.Data == null)
            {
                return null;
            }

            fileOrContent = xd.Data.OuterXml;
        }

        if (fileOrContent.Contains("<TargetFramework>netstandard2.0</TargetFramework>"))
        {
            netstandard = true;
        }

        return new IsProjectCsprojSdkStyleResult
        {
            Content = fileOrContent,
            IsProjectCsprojSdkStyleIsCore =
    await
            IsProjectCsprojSdkStyleIsCore(fileOrContent, true),
            IsNetstandard = netstandard
        };
    }

    // Whether is old .net fw, version
    public static
    async Task<Tuple<bool, string>>
    DetectNetVersion(string path)
    {
        var xml =
        await
        XmlDocumentsCache.Get(path);
        if (MayExcHelper.MayExc(xml.Exc))
        {
            return null!;
        }

        if (xml.Data == null)
        {
            return null!;
        }

        var csprojContent = xml.Data.OuterXml;
        var isNew =
    await
        IsProjectCsprojSdkStyleIsCore(csprojContent, true);
        if (isNew)
        {
            //// Probably will be first because "You need to reference the Microsoft.Build.Engine assembly"
            //Microsoft.Build.Evaluation.Project project = new Microsoft.Build.Evaluation.Project();
            ////Microsoft.CodeAnalysis.Project project = new Microsoft.CodeAnalysis.Project();
            ////Project project = new Project();
            //project.Load(fullPathName);
            //var embeddedResources =
            //    from grp in project.ItemGroups.Cast<BuildItemGroup>()
            //    from item in grp.Cast<BuildItem>()
            //    where item.Name == "EmbeddedResource"
            //    select item;
            var list = SHGetLines.GetLines(csprojContent);
            // žádný https://www.nuget.org/packages?q=Sunamo+Metaproject není, proto není ani TargetFramework a ctor text 2Mi args
            var csp = CsprojFileParser.ParseCsproj( /*list,*/path);
            return new Tuple<bool, string>(isNew, /*csp.TargetFramework*/ null!);
        }

        var value = FrameworkNameDetector.Detect(path);
        return new Tuple<bool, string>(isNew, VersionHelper.RemovePartsWhichIsZero(value.Version));
    }

    public static
    async Task<SupportedNetFw>
    DetectNetVersion2(string path)
    {
        var temp =
            await
        DetectNetVersion(path);
        if (temp != null)
        {
            if (temp.Item1)
            {
                var text = SHParts.RemoveAfterFirst(temp.Item2, '-');
                text = text.Replace(".", String.Empty);
                return EnumHelper.Parse(text, SupportedNetFw.None);
            }
            else
            {
                if (temp.Item2 == "4.8")
                {
                    return SupportedNetFw.net48;
                }

                return SupportedNetFw.None;
            }
        }

        return SupportedNetFw.BadXml;
    }

    public static
        async Task<List<string>>
    BuildProjectsDependencyTreeList(string csprojPath, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged)
    {
        XmlDocumentsCache.CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull.Clear();
        var result =
            await
        BuildProjectsDependencyTree2(csprojPath, dictToAvoidCollectionWasChanged);
        return result.Collection;
    }

    static
        async Task<CollectionWithoutDuplicatesDC<string>>
    BuildProjectsDependencyTree2(string csprojPath, Dictionary<string, XmlDocument>? dictToAvoidCollectionWasChanged = null)
    {
        CollectionWithoutDuplicatesDC<string> paths = new CollectionWithoutDuplicatesDC<string>();
        await
        BuildProjectsDependencyTree(paths, csprojPath, dictToAvoidCollectionWasChanged);
        return paths;
    }

    static
        async Task
    BuildProjectsDependencyTree(CollectionWithoutDuplicatesDC<string> paths, string csprojPath, Dictionary<string, XmlDocument>? dictToAvoidCollectionWasChanged = null)
    {
        if (Ignored.IsIgnored(csprojPath))
        {
            return;
        }

        // This is problematic - in this situation I have only undamaged projects.
        // For example, for E:\vs\Projects\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths.csproj
        // it threw an error, but when I ran it again with just this file, it passed
        var result =
            await
        VsProjectsFileHelper.GetProjectReferences(csprojPath, dictToAvoidCollectionWasChanged!);
        if (result.Projects == null)
        {
            // debug step by step now
            result =
                await
            VsProjectsFileHelper.GetProjectReferences(csprojPath, null!);
            if (result.Projects == null)
            {
                result =
                    await
                VsProjectsFileHelper.GetProjectReferences(csprojPath, dictToAvoidCollectionWasChanged!);
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                XmlDocumentsCache.CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull.Add(csprojPath);
            }
        }

        // I don't understand how this is possible, but for some projects on VPS it returns paths as they exist on my machine, so I must check for null
        if (result.Projects != null)
        {
            paths.AddRange(result.Projects);
            foreach (var item in result.Projects)
            {
                if (FS.ExistsFile(item))
                {
                    await BuildProjectsDependencyTree(paths, item, dictToAvoidCollectionWasChanged);
                }
            }
        }
    }

    public static
        async Task
    AddMissingProjects(SolutionFolder solutionFolder, bool isAddingDependencies = false)
    {
        var text = ApsHelper.Instance.MainSln(solutionFolder);
        if (text == null)
        {
            ThisApp.Error($"Sln with name {solutionFolder.NameSolution} doesn't have main sln");
            return;
        }

        await
        AddMissingProjectsAlsoString(text, isAddingDependencies);
    }
}