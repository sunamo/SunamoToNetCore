namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoFileSystem;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{
    /// <summary>
    /// Copy move file prepare.
    /// </summary>
    internal static bool CopyMoveFilePrepare(ref string item, ref string fileTo, FileMoveCollisionOptionDC co)
    {
        //var fileTo = fileTo2.ToString();
        item = @"\\?\" + item;
        MakeUncLongPath(ref fileTo);
        CreateUpfoldersPsysicallyUnlessThere(fileTo);
        // Toto tu je důležité, nevím který kokot to zakomentoval
        if (File.Exists(fileTo))
        {
            if (co == FileMoveCollisionOptionDC.AddFileSize)
            {
                var newFn = InsertBetweenFileNameAndExtension(fileTo, " " + new FileInfo(item).Length);
                if (File.Exists(newFn))
                {
                    File.Delete(item);
                    return true;
                }

                fileTo = newFn;
            }
            else if (co == FileMoveCollisionOptionDC.AddSerie)
            {
                var serie = 1;
                while (true)
                {
                    var newFn = InsertBetweenFileNameAndExtension(fileTo, " (" + serie + ")");
                    if (!File.Exists(newFn))
                    {
                        fileTo = newFn;
                        break;
                    }

                    serie++;
                }
            }
            else if (co == FileMoveCollisionOptionDC.DiscardFrom)
            {
                // Cant delete from because then is file deleting
                if (DeleteFileMaybeLocked != null)
                    DeleteFileMaybeLocked(item);
                else
                    File.Delete(item);
            }
            else if (co == FileMoveCollisionOptionDC.Overwrite)
            {
                if (DeleteFileMaybeLocked != null)
                    DeleteFileMaybeLocked(fileTo);
                else
                    File.Delete(fileTo);
            }
            else if (co == FileMoveCollisionOptionDC.LeaveLarger)
            {
                var fsFrom = new FileInfo(item).Length;
                var fsTo = new FileInfo(fileTo).Length;
                if (fsFrom > fsTo)
                    File.Delete(fileTo);
                else //if (fsFrom < fsTo)
                    File.Delete(item);
            }
            else if (co == FileMoveCollisionOptionDC.DontManipulate)
            {
                if (File.Exists(fileTo))
                    return false;
            }
            else if (co == FileMoveCollisionOptionDC.ThrowEx)
            {
                ThrowEx.Custom($"Directory {fileTo} already exists");
            }
        }

        return true;
    }

    internal static Action<string>? DeleteFileMaybeLocked = null;
    /// <summary>
    /// Move file.
    /// </summary>
    internal static void MoveFile(string item, string fileTo, FileMoveCollisionOptionDC co)
    {
        if (CopyMoveFilePrepare(ref item, ref fileTo, co))
            try
            {
                item = MakeUncLongPath(item);
                fileTo = MakeUncLongPath(fileTo);
                if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo))
                    return;
                File.Move(item, fileTo);
            }
            catch (Exception)
            {
            //ThisApp.Error(item + " : " + ex.Message);
            }
    }

    /// <summary>
    /// Move or copy.
    /// </summary>
    private static void MoveOrCopy(string sourceDirectory, string destinationDirectory, FileMoveCollisionOptionDC collisionOption, bool isMoving, string filePath)
    {
        var destinationFile = destinationDirectory + filePath.Substring(sourceDirectory.Length);
        if (isMoving)
            MoveFile(filePath, destinationFile, collisionOption);
        else
            CopyFile(filePath, destinationFile, collisionOption);
    }

    /// <summary>
    /// Copy file.
    /// </summary>
    internal static void CopyFile(string item, string fileTo2, FileMoveCollisionOptionDC co)
    {
        var fileTo = fileTo2;
        if (CopyMoveFilePrepare(ref item, ref fileTo, co))
        {
            if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo))
                return;
            File.Copy(item, fileTo);
        }
    }

    /// <summary>
    /// Copy move all files recursively.
    /// </summary>
    private static void CopyMoveAllFilesRecursively(ILogger logger, string sourceDirectory, string destinationDirectory, FileMoveCollisionOptionDC collisionOption, bool isMoving, string mustContain, SearchOption searchOption)
    {
        var files = FSGetFiles.GetFiles(logger, sourceDirectory, "*", searchOption);
        if (!string.IsNullOrEmpty(mustContain))
        {
            foreach (var item in files)
                if (SH.IsContained(item, mustContain))
                {
                    MoveOrCopy(sourceDirectory, destinationDirectory, collisionOption, isMoving, item);
                }
        }
        else
        {
            foreach (var item in files)
                MoveOrCopy(sourceDirectory, destinationDirectory, collisionOption, isMoving, item);
        }
    }

    /// <summary>
    /// Move all recursively and then directory.
    /// </summary>
    internal static void MoveAllRecursivelyAndThenDirectory(ILogger logger, string sourceDirectory, string destinationDirectory, FileMoveCollisionOptionDC collisionOption)
    {
        CopyMoveAllFilesRecursively(logger, sourceDirectory, destinationDirectory, collisionOption, true, null!, SearchOption.TopDirectoryOnly);
        var directories = Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories);
        for (var i = directories.Length - 1; i >= 0; i--)
            TryDeleteDirectory(directories[i]);
        TryDeleteDirectory(sourceDirectory);
    }

    /// <summary>
    /// Get dictionary by extension.
    /// </summary>
    internal static Dictionary<string, List<string>> GetDictionaryByExtension(ILogger logger, string folder, string mask, SearchOption searchOption)
    {
        var extDict = new Dictionary<string, List<string>>();
        foreach (var item in FSGetFiles.GetFiles(logger, folder, mask, searchOption))
        {
            var ext = Path.GetExtension(item);
            var fn = Path.GetFileNameWithoutExtension(item).ToLower();
            if (fn == string.Empty)
            {
                fn = ext;
                ext = "";
            }

            DictionaryHelper.AddOrCreate(extDict, ext, fn);
        }

        return extDict;
    }

    /// <summary>
    /// Create upfolders psysically unless there.
    /// </summary>
    internal static void CreateUpfoldersPsysicallyUnlessThere(string path)
    {
        CreateFoldersPsysicallyUnlessThere(Path.GetDirectoryName(path)!);
    }

    /// <summary>
    /// Create folders psysically unless there.
    /// </summary>
    internal static void CreateFoldersPsysicallyUnlessThere(string path)
    {
        ThrowEx.IsNullOrEmpty(nameof(path), path);
        //ThrowEx.IsNotWindowsPathFormat(nameof(path), path);
        if (Directory.Exists(path))
        {
            return;
        }

        var foldersToCreate = new List<string>
        {
            path
        };
        while (true)
        {
            path = Path.GetDirectoryName(path)!;
            if (Directory.Exists(path))
            {
                break;
            }

            foldersToCreate.Add(path);
        }

        foldersToCreate.Reverse();
        foreach (var folder in foldersToCreate)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}