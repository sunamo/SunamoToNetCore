namespace SunamoDevCodeCore;

using System.Collections.Generic;
using System.Linq;

internal class GetSlns
{
    public static List<string> GetSolutions(ILogger logger, bool onlyCs = false)
    {
        var parameter = @"E:\vs\";
        FoldersWithSolutions.PairProjectFolderWithEnum(logger, parameter);
        FoldersWithSolutions d = new FoldersWithSolutions(logger, parameter, null!, false);
        d.Reload(logger, parameter, null!);

        List<SolutionFolder> solutionFolders = d.GetSolutions(RepositoryLocal.Vs17);
        if (onlyCs)
        {
            solutionFolders = solutionFolders.Where(d => d.TypeProjectFolder == ProjectsTypes.Cs).ToList();
        }

        return solutionFolders.Select(d => d.FullPathFolder).ToList();
    }
}