namespace SunamoToNetCore._sunamo.SunamoAps.Aps.Projs;

internal partial class SunamoCsprojHelper
{
    // Must have postfix 2 because IsProjectCsprojSdkStyleIsCore has also a same-argument method.
    /// <summary>
    /// Is project csproj sdk style is core.
    /// </summary>
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

    /// <summary>
    /// Is project csproj sdk style is core.
    /// </summary>
    public static
    async Task<IsProjectCsprojSdkStyleResult?>
    IsProjectCsprojSdkStyleIsCore(string fileOrContent)
    {
        bool netstandard = false;
        if (!fileOrContent.StartsWith("<") && FS.ExistsFile(fileOrContent))
        {
            var documentResult =
            await
            XmlDocumentsCache.Get(fileOrContent);
            if (documentResult.Data == null)
            {
                return null;
            }

            fileOrContent = documentResult.Data.OuterXml;
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
    /// <summary>
    /// Detect net version.
    /// </summary>
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

    /// <summary>
    /// Detect net version2.
    /// </summary>
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
}