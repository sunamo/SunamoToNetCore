namespace SunamoDevCodeCore;

internal class FiltersNotTranslateAble
{
    public static FiltersNotTranslateAble Instance = new();

    // List of substrings - if a path contains any of these, it should not be translated.
    public readonly List<string> Contains;

    // Is good include the most files as is possible due to performamce
    public readonly List<string> Ending;

    public string AssemblyInfo = "AssemblyInfo.cs";
    public string Attributes = "Attributes";
    public string Consts = "Consts";
    public string Credentials = "Credentials";
    public string EnigmaData = "EnigmaData.cs";
    public string Enums = "Enums";
    public string Interfaces = "Interfaces";
    public string Layer = "Layer.cs";
    public string NotTranslateAbleCs = "NotTranslateAble.cs";
    public string NotTranslateAblePp = "NotTranslateAble";
    public string Standard = @"\standard\";

    // in XLF is not available sess coz is in sunamo
    public string SunamoXlf = "sunamo\\Xlf";

    // All which is WithoutDep cant have Xlf
    // If yes, I couldn't have Xlf.web and Xlf
    public string WithoutDep = "WithoutDep";

    private FiltersNotTranslateAble()
    {
        Ending = new List<string>([AssemblyInfo, Layer, NotTranslateAbleCs]);
        Contains = new List<string>([SunamoXlf, WithoutDep, Credentials, Interfaces, Enums, NotTranslateAblePp, Consts,
            Standard]);
    }
}