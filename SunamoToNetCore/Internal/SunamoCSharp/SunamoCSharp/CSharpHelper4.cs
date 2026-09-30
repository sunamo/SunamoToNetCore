namespace SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal static partial class CSharpHelper
{
    // CsKeywordsList je internal — veřejný vstup pro konzumenty balíčku
    /// <summary>
    /// Initializes the C# keyword lists (public entry point, because CsKeywordsList is internal).
    /// </summary>
    public static void InitKeywords() => CsKeywordsList.Init();

    public static object DefaultValueForTypeObject(string type)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "string":
                return "\"" + "\"";
            case "bool":
                return false;
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return -1;
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return 0;
            case "DateTime":
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat argument proto si ušetřím value kódu práci text MSSQL
                return "new(1900, 1, 1)";
            case "char":
                throw new Exception(type);
            case "byte" + "[]":
                // Podporovaný typ pouze value desktopových aplikacích, kde není lsožka sbf
                return null!;
        }

        throw new Exception("Nepodporovaný typ");
    }

    public static string WrapWithRegion(string text, string regionName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#region ");
        stringBuilder.AppendLine(regionName);
        stringBuilder.AppendLine(text);
        stringBuilder.AppendLine("#endregion");
        return stringBuilder.ToString();
    }

    public static bool IsKeyword(string con)
    {
        //CsKeywords.Init();
        if (CsKeywordsList.Modifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.AccessModifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Statement.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.MethodParameter.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Namespace.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Operator.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Access.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Literal.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Type.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Contextual.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Query.Contains(con))
        {
            return true;
        }

        return false;
    }

    public static string WrapWithQuoteList(Type tValue, IList valueS)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in valueS)
        {
            string value = item!.ToString()!;
            WrapWithQuote(tValue, ref value);
            stringBuilder.Append(value + ",");
        }

        return stringBuilder.ToString().TrimEnd(',');
    }

    public static void WrapWithQuote(Type tKey, ref string keyS)
    {
        if (tKey == Types.StringType)
        {
            keyS = SH.WrapWithQm(keyS);
        }
        else if (tKey == Types.CharType)
        {
            keyS = SH.WrapWith(keyS, "\'");
        }
        else
        {
        }
    }
}
