namespace SunamoDevCodeCore;

// Cant be derived from FiltersNotTranslateAble because easy of finding instances of CsFileFilter
internal partial class CsFileFilter : ICsFileFilter
{
    public class EndArgs
    {
        public bool designerCs;
        public bool DesignerCs;
        public bool gCs;
        public bool gICs;
        public bool iCs;
        public bool notTranslateAble;
        public bool sharedCs;
        public bool tmp;
        public bool TMP;
        public bool xamlCs;
        // false which not to index, true which to index
        public EndArgs(bool designerCs, bool xamlCs, bool sharedCs, bool iCs /*, bool gICs*/, bool gCs, bool tmp, bool TMP, bool DesignerCs)
        {
            this.designerCs = designerCs;
            this.xamlCs = xamlCs;
            this.sharedCs = sharedCs;
            this.iCs = iCs;
            this.gCs = gCs;
            this.tmp = tmp;
            this.TMP = TMP;
            this.DesignerCs = DesignerCs;
        }
    }

    public bool AllowOnly(string filePath)
    {
        return AllowOnly(filePath, true);
    }

    public bool AllowOnly(string filePath, bool isAlsoCheckingEnds)
    {
        var hasEndMatch = true;
        return !AllowOnly(filePath, endArgs!, containsArgs!, ref hasEndMatch, isAlsoCheckingEnds);
    }

    public bool AllowOnlyContains(string itemPath)
    {
        return !AllowOnlyContains(itemPath, containsArgs!);
    }
}