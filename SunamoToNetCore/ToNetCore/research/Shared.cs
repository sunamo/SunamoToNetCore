namespace SunamoToNetCore;

public class Shared
{
    public static Type Type = typeof(Shared);

    public static Action<string, bool> ExtractArchive = null!;

    static
    async Task<string>
 ReplaceTargetPlatform(string replacementValue, string propertyGroupTag, string startTag, string endTag, List<string> csprojFiles, bool isThrowException = false)
    {
        StringBuilder? onlyStartTagFiles = null;
        StringBuilder? onlyEndTagFiles = null;
        StringBuilder? missingPropertyGroupFiles = null;

        if (!isThrowException)
        {
            onlyStartTagFiles = new StringBuilder();
            onlyEndTagFiles = new StringBuilder();
            missingPropertyGroupFiles = new StringBuilder();
        }

        foreach (var csprojPath in csprojFiles)
        {
            var fileContent =
    await
 TF.ReadAllText(csprojPath);
            var hasStartTag = fileContent!.Contains(startTag);
            var hasEndTag = fileContent!.Contains(endTag);

            if (hasStartTag && hasEndTag)
            {
                var currentValue = SH.GetTextBetween(fileContent, startTag, endTag, false);

                if (currentValue != replacementValue)
                {
                    fileContent = SHReplace.ReplaceOnce(fileContent, startTag + currentValue + endTag, startTag + replacementValue + endTag);
                    await TF.WriteAllText(csprojPath, fileContent);
                }
            }
            else if (hasStartTag && !hasEndTag)
            {
                if (isThrowException)
                {
                    ThrowEx.Custom("Have only starting tag: " + csprojPath);
                }
                else
                {
                    onlyStartTagFiles!.AppendLine(csprojPath);
                }
            }
            else if (hasEndTag && !hasStartTag)
            {
                if (isThrowException)
                {
                    ThrowEx.Custom("Have only ending tag: " + csprojPath);
                }
                else
                {
                    onlyEndTagFiles!.AppendLine(csprojPath);
                }
            }
            else
            {
                var propertyGroupIndex = fileContent.IndexOf(propertyGroupTag);
                if (propertyGroupIndex == -1)
                {
                    if (isThrowException)
                    {
                        ThrowEx.Custom("Don't have PropertyGroup: " + csprojPath);
                    }
                    else
                    {
                        missingPropertyGroupFiles!.AppendLine(csprojPath);
                    }
                }
                else
                {
                    fileContent = fileContent.Insert(propertyGroupIndex + propertyGroupTag.Length, startTag + replacementValue + endTag);
                    await TF.WriteAllText(csprojPath, fileContent);
                }
            }
        }

        if (!isThrowException)
        {
            TextOutputGenerator outputGenerator = new();
            outputGenerator.ListSB(onlyStartTagFiles!, "onlyStart");
            outputGenerator.ListSB(onlyEndTagFiles!, "onlyEnd");
            outputGenerator.ListSB(missingPropertyGroupFiles!, "dontHavePropertyGroup");

            return outputGenerator.ToString();
        }
        return null!;

    }

    public static
    async Task<string?>
 PlaformTargetTo(ILogger logger, string replaceFor, string folderNonRec, bool throwEx = false)
    {
        if (ExtractArchive != null)
        {
            var zf = FS.Combine(folderNonRec, FS.GetFileName(folderNonRec) + AllExtensions.ZipExtension);
            ExtractArchive(zf, true);
        }

        var gf = FSGetFiles.GetFiles(logger, folderNonRec, "*.csproj", false);
        return
    await
 Shared.PlatformTargetTo(replaceFor, gf, throwEx);
    }

    // Využívá se v ChangeConvertNonWebPlatformTargetTo(), PlatformTargetTo a PlatformTargetToWeb()
    public static
    async Task<string>
 PlatformTargetTo(string replaceFor, List<string> tt, bool throwEx = false)
    {
        const string PropertyGroup = "<PropertyGroup>";
        const string start = "<PlatformTarget>";
        const string end = "</PlatformTarget>";

        StringBuilder f2 = new();

        if (replaceFor.StartsWith("!"))
        {
            replaceFor = replaceFor[1..];
            foreach (var item in tt)
            {
                var f =
    await
 TF.ReadAllText(item);
                f2.Clear();
                f2.Append(f!.Replace(start, string.Empty));
                var f2s = f2.ToString();
                if (f != f2s)
                {
                    await TF.WriteAllText(item, f2s);
                }
            }
        }
        else
        {
            return
    await
 Shared.ReplaceTargetPlatform(replaceFor, PropertyGroup, start, end, tt, throwEx);
        }

        return null!;
    }
}
