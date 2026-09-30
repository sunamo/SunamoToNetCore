namespace SunamoAps.Aps.Projs;

internal class ApsProjsHelper
{
    public static Type Type = typeof(ApsProjsHelper);

    public static void ReplacePathWithCsproj(ILogger logger, List<string> projectsToAdd)
    {
        for (int i = 0; i < projectsToAdd.Count; i++)
        {
            var item = projectsToAdd[i];
            var csproj = ApsHelper.Instance.GetCsprojsOnlyTopDirectory(logger, item);
            if (csproj.Count > 0)
            {
                projectsToAdd[i] = csproj[0];
            }
            else
            {
                projectsToAdd[i] = $"Folder {item} doesn't have csproj";
            }
        }
    }

    public static
    async Task<XmlDocument>
         AbsoluteToRelativeCsProj(string csprojPath, string basePath)
    {
        if (csprojPath == null)
        {
            return null!;
        }
        var xmlDocumentResult =
            await
        XmlDocumentsCache.Get(csprojPath);
        if (MayExcHelper.MayExc(xmlDocumentResult.Exc))
        {
            return null!;
        }
        var projectReferences =
            await
        VsProjectsFileHelper.GetProjectReferences(csprojPath, null!, UriKind.RelativeOrAbsolute);
        // When file contains e.g. git characters, GetProjectReferencesAsync
        if (projectReferences.Projects != null)
        {
            if (projectReferences.Projects.Count != projectReferences.Nodes.Count)
            {
                ThrowEx.DifferentCountInLists("nodes", projectReferences.Nodes.Count, "projs", projectReferences.Projects.Count);
            }
            for (int i = 0; i < projectReferences.Nodes.Count; i++)
            {
                var projectPath = projectReferences.Projects[i];
                var isAbsolutePath = FS.IsAbsolutePath(projectPath);
                if (DefaultPaths.IsIgnored(projectPath, basePath))
                {
                    continue;
                }
                if (isAbsolutePath)
                {
                    var relativePath = PathPolyfill.GetRelativePath(csprojPath, projectPath);
                    var node = projectReferences.Nodes[i];
                    XmlNode clonedNode = node.Clone();
                    XmlHelper.SetAttribute(clonedNode, "Include", relativePath);
                    node.ParentNode!.ReplaceChild(clonedNode, node);
                }
            }
            await XmlDocumentsCache.Set(csprojPath, xmlDocumentResult.Data);
        }
        return xmlDocumentResult.Data;
    }
    #region Projs - working with projects absolutely independently of .sln existence
    public static
    async Task
 SaveEmptyTestProject(string csprojPath)
    {
        await
        TF.WriteAllText(csprojPath, "<Project Sdk=\"Microsoft.NET.Sdk\"></Project>");
    }

    public static
    async Task
 SaveEmptyFullNetProject(string csprojPath, string projectName, string projectsBasePath)
    {
        var templateContent =
    await
 TF.ReadAllText(Path.Combine(projectsBasePath, @"SunamoDevCode\Templates\FullNet.xml"));
        string content = SHFormat.Format4(templateContent!, projectName);
        await
        TF.WriteAllText(csprojPath, content);
    }

    [Obsolete("Used MSBuildProject")]
    public static string RemoveSchema(string content)
    {
        return content;
    }
    #endregion
    #region Uap
    public static void ChangeUapFramework(ILogger logger, SolutionFolder solutionFolder, string targetFramework, string minFramework)
    {
        SolutionFolder.GetCsprojs(logger, solutionFolder);
        foreach (var item in solutionFolder.ProjectsGetCsprojs)
        {
            VsProjectsFileHelper.SetTargetFrameworksUap(item, targetFramework, minFramework);
        }
    }
    #endregion
}