namespace SunamoAps.Aps.Projs;

// EN: Methods that were moved to SunamoMicrosoftBuild.
// They could not remain here due to transitive dependencies.
// CZ: Metody které se přesunuly do SunamoMicrosoftBuild.
// Zde nemohly zůstat kvůli transitive deps.
internal partial class VsProjectsFileHelper
{
    // This method has been moved to SunamoMicrosoftBuild.
    private static XmlNode AddNewItemGroup(string csprojPath, XmlDocument xmlDocument, XmlNamespaceManager namespaceManager, XmlNode itemGroup, XmlNode project)
    {
        throw new NotImplementedException();
    }

    // This method has been moved to SunamoMicrosoftBuild.
    private static void ReplaceProjectTemplateParameter(ref string content, VsProjectTemplateParameters templateParameter, object value)
    {
        throw new NotImplementedException();
    }

    // This method has been moved to SunamoMicrosoftBuild.
    public static void SetTargetFrameworksUap(string csprojPath, string targetFramework, string minFramework)
    {
        throw new NotImplementedException();
    }
}