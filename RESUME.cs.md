---
schema_version: 2
type: library
file_count: 135
delete_recommendation_percent: 5
generated_date: 2026-09-30
generated_time: 16:42:18
---

## Description

Nástroje pro migraci starších .NET Framework projektů na moderní SDK-style .NET (Core+), vyčleněné z monolitu `SunamoDevCode`. Umí rozpoznat, které projekty už jsou SDK-style, ověřit cílový moniker a obsahuje experimentální kód (`research/`) pro hromadnou úpravu web i non-web projektů.
Balíček je self-contained: kód dříve referencovaných balíčků (DevCodeBase, SolutionsIndexer, Aps, DevCodeCore, CSharp, MsBuild) je zkopírován do `_sunamo\` a zeštíhlen jen na skutečně používané členy (internal) a jiné Sunamo balíčky nereferencuje.
