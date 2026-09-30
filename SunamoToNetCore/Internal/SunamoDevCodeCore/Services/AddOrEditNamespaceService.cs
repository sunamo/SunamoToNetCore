namespace SunamoDevCodeCore.Services;

internal class AddOrEditNamespaceService
{
    public async Task<string?> AddOrEditNamespaceForSingleFileAndSave(string pathToCsprojFolder, string projectName
        , string csPath, List<string>? linesFile = null, string? pathToSave = null)
    {
        linesFile ??= (await FileAsync.ReadAllLinesAsync(csPath)).ToList();
        if (CSharpHelper.IsEmptyCommentedOrOnlyWithNamespace(Path.GetFileNameWithoutExtension(csPath), linesFile, null!, [null!]))
        {
            return null;
        }

        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(csPath);
        if (csPath.EndsWith(".xaml.cs")) return null;
        if (csPath.Contains(@"\obj\")) return null;
        var filename = Path.GetFileNameWithoutExtension(csPath);
        if (filename == "GlobalSuppressions") return null;
        if (filename == "GlobalUsings") return null;
        if (filename == "Net48Polyfills") return null;
        if (pathToSave != null)
        {
            csPath = pathToSave;
        }
        //if (CSharpHelper.IsEmptyCommentedOrOnlyWithNamespace("", linesFile, null, new List<string>()))
        //{
        //    return null;
        //}
        var relPath = csPath.Replace(pathToCsprojFolder, string.Empty);
        var parts = relPath.Split('\\', StringSplitOptions.RemoveEmptyEntries).ToList();
        parts.RemoveAt(parts.Count - 1);
        // Strip leading path segment that equals the project name (handles projects with an inner subfolder
        // named the same as the project, which would otherwise produce duplicate namespace segments
        // like SunamoFileSystem.SunamoFileSystem.FS instead of SunamoFileSystem.FS)
        if (parts.Count > 0 && string.Equals(parts[0], projectName, StringComparison.OrdinalIgnoreCase))
        {
            parts.RemoveAt(0);
        }
        var linesFileOri = linesFile.ToList();
        linesFile = await RemoveFileScopedNamespaceWhenIsInSharpIf(linesFile);
        var fullNamespace = projectName + (parts.Count == 0 ? "" : ".") + string.Join(".", parts);
        linesFile = AddNamespaceIfIsMissingInCs(linesFile, fullNamespace);
        linesFile = RemoveIfContainsMoreNamespace(linesFile);
        if (!linesFileOri.SequenceEqual(linesFile))
        {
            await TFCsFormat
                .WriteAllLines(pathToSave == null ? csPath : pathToSave, linesFile);
        }
        return fullNamespace;
    }
    private List<string> RemoveIfContainsMoreNamespace(List<string> list)
    {
        // EN: Find all namespace lines and remove duplicates (keep only the first one)
        // CZ: Najdi všechny namespace řádky a odstraň duplikáty (zachovej pouze první)
        var nsLines = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].StartsWith("namespace ") && list[i].EndsWith(";"))
            {
                nsLines.Add(i);
            }
        }

        // EN: Remove from the end to avoid index shifting
        // CZ: Odstraňuj od konce aby se neposunuly indexy
        for (int i = nsLines.Count - 1; i > 0; i--)
        {
            list.RemoveAt(nsLines[i]);
        }
        return list;
    }
    // Pracovní metoda která se už volá na konkrétní soubor
    // Volána z AddNamespaceByInputFolderName
    private List<string> AddNamespaceIfIsMissingInCs(List<string> lines, string newNs)
    {
        // Tohle jsem tu dal, když jsem byl dement a pracoval jsem v konzoli na neex cestě. Divil jsem se jaktože to v programu jde. Nebylo to tedy debug vs release jak jsem si původně myslel! Opět jsem hledal problém jinde než byl!
        //if (item.EndsWith("XmlGenerator.cs"))
        //{
        //    Console.WriteLine("XmlGenerator: " + item);
        //}
        //throw new Exception("Nepouštět, dojebává mi to soubory tím že sice vloží na první řádek NS; ale nesmaže { }");
        // todo napsat testy na toto
        //var lastIndexOfUsing = -1;
        var isNsOuter = false;
        var namespaceLineIndex = -1;
        RemoveEmptyLinesService removeEmptyLinesService = new RemoveEmptyLinesService();
        for (var i = 0; i < lines.Count; i++)
        {
            // .Trim() tu nemůže být protože pak mi to ořezává celý soubor a musím to znovu formátovat
            lines[i] = lines[i];
            var list = lines[i];
            isNsOuter = list.StartsWith("namespace");
            if (isNsOuter)
            {
                //isNsOuter = isNs;
                namespaceLineIndex = i;
                break;
            }
            if (classCodeElements.Any(d => list.Contains(d)))
            {
                break;
            }
            // vůbec nevím k čemu jsem tu dal tuto konstrukci
            //if (list != "" && list.StartsWith("using") && list.StartsWith("global using") && isNsOuter)
            //{
            //    lastIndexOfUsing = i - 1;
            //    break;
            //}
        }
        // todo zde je problém. přidává mi to NS i když už je v #elif. stačí potom zakomentovat RemoveFileScopedNamespaceWhenIsInSharpIf
        if (namespaceLineIndex != -1 && lines[namespaceLineIndex].Trim() == "namespace")
        {
            // kontrola zda je pod #else správný NS
            var dx = lines.IndexOf("#else");
            if (lines[dx + 1] != newNs)
            {
                lines[dx + 1] = newNs;
            }
        }
        else
        {
            // pokud nějaký řádek začíná namespace
            if (!isNsOuter)
            {
                AddNamespaceOnBegin(newNs, lines);
            }
            else
            {
                // EN: Check if namespace is already correct at line 0 - skip processing to avoid corruption
                // CZ: Zkontroluj zda je namespace už správně na řádku 0 - skipni processing aby se nepokazil soubor
                var expectedNs = "namespace " + newNs + ";";
                if (namespaceLineIndex == 0 && lines[0].Trim() == expectedNs.Trim())
                {
                    // EN: Namespace is already correct, do nothing
                    // CZ: Namespace je již správně, nedělej nic
                    return lines;
                }

                if (namespaceLineIndex != 0)
                {
                    //lines.RemoveAt(namespaceLineIndex);
                    if (!lines[namespaceLineIndex].Contains(";"))
                    {
                        removeEmptyLinesService.RemoveEmptyLinesFromStartAndEnd(lines);
                        lines[0] = lines[0].TrimStart('{');
                        lines[lines.Count - 1] = lines[lines.Count - 1].TrimEnd('}');
                    }
                    //AddNamespaceOnBegin(newNs, lines);
                    #region Tohle jsem nemusel dělat, od toho tu mám už TFCsFormat
                    lines.RemoveAt(namespaceLineIndex);
                    lines.Insert(0, "");
                    lines.Insert(0, "namespace " + newNs + ";");
                    #endregion
                }
                else
                {
                    lines[0] = "namespace " + newNs + ";";
                }
            }
        }
        //// u jiných přidává prázdný řádek protože smazal }
        //var temp = SHJoin.JoinNL(lines, false, lines);
        //// Tohle jsem tu dal, když jsem byl dement a pracoval jsem v konzoli na neex cestě. Divil jsem se jaktože to v programu jde. Nebylo to tedy debug vs release jak jsem si původně myslel! Opět jsem hledal problém jinde než byl!
        ////if (item.EndsWith("XmlGenerator.cs"))
        ////{
        ////    if (temp != text)
        ////    {
        ////        Console.WriteLine("Content changed, writing...");
        ////    }
        ////    else
        ////    {
        ////        Console.WriteLine("Content NOT changed");
        ////    }
        ////}
        //if (temp != text)
        //{
        //    await TFCsFormat.WriteAllText(item, temp);
        //}
        //await TFCsFormat.WriteAllLines(item, lines);
        return lines;
    }
    public readonly List<string> classCodeElements = new List<string>() { "class ", "interface ", "delegate", "enum ", "struct " };
    // FUnguje to OK, prošel jsem si všechny soubory před commitem
    private async Task<List<string>> RemoveFileScopedNamespaceWhenIsInSharpIf(List<string> list)
    {
        //var list = (await TF.ReadAllLines(item)).ToList();
        List<int> dxNs = new List<int>();
        int dxElse = -1;
        for (int i = 0; i < list.Count; i++)
        {
            var line = list[i];
            // chyba byla tady že namespace bylo s mezerou. pak mi to nevrátilo tu v #if
            if (line.StartsWith("namespace"))
            {
                dxNs.Add(i);
            }
            if (line == "#else")
            {
                dxElse = i + 1;
            }
            if (dxElse != -1 && dxNs.Any())
            {
                break;
            }
            if (classCodeElements.Any(d => line.Contains(d)))
            {
                break;
            }
        }
        if (dxElse != -1 && dxNs.Count == 1)
        {
            // tady nevím jestli to je správně. mám jen jeden řádek začínající namespace ale chci ho odstranit
            // proto to zakomentuji
            //list.RemoveAt(dxNs.First());
            //await TFCsFormat.WriteAllLines(item, list);
        }
        else if (dxNs.Count > 1)
        {
            List<string> lines = new List<string>(dxNs.Count);
            foreach (var item2 in dxNs)
            {
                lines.Add(list[item2]);
            }
            // seřadím od nejmenší k největší
            var ordered = lines.OrderBy(d => d.Length).Skip(1);
            foreach (var item3 in ordered)
            {
                list.Remove(item3);
            }
            // Řádky jen odstraňuje, není nutné formátovat
            // Zde se zapíšou jen změny - je to v else if
            //await TFCsFormat.WriteAllLines(item, list);
        }
        return list;
    }
    // Přidá nový file scoped namespace na začátek souboru
    private void AddNamespaceOnBegin(string newNs, List<string> lines)
    {
        if (lines.Count > 0)
        {
            lines.Insert(0, "namespace " + newNs + ";");
            if (lines[1].Trim() != string.Empty) lines.Insert(0, "");
        }
    }
}