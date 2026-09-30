namespace SunamoDevCodeCore;

internal class Boilerplate
{
    public static string CSharpCmd(string innerMain)
    {
        var csharpTemplate = @"using System;

{
    class Program
    {
        static void Main(String[] args)
        {
            {0}
        }
    }
}";

        // The monolith source file used CRLF, so the verbatim template contained CRLF regardless of how this file is checked out
        csharpTemplate = csharpTemplate.Replace("\r\n", "\n").Replace("\n", "\r\n");

        var stringBuilder = new StringBuilder();
        // If it not working, try Format3. Dont use any try-catch!
        stringBuilder.AppendLine(SHFormat.Format4(csharpTemplate, innerMain));

        return stringBuilder.ToString();
    }

    public static string CSharpClass(string addNamespacesLines, string className, string fields,
        string contentOfInitMethod)
    {
        var classTemplate = @"using System;
{0}


    public class {1}
    {
        {2}

        public static void Init()
        {
            {3}
        }
    }";

        // The monolith source file used CRLF, so the verbatim template contained CRLF regardless of how this file is checked out
        classTemplate = classTemplate.Replace("\r\n", "\n").Replace("\n", "\r\n");

        var stringBuilder = new StringBuilder();

        // If it not working, try Format3. Dont use any try-catch!
        stringBuilder.AppendLine(SHFormat.Format4(classTemplate, addNamespacesLines, className, fields, contentOfInitMethod));

        return stringBuilder.ToString();
    }
}