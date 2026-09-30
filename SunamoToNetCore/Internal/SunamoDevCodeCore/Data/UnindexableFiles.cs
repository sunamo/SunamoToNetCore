namespace SunamoDevCodeCore.Data;

internal class UnindexableFiles
{
    public static UnindexableFiles Instance = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    public CollectionWithoutDuplicatesDC<string> UnindexablePathPartsFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    public CollectionWithoutDuplicatesDC<string> UnindexableFileNamesFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    public CollectionWithoutDuplicatesDC<string> UnindexableFileNamesExactlyFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    public CollectionWithoutDuplicatesDC<string> UnindexablePathEndsFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    public CollectionWithoutDuplicatesDC<string> UnindexablePathStartsFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();
}