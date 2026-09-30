namespace SunamoDevCodeCore;

internal class RepoGetFileMascGeneratorData
{
    public bool Cs { get; set; }
    public bool Css { get; set; }
    public bool JsJsx { get; set; }
    public bool Json { get; set; }
    public bool Scss { get; set; }
    public bool TsTsx { get; set; }
    public bool Yaml { get; set; }

    public int NumbersOf(bool value)
    {
        var count = 0;
        if (TsTsx == value) count++;
        if (JsJsx == value) count++;
        if (Scss == value) count++;
        if (Json == value) count++;
        if (Yaml == value) count++;
        if (Css == value) count++;
        if (Cs == value) count++;
        return count;
    }

    public static RepoGetFileMascGeneratorData AllSourceCodes()
    {
        var argument = new RepoGetFileMascGeneratorData();
        argument.AllTo(true);
        return argument;
    }

    public void AllTo(bool value)
    {
        // do it with reflection
        TsTsx = value;
        JsJsx = value;
        Scss = value;
        Json = value;
        Yaml = value;
        Css = value;
        Cs = value;
    }

    public string Generate()
    {
        var stringBuilder = new StringBuilder();
        if (TsTsx) stringBuilder.Append("*.ts;*.tsx;");
        if (JsJsx) stringBuilder.Append("*.js;*.jsx;");
        if (Scss) stringBuilder.Append("*.scss;");
        if (Json) stringBuilder.Append("*.json;");
        if (Yaml) stringBuilder.Append("*.yaml;");
        if (Css) stringBuilder.Append("*.css;");
        if (Cs) stringBuilder.Append("*.cs;");

        return stringBuilder.ToString();
    }
}