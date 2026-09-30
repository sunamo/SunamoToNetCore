namespace SunamoDevCodeCore;

using FileMs = File;

internal class TFCsFormat
{
    private static readonly List<string> classCodeElements = new()
        { "class ", "interface ", "enum ", "struct ", "delegate " };

    private static List<string> OnlyToFirst(List<string> lines)
    {
        var toFirstCodeElement = new List<string>();

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (classCodeElements.Any(element => line.Contains(element)))
            {
                for (var previousIndex = i - 1; previousIndex >= 0; previousIndex--)
                    if (lines[previousIndex].StartsWith("//"))
                        i--;
                    else
                        break;

                toFirstCodeElement = lines.Take(i).ToList();

                for (var j = toFirstCodeElement.Count - 1; j >= 0; j--) lines.RemoveAt(0);

                break;
            }
        }

        return toFirstCodeElement;
    }

    public static void WriteAllTextSync(string filePath, string content)
    {
        WriteAllText(filePath, content).GetAwaiter().GetResult();
    }

    public static void WriteAllLinesSync(string filePath, IEnumerable<string> lines)
    {
        WriteAllLines(filePath, lines).GetAwaiter().GetResult();
    }

    public static async Task WriteAllText(string filePath, string content)
    {
        if (!filePath.EndsWith(".cs") || filePath.EndsWith("GlobalUsings.cs"))
        {
            await FileAsync.WriteAllTextAsync(filePath, content);
            return;
        }

        var lines = SHGetLines.GetLines(content);
        await WriteAllLines(filePath, lines);
    }

    public static async Task WriteAllLines(string filePath, IEnumerable<string> lines)
    {
        if (!filePath.EndsWith(".cs") || filePath.EndsWith("GlobalUsings.cs"))
        {
            await FileAsync.WriteAllLinesAsync(filePath, lines);
            return;
        }

        var mutableLines = lines.ToList();

        var toFirstCodeElement = OnlyToFirst(mutableLines);

        var usings = new List<string>();
        var ns = string.Empty;

        foreach (var item in toFirstCodeElement)
            if (item.StartsWith("using "))
                usings.Add(item);
            else if (item.StartsWith("namespace "))
                ns = item;

        ns = ns.TrimEnd(';') + ";";
        if (ns == string.Empty)
        {
            // todo doplnit ns
        }

        if (usings.Count != 0) usings.Add("");

        var wasBlockScopedNs = !ns.EndsWith(";");

        if (wasBlockScopedNs)
        {
            ThrowEx.Custom("Block scoped namespace is not allowed.");
        }

        usings.Insert(0, ns);
        usings.Insert(1, "");

        TrimWhiteSpaceRowFromEnd(mutableLines);

        if (wasBlockScopedNs)
        {

            if (mutableLines[mutableLines.Count - 1] == "}")
            {
                mutableLines.RemoveAt(mutableLines.Count - 1);
            }
            else
            {
                throw new Exception("The last line is not }");
            }
        }

        usings.AddRange(mutableLines);

        await FileAsync.WriteAllTextAsync(filePath, SHJoin.JoinNL(usings));
    }

    public static void TrimWhiteSpaceRowFromEnd(List<string> lines)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                break;
            }
            lines.RemoveAt(i);
        }
    }
}