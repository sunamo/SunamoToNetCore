namespace SunamoDevCodeCore;

internal class GlobalUsingsInstance
{
    private string path = null!;
    private ParseGlobalUsingsResult r = null!;

    public async Task Init(string path)
    {
        this.path = path;
        if (!File.Exists(path)) await FileAsync.WriteAllTextAsync(path, "");
        var list = await FileAsync.ReadAllLinesAsync(path);

        r = GlobalUsingsHelper.Parse(list.ToList());
    }

    public void AddNewGlobalUsing(string _using)
    {
        if (!r.GlobalUsings.Contains(_using)) r.GlobalUsings.Add(_using);
    }

    // EN: Remove all global usings that start with the specified prefix (case insensitive)
    // CZ: Odstraň všechny global usings které začínají zadaným prefixem (case insensitive)
    public void RemoveGlobalUsingsStartingWith(string prefix)
    {
        r.GlobalUsings = r.GlobalUsings.Where(ns => !ns.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task Save()
    {
        StringBuilder stringBuilder = new();

        foreach (var item in r.GlobalSymbols)
            stringBuilder.AppendLine(GlobalUsingsHelper.global + item.Key + " = " + item.Value + ";");

        foreach (var item in r.GlobalUsings) stringBuilder.AppendLine(GlobalUsingsHelper.globalUsing + item + ";");

        await FileAsync.WriteAllTextAsync(path, stringBuilder.ToString());
    }
}