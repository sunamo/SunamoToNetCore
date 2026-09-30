namespace SunamoDevCodeCore;

internal class BackslashEncoding
{
    public static void RemoveWhichIsInString(string inputString, List<int> positionsList)
    {
        var fromToDetector = CSharpHelperSunamo.DetectFromToString(inputString);
        for (var index = 0; index < positionsList.Count; index++)
            if (fromToDetector.IsInRange(positionsList[index]))
                positionsList.RemoveAt(index);
    }
}