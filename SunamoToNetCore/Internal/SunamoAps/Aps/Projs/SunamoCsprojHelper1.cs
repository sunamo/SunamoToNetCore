namespace SunamoAps.Aps.Projs;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class SunamoCsprojHelper
{
    public static Type type = typeof(SunamoCsprojHelper);
    public static
        async Task<object>
    AddMissingProjectsAlsoString(object text, bool addAlsoDepedencies = false)
    {
        throw new Exception("Používá se tu fubucsproj. přepsat do dotnet cmd");
    //        string ts2 = null;
    //        var sfs = new SolutionFolderSerialize();
    //        sfs.slnFullPath = ts2;
    //        SolutionFolder sln = null;
    //        var ts = text.GetType();
    //        if (ts == Types.StringType)
    //        {
    //            ts2 = text.ToString();
    //            if (ts2.EndsWith(AllExtensions.SlnExtension))
    //            {
    //                ts2 = FS.GetDirectoryName(ts2);
    //            }
    //            sln = FoldersWithSolutions.CreateSolutionFolder(ts2, SellingUCAps.toSelling);
    //        }
    //        else if (ts == SolutionFolder.Type)
    //        {
    //            sln = (SolutionFolder)text;
    //            ts2 = ApsHelper.Instance.MainSln(sln);
    //        }
    //        else if (ts == SolutionFolderSerialize.Type)
    //        {
    //            sln = new SolutionFolder((SolutionFolderSerialize)text);
    //            ts2 = ApsHelper.Instance.MainSln(sln);
    //        }
    //        else
    //        {
    //            ThrowEx.NotImplementedCase(ts);
    //        }
    //        FubuCsprojFile.Solution solution =
    //    await
    //#endif
    // FubuCsprojFile.Solution.LoadFrom(text.ToString());
    //        sln.projectsInSolution = SolutionsIndexerHelper.ProjectsInSolution(true, sln.FullPathFolder);
    //        var pr = sln.projectsInSolution;
    //        CollectionWithoutDuplicates<string> projectsOnWhichDepend = new CollectionWithoutDuplicates<string>();
    //        foreach (var prr in pr)
    //        {
    //            var path = Path.Combine(sln.FullPathFolder, prr);
    //            var d2 = ApsHelper.Instance.GetCsprojsOnlyTopDirectory(path);
    //            foreach (var item2 in d2)
    //            {
    //                FubuCsprojFile.CsprojFile csp = new CsprojFile(item2);
    //                if (!csp.IsValidXml)
    //                {
    //                    continue;
    //                }
    //                if (addAlsoDepencies)
    //                {
    //                    await
    //#endif
    //                    SunamoCsprojHelper.BuildProjectsDependencyTree(projectsOnWhichDepend, item2);
    //                }
    //                //if (item2.Contains("SunamoCef"))
    //                //{
    //                //}
    //#endif
    //                solution.AddProject(csp);
    //            }
    //        }
    //        var allGlobalCsprojs =
    //            await
    //#endif
    //            FoldersWithSolutions.AllGlobalCsprojs();
    //        allGlobalCsprojs = FoldersWithSolutions.allCsprojGlobal;
    //        foreach (var item2 in projectsOnWhichDepend.c)
    //        {
    //            string item = item2;
    //            if (!
    // FS.ExistsFile(item))
    //            {
    //                var fnwoe = Path.GetFileNameWithoutExtension(item);
    //                if (allGlobalCsprojs.ContainsKey(fnwoe))
    //                {
    //                    item = allGlobalCsprojs[fnwoe].First();
    //                }
    //            }
    //            if (FS.ExistsFile(item))
    //            {
    //                //var f = solution.Projects.FirstOrDefault(d => d.ProjectName.Contains("Scz.import"));
    //                var csp = new CsprojFile(item);
    //                if (!csp.IsValidXml)
    //                {
    //                    continue;
    //                }
    //                solution.AddProject(csp);
    //            }
    //        }
    //        solution.Save();
    //        // just for mark as finised for PB after done item (void can't be passed as finished object)
    //        return solution;
    }
}