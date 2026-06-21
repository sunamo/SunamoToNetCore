namespace SunamoDevCode.ToNetCore.research;

/// <summary>
/// Shared utilities for .NET Core migration.
/// </summary>
public class Shared
{
    /// <summary>
    /// Type of the Shared class for reflection purposes.
    /// </summary>
    public static Type Type = typeof(Shared);

    /// <summary>
    /// Action to extract archives during migration.
    /// </summary>
    public static Action<string, bool> ExtractArchive = null!;

    static
#if ASYNC
    async Task<string>
#else
    string
#endif
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
#if ASYNC
    await
#endif
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
            TextOutputGenerator outputGenerator = new TextOutputGenerator();
            outputGenerator.ListSB(onlyStartTagFiles!, "onlyStart");
            outputGenerator.ListSB(onlyEndTagFiles!, "onlyEnd");
            outputGenerator.ListSB(missingPropertyGroupFiles!, "dontHavePropertyGroup");

            return outputGenerator.ToString();
        }
        return null!;

    }

    /// <summary>
    /// Changes the PlatformTarget in all csproj files within the given folder.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="replaceFor">Target platform value to set.</param>
    /// <param name="folderNonRec">Folder to search for csproj files (non-recursive).</param>
    /// <param name="throwEx">Whether to throw exceptions on errors.</param>
    /// <returns>Report of files that had issues, or null.</returns>
    public static
#if ASYNC
    async Task<string?>
#else
    string?
#endif
 PlaformTargetTo(ILogger logger, string replaceFor, string folderNonRec, bool throwEx = false)
    {
        if (ExtractArchive != null)
        {
            var zf = FS.Combine(folderNonRec, FS.GetFileName(folderNonRec) + AllExtensions.ZipExtension);
            ExtractArchive(zf, true);
        }

        var gf = FSGetFiles.GetFiles(logger, folderNonRec, "*.csproj", false);
        return
#if ASYNC
    await
#endif
 Shared.PlatformTargetTo(replaceFor, gf, throwEx);
    }

    /// <summary>
    /// Vyu��v� se v ChangeConvertNonWebPlatformTargetTo(), PlatformTargetTo a PlatformTargetToWeb()
    ///
    /// </summary>
    /// <param name="replaceFor">Target platform value to set (prefix with ! to remove).</param>
    /// <param name="tt">List of csproj file paths to process.</param>
    /// <param name="throwEx">Whether to throw exceptions on errors.</param>
    /// <returns>Report of files that had issues, or null.</returns>
    public static
#if ASYNC
    async Task<string>
#else
    string
#endif
 PlatformTargetTo(string replaceFor, List<string> tt, bool throwEx = false)
    {
        const string PropertyGroup = "<PropertyGroup>";
        const string start = "<PlatformTarget>";
        const string end = "</PlatformTarget>";

        StringBuilder f2 = new StringBuilder();

        if (replaceFor.StartsWith("!"))
        {
            replaceFor = replaceFor.Substring(1);
            string start2 = start + replaceFor + end;
            foreach (var item in tt)
            {
                var f =
#if ASYNC
    await
#endif
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
#if ASYNC
    await
#endif
 Shared.ReplaceTargetPlatform(replaceFor, PropertyGroup, start, end, tt, throwEx);
        }

        return null!;
    }
}