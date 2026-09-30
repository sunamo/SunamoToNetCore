namespace SunamoDevCodeCore;

internal class GenerateJson
{
    public static Dictionary<string, string> WithSnakeConventionDict(List<string> list)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var item in list) dictionary.Add(ConvertSnakeConvention.ToConvention(item), item);
        return dictionary;
    }

    public static string WithSnakeConvention(List<string> list)
    {
        var dictionary = WithSnakeConventionDict(list);
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("{");
        foreach (var item in dictionary) stringBuilder.AppendLine($"{SH.WrapWithQm(item.Key) + ": " + SH.WrapWithQm(item.Value)},");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}