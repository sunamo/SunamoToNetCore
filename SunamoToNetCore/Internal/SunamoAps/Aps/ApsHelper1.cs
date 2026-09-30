namespace SunamoAps.Aps;

internal partial class ApsHelper : ApsPluginStatic
{
    public
    async Task
    CheckForPushInThread(object asyncPushSolutionsObject, Func<List<string>, Task<List<List<string>>>> psInvoke, string eVs, string pathGetMessagesFromGitOutput)
    {
        AsyncPushSolutions asyncPushSolutions = (AsyncPushSolutions)asyncPushSolutionsObject;
        ThisApp.Appeal("foldersWithSolutions before " + asyncPushSolutions.FoldersWithSolutions.Count);
        foreach (var sln in asyncPushSolutions.FoldersWithSolutions)
        {
            if (
    await
            GitHelper.PushSolution(asyncPushSolutions.Release, asyncPushSolutions.GitBashBuilder, asyncPushSolutions.PushArgs, asyncPushSolutions.CommitMessage!, sln.FullPathFolder, pushSolutionsData, asyncPushSolutions.GitStatus, psInvoke))
            {
                var fn = FS.GetFileName(sln.FullPathFolder);
                if (pushSolutionsData.onlyThese == VpsHelperDevCode.ListVpsNew)
                {
                    if (!pushSolutionsData.onlyThese.Contains(sln.NameSolution))
                    {
                        continue;
                    }
                }

                gitPullVps.Cd( /*Path.Combine(VpsHelperDevCode.path,*/fn);
                gitPullVps.Pull();
                ThisApp.Info(fn);
            }
        }

        var gitBashBuilderS = asyncPushSolutions.GitBashBuilder.ToString();
        //gitBashBuilderS = "abc";
        if (!string.IsNullOrWhiteSpace(gitBashBuilderS) || !asyncPushSolutions.Release)
        {
            //gitBashBuilderS = gitBashBuilder.ToString();
            //gitBashBuilderS = TF.ReadAllText(@"D:\Desktop\gitOutput.txt");
            ClipboardService.SetText(gitBashBuilderS);
            //LoginUc enterOutputOfPowershellGit = new LoginUc();
            pushSolutionsData.checkForGit = GitTypesOfMessages.fatal | GitTypesOfMessages.error;
            await ShowWindowForEnterOutputOfPowershell(gitBashBuilderS, true, eVs, pathGetMessagesFromGitOutput);
        //uc.Accept(gitBashBuilderS);
        }
        else
        {
            ThisApp.Appeal("gitBashBuilderS is null");
        }
    }

    public async Task ShowWindowForEnterOutputOfPowershell(string gitBashBuilder, bool? push, string eVs, string pathGetMessagesFromGitOutput)
    {
        if (cmd)
        {
            typed = SNumConsts.MTwo;
            while (typed == SNumConsts.MTwo)
            {
                typed = CL.UserMustTypeMultiLine("Output of powershell. Enter -2 for copy again push commands", SNumConsts.MTwo);
                if (typed == SNumConsts.MTwo)
                {
                    ClipboardService.SetText(gitBashBuilder);
                    break;
                }
            }

            var hasSucceeded = true;
            if (push.HasValue)
            {
                if (push.Value)
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(hasSucceeded, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
                else
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(hasSucceeded, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
            }
            else
            {
                await EnterOutputOfPowershellGit_ChangeDialogResult(hasSucceeded, null!, eVs, pathGetMessagesFromGitOutput);
            }
        }
        else
        {
            MoveToApsWpf();
        //enterOneValueUC = new EnterOneValueUC("output of powershell");
        //enterOneValueUC.txtEnteredText.AcceptsReturn = true;
        //enterOneValueUC.txtEnteredText.Text = string.Empty;
        //enterOneValueUC.txtEnteredText.TextWrapping = TextWrapping.Wrap;
        //enterOneValueUC.txtEnteredText.Height = 400;
        //enterOneValueUC.txtEnteredText.Width = 400;
        //SuMenuItem miCopyToClipboard = new SuMenuItem();
        //miCopyToClipboard.Header = "Copy git commands to clipboard";
        //miCopyToClipboard.Click += MiCopyToClipboard_Click;
        //miCopyToClipboard.Tag = gitBashBuilder;
        //enterOneValueUC.suSuMenuItems.Add(miCopyToClipboard);
        //enterOutputOfPowershellGitWindow = new WindowWithUserControl(enterOneValueUC, ResizeMode.CanResize, false, "enterOutputOfPowershellGit");
        //enterOneValueUC.ChangeDialogResult += EnterOutputOfPowershellGit_ChangeDialogResult;
        //enterOutputOfPowershellGitWindow.ShowDialog();
        }
    }

    const string determining = "  Determining projects to restore...";
    const string call = "Call: ";
    const string restored = "log  : Restored";
    const string failedToRestore = "log  : Failed to restore";
    public IList<string>? GetMessagesFromGitOutput(List<string> lines)
    {
        lines.RemoveAll(data => data.StartsWith(call));
        var sections = CA.Split(lines, determining);
        List<string> result = new List<string>();
        List<List<string>> restoredSections = new List<List<string>>();
        List<List<string>> failedSections = new List<List<string>>();
        foreach (var section in sections)
        {
            if (section.Count > 0)
            {
                int i = section.Count - 1;
                if (section[i].StartsWith(restored))
                {
                    restoredSections.Add(section);
                }
                else if (section[i].StartsWith(failedToRestore))
                {
                    failedSections.Add(section);
                }
                else
                {
                    ThrowEx.NotImplementedCase(section[i]);
                }
            }
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var failedSection in failedSections)
        {
            stringBuilder.AppendLine(SHJoin.JoinNL(failedSection));
        }

        ClipboardService.SetText(stringBuilder.ToString());
        return null;
    }

    // Return empty collection if shouldProcessMessages = false
    async Task<IList<string>>
    GetMessagesFromGitOutput(bool? shouldProcessMessages, string pathGetMessagesFromGitOutput, string eVs)
    {
        if (cmd)
        {
            //#if !DEBUG
            if (typed == null)
            {
                typed = CL.UserMustTypeMultiLine("git output. Enter -1 for loading default from file");
            }

            //#endif
            if (string.IsNullOrEmpty(typed))
            {
                //var pathGetMessagesFromGitOutput = @"D:\_Test\AllProjectsSearch\AllProjectsSearch\SearchInSolutionsUC\GetMessagesFromGitOutput.txt";
                FS.CreateUpfoldersPsysicallyUnlessThere(pathGetMessagesFromGitOutput);
                typed =
    await
                TF.ReadAllText(pathGetMessagesFromGitOutput);
            }
        }
        else
        {
            MoveToApsWpf();
        //typed = enterOutputOfPowershellGit.txtEnteredText.Text;
        }

        var lines = SHGetLines.GetLines(typed!);
        //const string storingIndexDone = "remote: Storing index... done";
        //const string m2m = "master -> master";
        //for (int i = lines.Count - 1; i >= 0; i--)
        //{
        //    if (lines[i].Contains(m2m))
        //    {
        //        //lines.Insert(i, "fatal: femme fatale");
        //        break;
        //    }
        //}
        var badSolutions = GetMessagesFromGitOutput(shouldProcessMessages, ref lines, pushSolutionsData.checkForGit, eVs);
        return badSolutions;
    }

    public List<string> GetMessagesFromGitOutput(bool? shouldProcessMessages, ref List<string> lines, GitTypesOfMessages gitMessageTypes, string eVs)
    {
        List<string> badSolutions = new List<string>();
        if (BTS.GetValueOfNullable(shouldProcessMessages))
        {
            lines = CA.RemoveStringsEmptyTrimBefore(lines);
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                bool shouldAdd = false;
                if (gitMessageTypes.HasFlag(GitTypesOfMessages.fatal) && line.StartsWith("fatal: "))
                {
                    shouldAdd = true;
                }

                if (gitMessageTypes.HasFlag(GitTypesOfMessages.error) && line.StartsWith("error: "))
                {
                    shouldAdd = true;
                }

                if (shouldAdd)
                {
                    // -1 is in GetNameSolution
                    string nameSolution = GetNameSolution(lines, i, eVs)!;
                    string all = nameSolution + " " + line;
                    badSolutions.Add(all);
                }
            }

            ThisApp.Success("Solutions with selected types messages were copied to clipboard.");
        }

        badSolutions = badSolutions.Distinct().ToList();
        return badSolutions;
    }

    // Will decrement A2
    // Return relative paths like Projects\_Uap\CreateW10AppGraphics
    private string? GetNameSolution(List<string> lines, int lineIndex, string eVs)
    {
        lineIndex--;
        var drive = SH.FirstCharUpper(eVs);
        string promptPrefix = "PS " + drive;
        var promptPrefixLower = promptPrefix.ToLower();
        for (; lineIndex >= 0; lineIndex--)
        {
            var line = lines[lineIndex];
            // Must be lower - in powershell output is data:, here data:
            if (!line.ToLower().StartsWith(promptPrefixLower))
            {
                continue;
            }

            //PS data:\\Documents\\vs\\Code_Projects\\CodeLearnRoslyn> git
            string pathPart = line.Replace(promptPrefix, string.Empty);
            int promptEndIndex = pathPart.IndexOf('>');
            pathPart = pathPart.Substring(0, promptEndIndex);
            return pathPart;
        }

        return null;
    }

    public void EnterOutputOfPowershellGit_ChangeDialogResult(bool? shouldProcessMessages)
    {
        EnterOutputOfPowershellGit_ChangeDialogResult(shouldProcessMessages);
    }
}