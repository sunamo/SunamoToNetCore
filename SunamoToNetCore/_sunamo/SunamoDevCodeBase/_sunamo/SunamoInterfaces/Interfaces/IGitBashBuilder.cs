namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal interface IGitBashBuilder
{
    List<string> Commands { get; }
    /// <summary>
    /// Add.
    /// </summary>
    void Add(string filePattern);
    /// <summary>
    /// Add new remote.
    /// </summary>
    void AddNewRemote(string remoteName);
    /// <summary>
    /// Append.
    /// </summary>
    void Append(string text);
    /// <summary>
    /// Append line.
    /// </summary>
    void AppendLine();
    /// <summary>
    /// Append line.
    /// </summary>
    void AppendLine(string text);
    /// <summary>
    /// Cd.
    /// </summary>
    void Cd(string directory);
    /// <summary>
    /// Checkout.
    /// </summary>
    void Checkout(string branchName);
    /// <summary>
    /// Clean.
    /// </summary>
    void Clean(string options);
    /// <summary>
    /// Clear.
    /// </summary>
    void Clear();
    /// <summary>
    /// Clone.
    /// </summary>
    void Clone(string repositoryUrl);
    /// <summary>
    /// Commit.
    /// </summary>
    void Commit(bool isAddingAllUntrackedFiles, string commitMessage);
    /// <summary>
    /// Config.
    /// </summary>
    void Config(string configOption);
    /// <summary>
    /// Fetch.
    /// </summary>
    void Fetch(string remoteName = "");
    /// <summary>
    /// Init.
    /// </summary>
    void Init();
    /// <summary>
    /// Merge.
    /// </summary>
    void Merge(string branchName);
    /// <summary>
    /// Pull.
    /// </summary>
    void Pull();
    /// <summary>
    /// Push.
    /// </summary>
    void Push(bool isForce);
    /// <summary>
    /// Push.
    /// </summary>
    void Push(string remoteName);
    /// <summary>
    /// Remote.
    /// </summary>
    void Remote(string command);
    /// <summary>
    /// Status.
    /// </summary>
    void Status();
    /// <summary>
    /// To string.
    /// </summary>
    string ToString();
}