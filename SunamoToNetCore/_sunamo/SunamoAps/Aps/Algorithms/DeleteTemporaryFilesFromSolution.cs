namespace SunamoToNetCore._sunamo.SunamoAps;

internal class DeleteTemporaryFilesFromSolution
{

    /// <summary>
    /// Clear solution.
    /// </summary>
    public static List<string> ClearSolution(ILogger logger, string solutionFolder, bool delete, string folderWithTemporaryMovedContentWithoutBackslash, bool keepPackageJson = false)
    {
        string folderWithProjectsFolders = FS.GetDirectoryName(solutionFolder);
        // Navigate up directory tree until we find the solutions folder
        while (!SolutionsIndexerHelper.IsTheSolutionsFolder(FS.GetFileName(folderWithProjectsFolders)))
        {
            folderWithProjectsFolders = FS.GetDirectoryName(folderWithProjectsFolders);
        }
        var foundFiles = new List<string>();
        foundFiles = FSGetFiles.GetFiles(logger, solutionFolder,
            true);
        foreach (var folderPattern in VisualStudioTempFse.FoldersInSolutionToDelete)
        {
            var trimmedPattern = SHTrim.Trim(folderPattern, "\\");
            DeleteFolderIn(logger, solutionFolder, trimmedPattern, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        foreach (var folderPattern in VisualStudioTempFse.FoldersInSolutionDownloaded)
        {
            var trimmedPattern = SHTrim.Trim(folderPattern, "\\");
            DeleteFolderIn(logger, solutionFolder, trimmedPattern, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        var filesInSolutionToDelete = VisualStudioTempFse.FilesInSolutionToDelete.dictionary;
        var filesInSolutionDownloaded = VisualStudioTempFse.FilesInSolutionDownloaded.dictionary;
        var filesInSolutionReal = FS.GetDictionaryByExtension(logger, solutionFolder, "*", SearchOption.TopDirectoryOnly);
        foreach (var extensionEntry in filesInSolutionReal)
        {
            string extension = extensionEntry.Key;
            if (filesInSolutionToDelete.ContainsKey(extension))
            {
                foreach (var file in filesInSolutionReal[extension])
                {
                    if (filesInSolutionDownloaded[extension].Any(pattern => SH.MatchWildcard(file, pattern)))
                    {
                        DeleteFilesIn(solutionFolder, file + extension, folderWithProjectsFolders, foundFiles,
                            delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
            if (filesInSolutionDownloaded.ContainsKey(extension))
            {
                foreach (var file in filesInSolutionReal[extension])
                {
                    if (filesInSolutionDownloaded[extension].Any(pattern => SH.MatchWildcard(file, pattern)))
                    {
                        DeleteFilesIn(solutionFolder, file + extension, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
        }
        if (!keepPackageJson)
        {
            foundFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, solutionFolder, AllExtensions.JsonExtension, true).ToList());
        }
        foundFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, solutionFolder, "", true).ToList());
        var projects = FSGetFolders.GetFolders(solutionFolder);
        foreach (string project in projects)
        {
            ClearProject(logger, delete, folderWithProjectsFolders, foundFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        }
        // TODO: Implement using VisualStudioTempFse
        FS.DeleteFoldersWhichNotContains(solutionFolder, "bin", CA.ToListString("node_modules"));
        return foundFiles;
    }

    /// <summary>
    /// Clear project.
    /// </summary>
    private static void ClearProject(ILogger logger, bool delete, string folderWithProjectsFolders, List<string> foundFiles, string project, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        DeleteTemporaryFiles(logger, folderWithProjectsFolders, project, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);

        foreach (var folderInProject in VisualStudioTempFse.FoldersInProjectToDelete)
        {
            DeleteFolderIn(logger, project, folderInProject, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
    }

    /// <summary>
    /// Delete temporary files.
    /// </summary>
    private static void DeleteTemporaryFiles(ILogger logger, string folderWithProjectsFolders, string project, List<string> foundFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        var filesInProjectToDelete = VisualStudioTempFse.FilesInProjectToDelete.dictionary;
        var filesInProjectReal = FS.GetDictionaryByExtension(logger, project, "*", SearchOption.TopDirectoryOnly);

        foreach (var extensionEntry in filesInProjectReal)
        {
            string extension = extensionEntry.Key;
            if (filesInProjectToDelete.ContainsKey(extension))
            {
                foreach (var file in filesInProjectReal[extension])
                {
                    var filePatternsToDelete = filesInProjectToDelete[extension];
                    foreach (var filePattern in filePatternsToDelete)
                    {
                        if (SH.MatchWildcard(file + extension, filePattern + extension))
                        {
                            DeleteFilesIn(project, file + extension, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Delete files in.
    /// </summary>
    static void DeleteFilesIn(string parentPath, string fileOrFolderName, string folderWithProjectsFolders, List<string> foundFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        if (folderWithProjectsFolders != null)
        {
            string filePath = Path.Combine(parentPath, fileOrFolderName);
            if (FS.ExistsFile(filePath))
            {
                if (delete)
                {
                    if (foundFiles != null)
                    {
                        foundFiles.Add(filePath);
                    }
                }
                else
                {
                    foundFiles.Remove(filePath);
                }
                if (delete)
                {
                    try
                    {
                        string destinationPath = filePath.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
                        FS.CreateUpfoldersPsysicallyUnlessThere(destinationPath);
                        File.Move(filePath, destinationPath);
                    }
                    catch (Exception ex)
                    {
                        CL.WriteLine(ex.Message);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Delete folder in.
    /// </summary>
    static void DeleteFolderIn(ILogger logger, string parentPath, string folderName, string folderWithProjectsFolders, List<string> foundFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        var folders = Directory.GetDirectories(parentPath, folderName, SearchOption.TopDirectoryOnly);
        foreach (var folderPath in folders)
        {
            if (FS.ExistsDirectory(folderPath))
            {
                var filesFound = FSGetFiles.GetFiles(logger, folderPath, true);
                if (delete)
                {
                    if (foundFiles != null)
                    {
                        foundFiles.AddRange(filesFound);
                    }
                }
                else
                {
                    // When delete is false, remove files from list. When delete is true, add files to history list.
                    foreach (var filePath in filesFound)
                    {
                        foundFiles!.Remove(filePath);
                    }
                }
                if (delete)
                {
                    try
                    {
                        FS.MoveAllRecursivelyAndThenDirectory(logger, folderPath, FS.ReplaceDirectoryThrowExceptionIfFromDoesntExists(folderPath, folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash), FileMoveCollisionOptionDC.Overwrite);
                    }
                    catch (Exception ex)
                    {
                        ThisApp.Error("Can't delete folder: " + folderPath + " " + Exceptions.TextOfExceptions(ex));
                    }
                }
            }
        }
    }
}