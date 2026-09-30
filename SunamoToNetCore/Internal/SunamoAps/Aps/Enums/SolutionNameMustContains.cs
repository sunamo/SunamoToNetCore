namespace SunamoAps.Aps.Enums;

internal enum SolutionNameMustContains
{
    Text,
    // Není možné použít, v první fázi se zjistí projekty které by mohli být potenciálně duplicitní. V druhé
    SameNameWithDiacritic,
    // FS.GetNameWithoutSeries
    Serie,
    // SH.EndsWithNumber, SHTrim.TrimTrailingNumbersAtEnd
    NumberAtEnd,
    // Berou se všechny projekty
    None
}