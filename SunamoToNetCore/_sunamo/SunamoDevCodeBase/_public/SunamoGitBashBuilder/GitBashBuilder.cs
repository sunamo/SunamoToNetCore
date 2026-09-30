namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._public.SunamoGitBashBuilder;

internal partial class GitBashBuilder : IGitBashBuilder
{
    /// <summary>
    /// Pull.
    /// </summary>
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }

    // 11-9 the repoUrl attribute has been removed because it is fully replaceable with args
    /// <summary>
    /// Clone.
    /// </summary>
    public void Clone(string args)
    {
        Git("clone " + args);
        AppendLine();
    }

    /// <summary>
    /// Commit.
    /// </summary>
    public void Commit(bool addAllUntrackedFiles, string commitMessage)
    {
        ThrowEx.IsNullOrWhitespace("commitMessage", commitMessage);
        Git("commit ");
        if (addAllUntrackedFiles)
        {
            Append("-a");
        }

        if (!string.IsNullOrWhiteSpace(commitMessage))
        {
            Append("-m " + SH.WrapWithQm(commitMessage));
        }

        AppendLine();
    }

    /// <summary>
    /// Push.
    /// </summary>
    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }

        AppendLine();
    }

    /// <summary>
    /// Push.
    /// </summary>
    public void Push(string arg)
    {
        Git("push");
        Append(arg);
        AppendLine();
    }

    /// <summary>
    /// Init.
    /// </summary>
    public void Init()
    {
        Git("init");
        AppendLine();
    }

    /// <summary>
    /// Add.
    /// </summary>
    public void Add(string filePath)
    {
        Git("add");
        Append(filePath);
        AppendLine();
    }

    /// <summary>
    /// Config.
    /// </summary>
    public void Config(string configOption)
    {
        Git("config");
        Append(configOption);
        AppendLine();
    }

    /// <summary>
    /// Clean.
    /// </summary>
    public void Clean(string cleanOptions)
    {
        Git("clean");
        Arg(cleanOptions);
        AppendLine();
    }

    /// <summary>
    /// Git.
    /// </summary>
    private void Git(string remainingCommand)
    {
        if (remainingCommand[remainingCommand.Length - 1] != ' ')
        {
            remainingCommand += " ";
        }

        StringBuilder.Append((GitForDebug ? "GitForDebug " : "git ") + remainingCommand);
    }

    /// <summary>
    /// Arg.
    /// </summary>
    private void Arg(string argument)
    {
        Append("-" + argument);
    }

    /// <summary>
    /// Remote.
    /// </summary>
    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }

    /// <summary>
    /// Status.
    /// </summary>
    public void Status()
    {
        Git("status");
        AppendLine();
    }

    /// <summary>
    /// Fetch.
    /// </summary>
    public void Fetch(string remoteName = "")
    {
        Git("fetch " + remoteName);
        AppendLine();
    }

    /// <summary>
    /// Merge.
    /// </summary>
    public void Merge(string branchName)
    {
        Git("merge " + branchName);
        AppendLine();
    }

    /// <summary>
    /// Add new remote.
    /// </summary>
    public void AddNewRemote(string remoteUrl)
    {
        Remote("add origin " + remoteUrl);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }

    /// <summary>
    /// Checkout.
    /// </summary>
    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }
    public TextBuilderDC StringBuilder { get; set; } = null!;
    /// <summary>
    /// Initializes a new instance of GitBashBuilder.
    /// </summary>
    public GitBashBuilder(TextBuilderDC stringBuilder)
    {
        this.StringBuilder = stringBuilder;
    }

    public bool GitForDebug { get; set; } = false;
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }
}