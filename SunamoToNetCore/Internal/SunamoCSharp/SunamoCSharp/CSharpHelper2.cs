namespace SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal static partial class CSharpHelper
{
    public static string GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs argument) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromTwoList<Key, Value>(tabCount, nameDictionary, keys, values, argument);
        return csg.ToString();
    }

    public static string GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromRandomValue<Key, Value>(tabCount, nameDictionary, keys, randomValue);
        return csg.ToString();
    }

    //public static string GetDictionary(string nameDictionary)
    //{
    //}
    public static string RemoveXmlDocCommentsExceptSummary(List<string> list, ref bool removedAnything)
    {
        removedAnything = false;
        const string argument = @"/// <returns></returns>";
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Trim() == argument)
            {
                removedAnything = true;
                list.RemoveAt(i);
            }
        }

        return string.Join(Environment.NewLine, list); //string.Join(Environment.NewLine, list);
    }

    public static string RemoveXmlDocComments(List<string> list)
    {
        ThrowEx.NotImplementedMethod();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i] = SHParts.RemoveAfterFirst(list[i], "///");
        }

        CA.TrimWhereIsOnlyWhitespace(list);
        var text = string.Join(Environment.NewLine, list);
        CA.DoubleOrMoreMultiLinesToSingle(ref text);
        return text;
    }

    public static string RemoveComments(string listOrString, bool line = true, bool block = true, bool keepLinesNumbers = false)
    {
        if (keepLinesNumbers)
        {
            return RemoveCommentsKeepLines(listOrString);
        }

        return SHJoin.JoinNL(RemoveComments(SHGetLines.GetLines(listOrString), line, block));
    }

    private static string RemoveCommentsKeepLines(string input)
    {
        string output = "";
        using (StringReader reader = new(input))
        {
            bool inSingleLineComment = false;
            bool inMultiLineComment = false;
            int currentChar;
            while ((currentChar = reader.Read()) != -1)
            {
                char current = (char)currentChar;
                char next = (char)reader.Peek();
                if (inSingleLineComment)
                {
                    if (current == '\n')
                    {
                        inSingleLineComment = false;
                        output += current;
                    }
                }
                else if (inMultiLineComment)
                {
                    if (current == '*' && next == '/')
                    {
                        inMultiLineComment = false;
                        reader.Read(); // Přečteme '/'
                    }
                    else if (current == '\n')
                    {
                        output += current; // Přidáme nový řádek do výstupu
                    }
                }
                else
                {
                    if (current == '/' && next == '/')
                    {
                        inSingleLineComment = true;
                        reader.Read(); // Přečteme '/'
                    }
                    else if (current == '/' && next == '*')
                    {
                        inMultiLineComment = true;
                        reader.Read(); // Přečteme '*'
                    }
                    else
                    {
                        output += current;
                    }
                }
            }
        }

        return output;
    }

    public static List<string> RemoveComments(List<string> listOrString, bool line = true, bool block = true, bool keepLinesNumbers = false)
    {
        if (keepLinesNumbers)
        {
            return SHGetLines.GetLines(RemoveCommentsKeepLines(SHJoin.JoinNL(listOrString)));
        }

        if (line)
        {
            listOrString = RemoveLineComments(listOrString);
        }

        if (block)
        {
            listOrString = SHGetLines.GetLines(RemoveBlockComments(string.Join(Environment.NewLine, listOrString)));
        }

        //var list = CastHelper.ToListString(listOrString);
        //jak jsem mohl být takový debil argument dát to tady - na co tady trimovat?
        //CA.Trim(listOrString);
        return listOrString;
    }

    // Direct edit
    public static List<string> RemoveLineComments(List<string> list)
    {
        //List<string> list = CastHelper.ToListString(listOrString);
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i] = SHParts.RemoveAfterFirst(list[i], "//");
        }

        CA.RemoveStringsEmpty2(list);
        return list;
    }

    const string blockComments = @"/\*(.*?)\*/";
    const string lineComments = @"//(.*?)\r?\n";
    const string strings = @"""((\\[^\n]|[^""\n])*)""";
    const string verbatimStrings = @"@(""[^""]*"")+";

    public static string RemoveBlockComments(string str)
    {
        str = Regex.Replace(str, blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings, me =>
        {
            if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                return me.Value.StartsWith("//") ? Environment.NewLine : "";
            // Keep the literal strings
            return me.Value;
        }, RegexOptions.Singleline);
        return str;
    }

    public static string GetDictionaryStringObject<Value>(int tabCount, List<string> keys, List<Value> values, string nameDictionary, CSharpGeneratorArgs argument)
    {
        int pocetTabu = 0;
        CSharpGenerator gen = new CSharpGenerator();
        gen.DictionaryFromTwoList<string, Value>(tabCount, nameDictionary, keys, values, argument);
        if (argument.CheckForNull)
        {
            gen.If(pocetTabu, nameDictionary + " " + "== null");
        }

        gen.GetDictionaryValuesFromDictionary<string, Value>(pocetTabu, nameDictionary, DictionaryHelper.GetDictionary<string, Value>(keys, values));
        if (argument.CheckForNull)
        {
            gen.EndBrace(pocetTabu);
        }

        string result = gen.ToString();
        return result;
    }

    public static string DefaultValueForTypeSqLite(string type)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "TEXT":
                return "\"";
            case "INTEGER":
                return int.MaxValue.ToString();
            case "REAL":
                return "0.0";
            case "DATETIME":
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat argument proto si ušetřím value kódu práci text MSSQL
                return "DateTime.MinValue";
            case "BLOB":
                // Podporovaný typ pouze value desktopových aplikacích, kde není lsožka sbf
                return "null";
            default:
                ThrowEx.NotImplementedCase(type);
                break;
        }

        throw new Exception("Nepodporovaný typ");
    }
}
