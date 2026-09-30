namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoResult;

internal class MayExcHelper
{
    /// <summary>
    /// May exc.
    /// </summary>
    internal static bool MayExc(string exception)
    {
        if (exception is not null)
        {
            Console.WriteLine(exception);
            //ThisApp.Error( result.exception);
            return true;
        }

        return false;
    }
}