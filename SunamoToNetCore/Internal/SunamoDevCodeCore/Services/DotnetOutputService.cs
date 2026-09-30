namespace SunamoDevCodeCore.Services;

internal class DotnetOutputService
{
    public DotnetBuildOutputLine? GetPartsFromDotnetBuildLine(string line)
    {
        if (line.StartsWith("MSBuild version"))
        {
            // MSBuild version 17.8.3+195e7f5a3 for .NET
            return null;
        }
        if (line.Trim() == string.Empty)
        {
            return null;
        }
        var parts = SHSplit.SplitToParts(line, 4, ":")!;
        var locationPart = parts![1].Trim();
        string? path = null;
        int lineNumber = -1;
        int columnNumber = -1;
        if (locationPart.Contains("("))
        {
            var locationParts = SHSplit.Split(locationPart, "(");
            var coordinates = locationParts[locationParts.Count - 1];
            locationParts.RemoveAt(locationParts.Count - 1);
            var lineColumnParts = SHSplit.Split(coordinates, ",");
            lineNumber = int.Parse(lineColumnParts[0]);
            columnNumber = int.Parse(lineColumnParts[1].TrimEnd(')'));
            path = parts[0] + ":" + string.Join("(", locationParts);
        }
        else
        {
            path = parts[0] + ":" + locationPart;
        }
        var typeParts = SHSplit.Split(parts[2].Trim(), " ");
        string? type = null;
        string? errorCode = null;
        if (typeParts.Count > 1)
        {
            type = typeParts[0];
            errorCode = typeParts[1];
        }
        else
        {
            return null;
        }
        var message = parts[3].Trim();
        return new DotnetBuildOutputLine { Path = path, Line = lineNumber, Column = columnNumber, Type = type, ErrorCode = errorCode, Message = message };
    }
}