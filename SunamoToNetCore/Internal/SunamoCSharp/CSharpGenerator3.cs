namespace SunamoCSharp;

internal partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    public void DictionaryFromDictionary<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, Value> dictionary, CSharpGeneratorArgs? args = null) where Key : notnull
    {
        if (args == null)
            args = new CSharpGeneratorArgs();
        string? valueType = null;
        if (dictionary.Count > 0)
            valueType = ConvertTypeShortcutFullName.ToShortcut(DictionaryHelper.GetFirstItemValue(dictionary)!.GetType().FullName!);
        var className = "Dictionary<string, " + valueType + ">"; //
        args.CreateInstance = false;
        NewVariable(tabCount, AccessModifiers.Private, className, dictionaryName, args);
        AppendLine();
        CreateInstance(className, dictionaryName);
        if (args.AddingValue)
            GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    public void DictionaryFromDictionaryInnerList<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, Value> dictionary, CSharpGeneratorArgs args) where Key : notnull
    {
        string? valueType = null;
        if (dictionary.Count > 0)
            valueType = ConvertTypeShortcutFullName.ToShortcut(DictionaryHelper.GetFirstItemValue(dictionary)!.GetType().FullName!);
        var className = "Dictionary<string, List<" + valueType + ">>"; //
        NewVariable(tabCount, AccessModifiers.Private, className, dictionaryName, args);
        AppendLine();
        CreateInstance(className, dictionaryName);
        if (args.AddingValue)
            GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    public void GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, Func<Value> randomValueGenerator) where Key : notnull
    {
        var dictionary = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dictionary.Add(keys[i], randomValueGenerator.Invoke());
        GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    public void GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, List<Value> values, CSharpGeneratorArgs args) where Key : notnull
    {
        var shouldSplitKeys = false;
        string? delimiter = null;
        if (args.SplitKeyWith != null)
        {
            if (typeof(Key) == Types.StringType)
            {
                shouldSplitKeys = true;
                delimiter = args.SplitKeyWith;
            }
            else
            {
                throw new Exception("App want to split key but key is not string");
            }
        }

        ThrowEx.DifferentCountInListsTU("keys", keys, "values", values);
        var dictionary = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            if (shouldSplitKeys)
            {
                var splitKeys = keys[i]!.ToString()!.Split(new[] { delimiter! }, StringSplitOptions.RemoveEmptyEntries); //SHSplit.Split(, delimiter);
                foreach (var item in splitKeys)
                    dictionary.Add((Key)(dynamic)item, values[i]);
            }
            else
            {
                dictionary.Add(keys[i], values[i]);
            }

        GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    public void GetDictionaryValuesFromDictionaryInnerList<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, List<Value>> dictionary, CSharpGeneratorArgs args) where Key : notnull
    {
        Type keyType, valueType;
        Key firstKey;
        GetTKeyAndTValue(dictionary, out keyType, out valueType, out firstKey);
        IList valueList;
        string keyText = null!;
        keyText = firstKey!.ToString()!;
        if (args.AlsoField)
            Field(tabCount, AccessModifiers.Public, true, VariableModifiers.None, string.Format("Dictionary<{0}, List<{1}>>", keyType.Name, valueType.Name), dictionaryName, true);
        string? valuesCSharp = null;
        foreach (var item in dictionary)
        {
            keyText = item.Key!.ToString()!;
            valueList = item.Value;
            CSharpHelper.WrapWithQuote(keyType, ref keyText);
            var formattedValues = CSharpHelper.WrapWithQuoteList(valueType, valueList);
            if (args.UseCA)
                valuesCSharp = "[" + formattedValues + "]";
            else
                valuesCSharp = "new List<" + valueType.Name + ">(new ;" + valueType.Name + "[] {" + formattedValues + "})";
            AppendLine(tabCount, dictionaryName + ".Add(" + keyText + ", " + valuesCSharp + ");");
        }
    }

    public static void GetTKeyAndTValue<Key, Value>(Dictionary<Key, List<Value>> dictionary, out Type keyType, out Type valueType, out Key firstKey) where Key : notnull
    {
        firstKey = default!;
        var firstValue = default(Value);
        foreach (var item in dictionary)
        {
            firstKey = item.Key;
            firstValue = (Value)(object)item.Value.FirstOrDefault()!;
            break;
        }

        keyType = firstKey!.GetType();
        valueType = firstValue!.GetType();
    }

    public void GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, Value> dictionary) where Key : notnull
    {
        var firstKey = default(Key);
        var firstValue = default(Value);
        foreach (var item in dictionary)
        {
            firstKey = item.Key;
            firstValue = item.Value;
            break;
        }

        var keyType = firstKey!.GetType();
        var valueType = firstValue!.GetType();
        string valueText, keyText = null!;
        valueText = firstValue!.ToString()!;
        keyText = firstKey!.ToString()!;
        foreach (var item in dictionary)
        {
            keyText = item.Key!.ToString()!;
            valueText = item.Value!.ToString()!;
            CSharpHelper.WrapWithQuote(keyType, ref keyText);
            CSharpHelper.WrapWithQuote(valueType, ref valueText);
            AppendLine(tabCount, dictionaryName + ".Add(" + keyText + ", " + valueText + ");");
        }
    }

    public void DictionaryStringObject<Value>(int tabCount, string dictionaryName, Dictionary<string, Value> dictionary, CSharpGeneratorArgs args)
    {
        DictionaryFromDictionary(tabCount, dictionaryName, dictionary, args);
    }
}