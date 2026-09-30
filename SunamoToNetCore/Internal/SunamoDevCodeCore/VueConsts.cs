namespace SunamoDevCodeCore;

internal class VueConsts
{
    public const string Cl = "class ";
    public const string Constructor = "constructor";
    public const string Import = "import";
    public const string Export = "export";
    public const string MethodsRcub = "methods:{";
    public const string PropsRcub = "props:{";

    // name, inject, mixins, extends, watch
    // https://github.com/pablohpsilva/vuejs-component-style-guide
    public static List<string> SStartWith = new List<string>(["name:", "inject:", "mixins:", "extends:", "watch:"]);

    public static List<string> DontAddRcub = new List<string>([MethodsRcub, PropsRcub]);

    //List<string> sContains = new List<string>("data():", "methods:{");
    public static List<string> ContainsList = new List<string>([Cl]);

    public static List<string> StartWithList = new List<string>([Constructor, Import, "//", Export]);
    public static List<char> Equal = new List<char>(['=']);
}