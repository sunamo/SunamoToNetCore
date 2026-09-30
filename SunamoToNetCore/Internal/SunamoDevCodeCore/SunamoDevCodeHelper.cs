namespace SunamoDevCodeCore;

internal class SunamoDevCodeHelper
{
    public static bool TryDeleteDirectory(ILogger logger, string directoryPath)
    {
        if (!Directory.Exists(directoryPath)) return true;

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception)
        {
            // EN: It's try so don't know what this is doing here
            // CZ: Je to try takže nevím co tu dělá tohle
        }

        var files = FSGetFiles.GetFiles(logger, directoryPath, "*", SearchOption.AllDirectories);
        foreach (var filePath in files) File.SetAttributes(filePath, FileAttributes.Normal);

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception)
        {
        }

        return false;
    }

    public static void CopySolution(string slnFolder, string folderTo, Action<string> archive)
    {
        var list = Directory.GetFiles(slnFolder, "*", SearchOption.AllDirectories).ToList();
        RemoveTemporaryFilesVS(list);
        RemoveGitFiles(list);

        var sourceBasePath = Path.GetDirectoryName(slnFolder)!;
        FS.WithEndSlash(ref sourceBasePath);
        FS.WithEndSlash(ref folderTo);

        var slnFolderTo = Path.Combine(folderTo, Path.GetFileName(slnFolder));
        FS.TryDeleteDirectory(slnFolderTo);

        foreach (var item in list)
        {
            var newPath = item.Replace(sourceBasePath, folderTo);
            FS.CreateUpfoldersPsysicallyUnlessThere(newPath);
            //FS.CopyFile(item, newPath, FileMoveCollisionOptionDC.DontManipulate);
        }

        archive(slnFolderTo);
        //ThisApp.Info("Archive was created successfully, is important create archive because first open with VS because will create folders package,obj,bin");
    }

    public static void RemoveGitFiles(List<string> files, bool isIncludingGitFiles = true, bool isIncludingDownloadedFolders = false,
        bool isIncludingFoldersToDelete = false)
    {
        string? wrapped = null;

        if (!isIncludingGitFiles)
        {
            wrapped = SH.WrapWith(VisualStudioTempFse.GitFolderName, "\"");
            files.RemoveAll(d => d.Contains(wrapped));
        }

        if (!isIncludingDownloadedFolders)
            foreach (var item in VisualStudioTempFse.FoldersInSolutionDownloaded)
            {
                wrapped = SH.WrapWithBs(item);
                files.RemoveAll(d => d.Contains(wrapped));
            }

        if (!isIncludingFoldersToDelete)
            foreach (var item in VisualStudioTempFse.FoldersInSolutionToDelete)
            {
                wrapped = SH.WrapWithBs(item);
                files.RemoveAll(d => d.Contains(wrapped));
            }
    }

    public static void RemoveTemporaryFilesVS(List<string> files)
    {
        var list = VisualStudioTempFseWrapped.FoldersInSolutionToDelete;

        // todo list je zde List<string>, chce jen string, později to analyzovat

        //As foldersInProjectToDelete dont have contains WildCard, set false
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersInProjectToDelete;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersAnywhereToDelete;
        CA.RemoveWhichContainsList(files, list, false);

        list = VisualStudioTempFseWrapped.FoldersInSolutionDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersInProjectDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersAnywhereDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
    }

    private static bool IsNameOfHtmlAttrValue(string between)
    {
        return AllHtmlAttrsValues.list.Contains(between.Trim());
    }

    private static bool IsNameOfHtmlAttr(string between)
    {
        return AllHtmlAttrs.list!.Contains(between.Trim());
    }

    public static bool IsNameOfHtmlTag(string between, bool add)
    {
        string? element = null;
        var startWithTag = CA.StartWith(AllHtmlTags.list!, between, out element);
        startWithTag = element;
        if (startWithTag != null)
        {
            if (startWithTag == between)
            {
                add = true;
            }
            else
            {
                var remain = between.Substring(startWithTag.Length);
                add = int.TryParse(remain, out var _);
            }
        }

        return add;
    }

    // A1 normal, not lower
    public static bool IsNameOfControl(string between)
    {
        var add = false;
        add = IsNameOfHtmlTag(between, add);
        if (!add) add = IsNameOfHtmlAttr(between);

        if (!add) add = IsNameOfHtmlAttrValue(between);

        if (!add)
        {
            var firstInt = -1;
            var i = 0;
            foreach (var item in between)
            {
                if (char.IsLower(item))
                {
                    if (firstInt != -1)
                    {
                        add = false;
                        break;
                    }
                }
                else if (char.IsNumber(item))
                {
                    if (firstInt == -1) firstInt = i;
                }
                else
                {
                    add = false;
                    break;
                }

                i++;
            }

            var prefix = between;
            if (firstInt != -1) prefix = between.Substring(0, firstInt);

            add = SystemWindowsControls.IsShortcutOfControl(prefix);
        }

        return add;
    }
}