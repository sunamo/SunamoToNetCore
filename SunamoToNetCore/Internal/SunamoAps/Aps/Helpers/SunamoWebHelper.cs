namespace SunamoAps.Aps.Helpers;

internal class SunamoWebHelper
{
    public static async Task<List<string>> ListOfSunamoWebProjects(ILogger logger, GetFileSettings getFileSettings)
    {
        var csprojs = new List<string>();
        foreach (var item in ApsMainWindow.Instance.Fwss)
        {
            foreach (var sln in item.GetSolutions(RepositoryLocal.Vs17))
            {
                if (await ApsHelper.Instance.IsWebProject(logger, sln, getFileSettings))
                {
                    SolutionFolder.GetCsprojs(logger, sln);
                    csprojs.AddRange(sln.ProjectsGetCsprojs);
                }
            }
        }
        return csprojs;
    }
}