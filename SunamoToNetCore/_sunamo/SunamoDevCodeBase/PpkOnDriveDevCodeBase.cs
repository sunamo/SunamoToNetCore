namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

public abstract class PpkOnDriveDevCodeBase<T> : List<T>
{
    #region DPP

    protected PpkOnDriveDevCodeArgs args;

    #endregion

    private bool isSaving;

    // Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    private readonly FileSystemWatcher w = null!;

    /// <summary>
    /// Clear.
    /// </summary>
    public new async Task Clear()
    {
        base.Clear();
        await Save();
    }

    /// <summary>
    /// Load.
    /// </summary>
    public abstract
        Task
        Load();

    /// <summary>
    /// Add.
    /// </summary>
    public async Task Add(IList<T> items)
    {
        foreach (var item in items) await Add(item);
    }

    /// <summary>
    /// Add.
    /// </summary>
    public new async Task<bool> Add(T value)
    {
        var wasAdded = false;
        if (!Contains(value))
        {
            if (value!.ToString()!.Trim() != string.Empty)
            {
                base.Add(value);
                wasAdded = true;
            }
            // keep on false
        }

        // keep on false
        await Save();
        return wasAdded;
    }

    /// <summary>
    /// Load.
    /// </summary>
    private void Load(bool loadImmediately)
    {
        if (loadImmediately) Load();
    }

    /// <summary>
    /// Save.
    /// </summary>
    public async Task Save()
    {
        if (args.Save)
        {
            isSaving = true;
            var removedOrNotExists = false;
            //if (FS.ExistsFile(args.File))
            //{
            //    removedOrNotExists = FS.TryDeleteFile(args.File);
            //}
            if (removedOrNotExists)
            {
                string content;
                content = ReturnContent();
                await FileAsync.WriteAllTextAsync(args.File, content);
            }

            isSaving = false;
        }
    }

    /// <summary>
    /// Return content.
    /// </summary>
    private string ReturnContent()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.AppendLine(item!.ToString());
        return stringBuilder.ToString();
    }

    /// <summary>
    /// To string.
    /// </summary>
    public override string ToString()
    {
        return ReturnContent();
    }

    #region base

    /// <summary>
    /// Initializes a new instance of PpkOnDriveDevCodeBase.
    /// </summary>
    public PpkOnDriveDevCodeBase(PpkOnDriveDevCodeArgs args)
    {
        this.args = args;
        File.AppendAllText(args.File, "");
        //FS.CreateFileIfDoesntExists(args.File);
        Load(args.Load);
        if (args.LoadChangesFromDrive)
        {
            w = new FileSystemWatcher(Path.GetDirectoryName(args.File)!);
            w.Filter = args.File;
            w.Changed += W_Changed;
        }
    }

    /// <summary>
    /// W changed.
    /// </summary>
    private void W_Changed(object sender, FileSystemEventArgs eventArgs)
    {
        if (!isSaving) Load();
    }

    #endregion
}
