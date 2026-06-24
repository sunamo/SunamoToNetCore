namespace SunamoToNetCore;

public partial class MoveToNet5 //: ProgramShared
{
    /// <summary>
    /// Gets the list of all FoldersWithSolutions instances from the static collection.
    /// </summary>
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

        //        ProgramShared.Output = output;
        //        ProgramShared.OutputOpen();
        //#elif !DEBUG
        //        ProgramShared.Output = output;
        //        ProgramShared.OutputOpen();
        //        //showTextResultWindow = new ShowTextResultWindow(output);
        //        //showTextResultWindow.ShowDialog();
        //#endif

        return output;
    }

    //Tuple<List<string>, List<string>> WebAndNonWebProjects()
    //{
    //    MoveToNet5 m = new MoveToNet5();
    //    return m.WebAndNonWebProjects();
    //}


}
