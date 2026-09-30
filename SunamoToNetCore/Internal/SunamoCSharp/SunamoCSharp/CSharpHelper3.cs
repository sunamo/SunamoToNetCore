namespace SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal static partial class CSharpHelper
{
    public static string GetCtorInner(int tabCount, IList values)
    {
        const string assignVariable = "this.{0} = {0};";
        CSharpGenerator csg = new CSharpGenerator();
        foreach (var item in values)
        {
            csg.AppendLine(tabCount, string.Format(assignVariable, item));
        }

        return csg.ToString().Trim();
    }

    public static string GetArray(List<string> input, string arrayName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", arrayName, input);
        return generator.ToString();
    }

    public static string GetList(List<string> input, string listName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", listName, input);
        return generator.ToString();
    }

    public static List<string> RemoveRegions(List<string> lines)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("#" + "region" + " ") || line.StartsWith("#endregion"))
            {
                lines.RemoveAt(i);
            }
        }

        return lines;
    }

    public static
        async Task
    ReplaceForConsts(string pathXlfKeys)
    {
        var count = SHGetLines.GetLines(
            await
        FileAsync.ReadAllTextAsync(pathXlfKeys)).ToList();
        for (int i = 0; i < count.Count; i++)
        {
            var argument = count[i];
            if (argument.Contains(CSharpParser.StaticReadonlyModifier))
            {
                if (!argument.Contains("const") && !argument.Contains("class"))
                {
                    argument = SHReplace.ReplaceOnce(argument, "static ", string.Empty);
                    argument = SHReplace.ReplaceOnce(argument, "readonly ", string.Empty);
                    count[i] = SHReplace.ReplaceOnce(argument, CSharpParser.StaticReadonlyModifier, CSharpParser.PublicModifier + "const ");
                }
            }
        }

        await FileAsync.WriteAllLinesAsync(pathXlfKeys, count);
    }

    public static string GetConsts(List<string> list, bool? toCamelConvention)
    {
        return GetConsts(null!, list, toCamelConvention);
    }

    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower)
    {
        return GetConsts(names, list, toCamelConventionFirstCharLower, Types.StringType);
    }

    // A1 can be null
    // A3 null = not use Pascal convention
    // GenerateConstants - const without value
    // GetConsts - static readonly with value
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower, Type type)
    {
        bool addHyphensToValue = true;
        if (type != Types.StringType)
        {
            addHyphensToValue = false;
        }

        if (names != null)
        {
            ThrowEx.DifferentCountInLists("names", names, "list", list);
        }

        CSharpGenerator csg = new CSharpGenerator();
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            string name = item;
            if (names != null)
            {
                name = names[i];
            }

            if (toCamelConventionFirstCharLower.HasValue)
            {
                if (toCamelConventionFirstCharLower.Value)
                {
                    name = ConvertPascalConvention.ToConvention(name);
                }
                else
                {
                    name = ConvertPascalConvention.ToConvention(name);
                }
            }

            csg.Field(0, AccessModifiers.Public, true, VariableModifiers.ReadOnly, ConvertTypeShortcutFullName.ToShortcut(type.FullName!, false), name, addHyphensToValue, item);
        }

        var result = csg.ToString();
        return result;
    }

    // GenerateConstants - const without value
    // GetConsts - static readonly with value
    public static string GenerateConstants(int tabCount, Func<string, string> changeInput, List<string> input)
    {
        CSharpGenerator csg = new CSharpGenerator();
        foreach (var item in input)
        {
            string name = item;
            if (changeInput != null)
            {
                name = changeInput(item);
            }

            csg.Field(tabCount, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", name, true, item);
        }

        return csg.ToString();
    }

    public static List<string> TrimEnd(List<string> sf, params char[] toTrim)
    {
        for (int i = 0; i < sf.Count; i++)
        {
            sf[i] = sf[i].TrimEnd(toTrim);
        }

        return sf;
    }

    // value - data type
    // key - name
    public static Dictionary<string, string> ParseFields(List<string> lines)
    {
        CA.RemoveStringsEmpty2(lines);
        CAChangeContent.ChangeContent0(null!, lines, e => SHParts.RemoveAfterFirst(e, '='));
        CA.TrimEnd(lines, ';');
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (var item in lines)
        {
            var parameter = SHSplit.SplitByWhiteSpaces(item);
            var count = parameter.Count;
            StringBuilder sb2 = new StringBuilder(parameter[count - 1]);
            sb2[0] = char.ToLower(sb2[0]);
            result.Add(sb2.ToString(), parameter[count - 2]);
        }

        return result;
    }

    // 0 - item.Value
    // 1 - CSharpHelperSunamo.DefaultValueForType(item.Value)
    // 2 - item.Key
    // 3 - SH.FirstCharUpper(item.Key)
    const string tProperty = @"{0} {2} = {1};
    public {0} {3} { get { return {2}; } set { {2} = value; OnPropertyChanged(" + "\"{3}\"); } }" + @"
";
    public static string GenerateProperties(GeneratePropertiesArgs argument)
    {
        var lines = argument.Input;
        Dictionary<string, string>? data = null;
        if (argument.AllStrings)
        {
            data = new Dictionary<string, string>(lines.Count);
            foreach (var item in lines)
            {
                var parameter = ConvertPascalConvention.ToConvention(item);
                StringBuilder sb2 = new StringBuilder(parameter);
                sb2[0] = char.ToUpper(sb2[0]);
                //p = SH.FirstCharLower(parameter);
                //DebugLogger.Instance.WriteLine(parameter);
                data.Add(sb2.ToString(), "string");
            }
        }
        else
        {
            data = ParseFields(lines);
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in data)
        {
            StringBuilder sb2 = new StringBuilder(item.Key);
            sb2[0] = char.ToUpper(sb2[0]);
            stringBuilder.AppendLine( /*SHFormat.Format3*/string.Format((string)tProperty, item.Value, CSharpHelperSunamo.DefaultValueForType(item.Value, ConvertTypeShortcutFullName.ToShortcut), item.Key, sb2.ToString()));
        }

        return stringBuilder.ToString();
    }

    public static string? LineWithClass(List<string> lines, bool mustDerive)
    {
        foreach (var item in lines)
        {
            if (item.Contains("class "))
            {
                if (mustDerive)
                {
                    if (item.Contains(":"))
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    static Type type = typeof(CSharpHelper);
}
