namespace SunamoAps.Aps.Projs;

internal partial class VsProjectsFileHelper
{
    const string sdkAttrName = "Sdk";
    // Vytvoří mi nový vsproj z templaty VS2019
    public static
        async Task<string>
        XmlClassLibraryFromTemplate(string safeProjectName)
    {
        var parameter = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\ProjectTemplates\CSharp\Windows\1033\ClassLibrary\classlibrary.csproj";
        var readResult =
            await
            TF.ReadAllText(parameter);
        string count = readResult!;
        ReplaceProjectTemplateParameter(ref count, VsProjectTemplateParameters.guid1, Guid.NewGuid());
        ReplaceProjectTemplateParameter(ref count, VsProjectTemplateParameters.safeprojectname, safeProjectName);
        ReplaceProjectTemplateParameter(ref count, VsProjectTemplateParameters.targetframeworkversion, "4.7.2");
        var list = SHGetLines.GetLines(count);
        string? trimmedLine = null;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            trimmedLine = list[i].Trim();
            if (trimmedLine.StartsWith("$if$") || trimmedLine.StartsWith("$endif"))
            {
                list.RemoveAt(i);
                continue;
            }
            if (trimmedLine == "<Compile Include=\"Class1.cs\" />" || trimmedLine == "<Compile Include=\"Properties\\AssemblyInfo.cs\" />")
            {
                list.RemoveAt(i);
                continue;
            }
        }
        return SHJoin.JoinNL(list);
    }
}