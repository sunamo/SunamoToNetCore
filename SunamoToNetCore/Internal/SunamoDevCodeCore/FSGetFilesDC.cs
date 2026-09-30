namespace SunamoDevCodeCore;

internal class FSGetFilesDC
{
    public static List<string> GetFilesDC(string slnFolder, string fileMask, SearchOption searchOption, GetFilesDCArgs arguments)
    {
        var resultFiles = new List<string>();
        var projectDirectories = Directory.GetDirectories(slnFolder);
        foreach (var projectDirectory in projectDirectories)
        {
            if (arguments.OnlyInSunamo)
            {
                var sunamoFolder = Path.Combine(projectDirectory, "_sunamo");
                if (Directory.Exists(sunamoFolder))
                {
                    resultFiles.AddRange(Directory.GetFiles(sunamoFolder, fileMask, searchOption));
                }
            }
            else
            {
                resultFiles.AddRange(Directory.GetFiles(projectDirectory, fileMask, searchOption));
            }
        }
        return resultFiles;
    }
}