namespace SunamoToNetCore.research;

public partial class MoveToNet5 //: ProgramShared
{
    public List<FoldersWithSolutions> Fwss => FoldersWithSolutions.Fwss;

    #region Helper methods

    #endregion

    private string ListOfProjectsWhichIsWebAndWhichIsNotWeb(ILogger logger)
    {
        var temp = WebAndNonWebProjects(logger);

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(temp.Item1, "Web projects");
        tog.List(temp.Item2, "Not web projects");
        var output = tog.ToString();

        //#elif !DEBUG
        //#endif

        return output;
    }

    //Tuple<List<string>, List<string>> WebAndNonWebProjects()


}