namespace SunamoAps.Aps.Projs;

// Use VsProjectFile,
internal partial class VsProjectsFileHelper
{
    public static List<string> GetTokens(string relativePath)
    {
        var deli = "";
        if (relativePath.Contains("\""))
            deli = "\"";
        else if (relativePath.Contains("/")) deli = "/";
        else
        {
            ThrowEx.NotImplementedCase(relativePath);
        }
        return SHSplit.Split(relativePath, deli);
    }
    #region Use cacheProjectReferences
    public static Dictionary<string, ProjectReferences> cacheProjectReferences = new Dictionary<string, ProjectReferences>();
    //public async static Task<ProjectReferences> GetProjectReferencesAsync(string csprojPath, UriKind uri = UriKind.Absolute)
    public static
        async Task<ProjectReferences>
        GetProjectReferences(string csprojPath, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged, UriKind uriKind = UriKind.Absolute)
    {
        // Not
        //ThrowEx.FirstLetterIsNotUpper(csprojPath);
        csprojPath = SH.FirstCharUpper(csprojPath);
        if (cacheProjectReferences.ContainsKey(csprojPath))
        {
            return cacheProjectReferences[csprojPath];
        }
        if (!FS.ExistsFile(csprojPath))
        {
            return new ProjectReferences();
        }
        var vs = new VsProjectFile();
        await
        vs.Load(csprojPath, dictToAvoidCollectionWasChanged);
        if (!vs.IsValidXml)
        {
            return new ProjectReferences();
        }
        var nodes = vs.ReturnAllItemGroup(ItemGroups.ProjectReference);
        var projectReferences = nodes.Select(d => XmlHelper.Attr(d, "Include")!).ToList();
        var dr = FS.GetDirectoryName(csprojPath);
        if (uriKind == UriKind.Absolute)
        {
            CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, projectReferences, FS.GetAbsolutePath2, dr);
        }
        else if (uriKind == UriKind.Relative)
        {
            CAChangeContent.ChangeContent(new ChangeContentArgsDC { SwitchFirstAndSecondArg = true }, projectReferences, null!, dr, PathPolyfill.GetRelativePath);
        }
        var pr = new ProjectReferences { Projects = projectReferences, Nodes = nodes };
        if (!cacheProjectReferences.ContainsKey(csprojPath))
        {
            cacheProjectReferences.Add(csprojPath, pr);
        }
        return pr;
    }
    #endregion
    public static async Task AddFilesToCsproj(SolutionFolder sln, string csprojpath, List<string> files)
    {
        var containedFiles = new List<string>();
        var dir = FS.GetDirectoryName(csprojpath);
        var vs = new VsProjectFile(csprojpath);
        if (!vs.IsValidXml)
        {
            return;
        }
        var compile = vs.ReturnAllItemGroup(ItemGroups.Compile).Select(d => XmlHelper.Attr(d, "Include")!);
        foreach (var item in compile)
        {
            containedFiles.Add(FS.GetAbsolutePath(dir, item!));
        }
        foreach (var item in files)
        {
            if (!containedFiles.Contains(item))
            {
                var c = new CompileItemGroup(csprojpath);
                var relativePathFromSolution = ApsHelper.Instance.GetRelativePathFromSolution(sln, item);
                var tokens = FS.GetTokens(relativePathFromSolution);
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);
                c.Include = Path.Combine(tokens.ToArray());
                await AddItemGroupSdkStyle(csprojpath, ItemGroups.Compile, c, true);
            }
        }
    }
}