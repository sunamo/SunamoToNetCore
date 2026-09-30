namespace SunamoCSharp;

internal partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    public void Ctor(int tabCount, ModifiersConstructor modifierType, string constructorName, bool autoAssign, bool isBase, params string[] parameters)
    {
        AddTab(tabCount);
        var modifierBuilder = new StringBuilder(modifierType.ToString());
        modifierBuilder[0] = char.ToLower(modifierBuilder[0]);
        sb.AddItem(modifierBuilder.ToString());
        sb.AddItem(constructorName);
        StartParenthesis();
        var parameterNames = new List<string>(parameters.Length / 2);
        for (var i = 0; i < parameters.Length; i++)
        {
            sb.AddItem(parameters[i]);
            var parameterName = parameters[++i];
            parameterNames.Add(parameterName);
            if (i != parameters.Length - 1)
                sb.AddItem(parameterName + ",");
            else
                sb.AddItem(parameterName);
        }

        EndParenthesis();
        if (!isBase)
            if (parameterNames.Count > 0)
                sb.AddItem(": base(" + string.Join(",", parameterNames.ToArray()) + ")");
        StartBrace(tabCount);
        if (autoAssign && isBase)
            foreach (var item in parameterNames)
            {
                This(tabCount, item);
                sb.AddItem("=");
                sb.AddItem(item + ";");
                sb.AppendLine();
            }

        EndBrace(tabCount);
        sb.AppendLine();
    }

    public void Property(int tabCount, AccessModifiers accessModifier, bool isStatic, string returnType, string name, object getImplementation, object setImplementation, string field, bool shortGet, bool shortSet)
    {
#region MyRegion
        AddTab(tabCount);
        PublicStatic(accessModifier, isStatic);
#endregion
        ReturnTypeName(returnType, name);
        AddTab(tabCount);
        if (shortGet && shortSet)
            sb.AddItem("{");
        else
            StartBrace(tabCount);
        var hasGetterImplementation = !(getImplementation == null || getImplementation.ToString() == false.ToString());
        if (hasGetterImplementation)
        {
            if (shortGet)
                throw new Exception("Can't be set shortGet and getImplementation in one time");
            var getterCode = getImplementation!.ToString();
            AddTab(tabCount + 1);
            sb.AddItem("get");
            StartBrace(tabCount + 1);
            AddTab(tabCount + 2);
            if (getterCode == true.ToString())
                sb.AddItem("return " + field + ";");
            else
                sb.AddItem(getterCode!);
            sb.AppendLine();
            EndBrace(tabCount + 1);
        }

        var hasSetterImplementation = !(setImplementation == null || setImplementation.ToString() == false.ToString());
        if (hasSetterImplementation)
        {
            if (shortSet)
                throw new Exception("Can't be set shortSet and setImplementation in one time");
            AddTab(tabCount + 1);
            sb.AddItem("set");
            StartBrace(tabCount + 1);
            AddTab(tabCount + 2);
            var setterCode = setImplementation!.ToString();
            if (setterCode == true.ToString())
                sb.AddItem(field + " = value;");
            else
                sb.AddItem(setterCode!);
            sb.AppendLine();
            EndBrace(tabCount + 1);
        }

        if (shortGet)
        {
            if (hasSetterImplementation)
                throw new Exception("Can't be set shortGet and getImplementation in one time");
            sb.AddItem("get;");
        }

        if (shortSet)
        {
            if (hasGetterImplementation)
                throw new Exception("Can't be set shortGet and getImplementation in one time");
            sb.AddItem("set;");
        }

        EndBrace(tabCount);
        sb.AppendLine();
    }

    public void Method(int tabCount, AccessModifiers accessModifier, bool isStatic, string returnType, string name, string bodyContent, string parametersDeclaration)
    {
        AddTab(tabCount);
        PublicStatic(accessModifier, isStatic);
        ReturnTypeName(returnType, name);
        StartParenthesis();
        sb.AddItem(parametersDeclaration);
        EndParenthesis();
        AppendLine();
        StartBrace(tabCount);
        AddTab(tabCount + 1);
        sb.AddItem(bodyContent);
        sb.AppendLine();
        EndBrace(tabCount);
        sb.AppendLine();
    }

    private void ReturnTypeName(string returnType, string name)
    {
        sb.AddItem(returnType);
        sb.AddItem(name);
    }

    public void Method(int tabCount, string header, string bodyContent)
    {
        AddTab(tabCount);
        sb.AddItem(header);
        StartBrace(tabCount);
        //AddTab(tabCount + 1);
        sb.AddItem(bodyContent);
        sb.AppendLine("");
        EndBrace(tabCount);
        sb.AppendLine();
    }

    public void Using(string usingStatement)
    {
        if (!usingStatement.StartsWith("using "))
            usingStatement = "using " + usingStatement + ";";
        else if (!usingStatement.Trim().EndsWith(";"))
            usingStatement += ";";
        sb.AddItem(usingStatement);
        sb.AppendLine();
    }

    public void If(int tabCount, string condition)
    {
        AddTab(tabCount);
        sb.AppendLine("if(" + condition + ")");
        StartBrace(tabCount);
    }

    public void Else(int tabCount)
    {
        AddTab(tabCount);
        sb.AppendLine("else");
        StartBrace(tabCount);
    }

    public void EnumWithComments(int tabCount, AccessModifiers accessModifier, string enumName, Dictionary<string, string> memberComments)
    {
        WriteAccessModifiers(accessModifier);
        AddTab(tabCount);
        sb.AddItem("enum " + enumName);
        StartBrace(tabCount);
        foreach (var item in memberComments)
        {
            XmlSummary(tabCount + 1, item.Value);
            AppendLine(tabCount + 1, item.Key + ",");
        }

        EndBrace(tabCount);
    }

    private void AppendAttribute(int tabCount, string attributeName, string attributeParameters)
    {
        var parentheses = "";
        if (attributeParameters != null)
            parentheses = "(" + attributeParameters + ")";
        AppendLine(tabCount, "[" + attributeName + parentheses + "]");
    }
}