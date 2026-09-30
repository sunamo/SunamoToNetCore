namespace SunamoToNetCore.Internal.SunamoDevCodeCore.Data;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public class CsprojsInSolution
{
    public List<string> CsprojFolderPaths { get; set; } = null!;
    public List<string> CsprojPaths { get; set; } = null!;

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dictionary = [];

        TestBothHaveSameLength();

        for (int i = 0; i < CsprojFolderPaths.Count; i++)
        {
            dictionary.Add(CsprojFolderPaths[i], CsprojPaths[i]);
        }

        return dictionary;
    }

    private void TestBothHaveSameLength()
    {
        if (CsprojPaths.Count != CsprojFolderPaths.Count)
        {
            ThrowEx.Custom("Different count in collection");
        }
    }

    public CsprojsInSolution Intersect(CsprojsInSolution other)
    {
        CsprojFolderPaths.AddRange(other.CsprojFolderPaths);
        CsprojPaths.AddRange(other.CsprojPaths);

        var distinctFolderPaths = CsprojFolderPaths.Distinct();
        if (distinctFolderPaths.Count() != CsprojFolderPaths.Count)
        {
            ThrowEx.Custom("Not all CsprojNames is unique");
        }

        return this;
    }
}