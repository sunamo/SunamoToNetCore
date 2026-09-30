namespace SunamoDevCodeCore.Data;

// EN: Loads all csproj in directory, can convert from path to name and vice versa
// CZ: Načte všechny csproj v dir, umí převádět z cesty na název a vice versa
internal static class UniqueCsprojs
{
    private static TwoWayDictionary<string, string> dictionary = new TwoWayDictionary<string, string>();

    public static void AddFromSlnFolder(CsprojsInSolution csprojsInSolution)
    {
        if (dictionary.FirstToSecond.Any())
        {
            return;
        }

        foreach (var csprojPath in csprojsInSolution.CsprojPaths)
        {
            dictionary.Add(Path.GetFileNameWithoutExtension(csprojPath), csprojPath);
        }
    }

    public static string ToName(string csprojPath)
    {

        return dictionary.SecondToFirst[csprojPath];
    }

    // EN: Converts to csproj path, not to folder path
    // CZ: Převede na csproj path, nikoliv na cestu ke složce
    public static string ToPath(string projectName)
    {

        return dictionary.FirstToSecond[projectName];
    }
}