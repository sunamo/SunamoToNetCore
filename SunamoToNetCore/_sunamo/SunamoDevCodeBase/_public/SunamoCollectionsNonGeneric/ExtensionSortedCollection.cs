namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal class ExtensionSortedCollection
{
    public Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

    /// <summary>
    /// Initializes a new instance of ExtensionSortedCollection.
    /// </summary>
    public ExtensionSortedCollection(params string[] d)
    {
        d.ToList().ForEach(fileName => AddOnlyFileName(fileName));
    }

    /// <summary>
    /// Add only file name.
    /// </summary>
    public void AddOnlyFileName(string fileName)
    {
        string key = Path.GetExtension(fileName).ToLower();
        string value = Path.GetFileNameWithoutExtension(fileName).ToLower();
        if (dictionary.ContainsKey(key))
        {
            if (!dictionary[key].Contains(value))
            {
                dictionary[key].Add(value);
            }
        }
        else
        {
            var ad = new List<string>();
            ad.Add(value);
            dictionary.Add(key, ad);
        }
    }
}
