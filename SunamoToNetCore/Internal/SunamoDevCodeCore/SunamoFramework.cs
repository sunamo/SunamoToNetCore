namespace SunamoDevCodeCore;

internal class SunamoFramework
{
    public void HtmlEntitiesForNonDigitsOrLetterChars()
    {
        AllLists.InitHtmlEntitiesFullNames();

        var charEntity = new Dictionary<char, string>();
        var constsToCreate = new List<string>();


        //foreach (var item in RLData.en)
        //{
        //    foreach (var c in item.Value)
        //    {
        //        if (!char.IsLetterOrDigit(c))
        //        {
        //            if (!charEntity.ContainsKey(c))
        //            {
        //                var cs = c.ToString();
        //                var ent = AllLists.HtmlEncode(cs);

        //                if (cs != ent)
        //                {
        //                    charEntity.Add(c, ent);
        //                    constsToCreate.Add(c.ToString());
        //                }
        //                else
        //                {

        //                }
        //            }
        //        }
        //    }
        //}

        //ClipboardHelper.SetText(CSharpHelper.GetConsts(charEntity.Values.ToList(), constsToCreate, false));

        //ClipboardHelper.SetDictionary<char, string>(charEntity, "\t");
    }
}