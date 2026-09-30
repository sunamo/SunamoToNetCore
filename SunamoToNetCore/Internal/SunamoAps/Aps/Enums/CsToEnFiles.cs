namespace SunamoAps.Aps.Enums;

internal enum CsToEnFiles
{
    // Files commented

    //AllStrings,
    //AssemblyInfo,
    //RegexHelper,
    //XamlGenerator,
    //ConstsShared,
    //CSharpGenerator,
    //GeneratorCpp,
    //MSColumnsDB,
    //JavaScriptInjection,

        // Files
    CL,
    FS,
    MailSender,
    PathSelector,
    RoslynHelper,
    SqlServerHelper,
    SunamoStrings,
    TextBoxBackend,
    TextBoxHelper,
    XamlDisplay,
    HtmlGenerator2,
    PowershellBuilder,
    JunctionPoint,
    ColorPicker,
    UH,
    SHShared,
    HtmlHelperShared,
    MSStoredProceduresIBaseShared,
    FSShared,
    SH,
    ChartsCs,
    AppsHtmlGenerator,

    // multiline string without lateral
    _CodeFragment,
    _Consts,
    _NumberOnSides,
    // var b = "{0}"; return $"c {b}";
    _StringInterpolation,
    // dont know purpose
    _EncodedQuote,
    // return sess.i18n(XlfKeys.HelloWorld);
    _AlreadyTranslated,
    _LeadingTrailing,

    // Last item to allow all have comma on end(
    End
}