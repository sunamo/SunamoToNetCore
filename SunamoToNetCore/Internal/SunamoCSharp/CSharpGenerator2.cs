namespace SunamoCSharp;

internal partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    public void List(int tabCount, string genericType, string listName, List<string> list, CSharpGeneratorArgs? args = null)
    {
        if (args == null)
            args = new CSharpGeneratorArgs
            {
                AddHyphens = true
            };
        var cn = "List<" + genericType + ">";
        NewVariable(tabCount, AccessModifiers.Private, cn, listName, args);
        if (args.AddHyphens)
            for (var i = 0; i < list.Count; i++)
                list[i] = SH.WrapWithQm(list[i]);
        //list = CA.WrapWith(list, "\"");
        if (genericType == "string")
        {
            if (args.UseCA)
                AppendLine(tabCount, listName + " = new List<string>(@" + string.Join(",", list) + ");");
            else
                AppendLine(tabCount, listName + " = new List<string>(new string[] {" + string.Join(",", list) + "});");
        }
        else
        {
            if (args.UseCA)
                AppendLine(tabCount, listName + " = new List<" + genericType + ">(CA.ToEnumerable(" + string.Join(",", list) + "));");
            else
                AppendLine(tabCount, listName + " = new List<" + genericType + ">(new " + genericType + "[] }" + string.Join(",", list) + "});");
        }
    }

    public void This(int tabCount, string memberName)
    {
        Append(tabCount, "this." + memberName);
    }

    private void NewVariable(int tabCount, AccessModifiers accessModifier, string className, string name, CSharpGeneratorArgs args)
    {
        AddTab2(tabCount, "");
        WriteAccessModifiers(accessModifier);
        sb.AddItem(className);
        sb.AddItem(name);
        if (args.CreateInstance)
        {
            sb.EndLine(';');
            AppendLine();
            CreateInstance(className, name);
        }
        else
        {
            sb.AddItem("= null;");
        }

        sb.AppendLine();
    }

    private void CreateInstance(string className, string variableName)
    {
        sb.AddItem(variableName + " = new " + className + "();");
    }

    public void Enum(int tabCount, AccessModifiers accessModifier, string enumName, List<EnumItem> enumItems)
    {
        WriteAccessModifiers(accessModifier);
        AddTab(tabCount);
        sb.AddItem("enum " + enumName);
        StartBrace(tabCount);
        foreach (var item in enumItems)
        {
            XmlSummary(tabCount + 1, item.Comment);
            if (item.Attributes != null)
                foreach (var attribute in item.Attributes)
                    AppendAttribute(tabCount + 1, attribute.Key, attribute.Value);
            var hexValue = "";
            if (item.Hex != "")
                hexValue = "=" + item.Hex;
            AppendLine(tabCount + 1, item.Name + hexValue + ",");
        }

        EndBrace(tabCount);
    }

    private void XmlSummary(int tabCount, string summary)
    {
        if (!string.IsNullOrEmpty(summary))
        {
            AppendLine(tabCount, "/// <summary>");
            AppendLine(tabCount, "/// " + summary);
            AppendLine(tabCount, "/// </summary>");
        }
    }

    public void AssignValue(int tabCount, string objectName, string variableName, string value, CSharpGeneratorArgs args)
    {
        AddTab(tabCount);
        sb.AddItem(objectName + "." + variableName);
        sb.AddItem("=");
        if (args.AddHyphens)
            value = SH.WrapWith(value, "\"");
        sb.AddItem(value + ";");
        sb.AppendLine();
    }

    public void AddValuesViaAddRange(int tabCount, string objectName, string collectionName, string type, List<string> values, CSharpGeneratorArgs args)
    {
        var objectPrefix = "";
        if (objectName != null)
            objectPrefix = objectName + ".";
        if (args.AddHyphens)
            //values = CA.WrapWith(values, "\"");
            for (var i = 0; i < values.Count; i++)
                values[i] = SH.WrapWithBs(values[i]);
        AddTab(tabCount);
        sb.AddItem(objectPrefix + collectionName + ".AddRange(new " + type + "[] { " + string.Join(",", values) + "});");
    }

    public void AddValuesViaAddRange(int tabCount, string objectName, string collectionName, Type type, List<string> values, CSharpGeneratorArgs args)
    {
        AddValuesViaAddRange(tabCount, objectName, collectionName, type.FullName!, values, args);
        sb.AppendLine();
    }

    public void DictionaryNumberNumber<T, U>(int tabCount, string dictionaryName, Dictionary<T, U> dictionary, CSharpGeneratorArgs? args = null) where T : notnull where U : notnull
    {
        DictionaryFromDictionary(tabCount, dictionaryName, dictionary, args);
    }

    public void DictionaryStringString(int tabCount, string dictionaryName, Dictionary<string, string> dictionary, CSharpGeneratorArgs? args = null)
    {
        DictionaryFromDictionary(tabCount, dictionaryName, dictionary, args);
    }

    public void DictionaryStringListString(int tabCount, string dictionaryName, Dictionary<string, List<string>> dictionary, CSharpGeneratorArgs? args = null)
    {
        if (args == null)
            args = new CSharpGeneratorArgs();
        var className = "Dictionary<string, List<string>>";
        NewVariable(tabCount, AccessModifiers.Private, className, dictionaryName, args);
        foreach (var item in dictionary)
        {
            var list = CAChangeContent.ChangeContent0(null!, item.Value, SH.WrapWithQm);
            AppendLine(tabCount, dictionaryName + ".Add(\"" + item.Key + "\", new List<string>(" + string.Join(",", list) + "));");
        }
    }

    public void DictionaryFromRandomValue<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, Func<Value> randomValueGenerator, CSharpGeneratorArgs? args = null) where Key : notnull
    {
        if (args == null)
        {
            args = new CSharpGeneratorArgs();
            args.AddingValue = true;
        }

        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], randomValueGenerator.Invoke());
        DictionaryFromDictionary(tabCount, dictionaryName, dict, args);
    }

    public void DictionaryFromTwoList<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, List<Value> values, CSharpGeneratorArgs? args = null) where Key : notnull
    {
        ThrowEx.DifferentCountInListsTU("keys", keys, "values", values);
        if (args == null)
            args = new CSharpGeneratorArgs();
        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], values[i]);
        DictionaryFromDictionary(tabCount, dictionaryName, dict, args);
    }
}