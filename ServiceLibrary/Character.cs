namespace ServiceLibrary;

using AttributeListType = List<IReturnValue>;

public class Character
{
    public static AttributeListType? ListGeneralAttributes = new();

    public Character(string name, string avatarPath)
    {
        Name = name;
        AvatarPath = avatarPath;
        ListAttributes = new AttributeListType();
        GeneralAttributesValue = new List<string>(ListGeneralAttributes.Count());
    }

    public string Name { get; set; }
    public string AvatarPath { get; set; }
    public List<string> GeneralAttributesValue { get; set; }
    public AttributeListType ListAttributes { get; set; }

    public void AddAttribute(IReturnValue stat)
    {
        if (stat.Title == "")
            stat.Title = $"New Global Character Statistic {ListGeneralAttributes.Count() + ListAttributes.Count() + 1}";
        ListAttributes.Add(stat);
    }

    public static void AddGeneralAttributes(IReturnValue stat)
    {
        if (stat.Title == "") stat.Title = $"New Global Character Statistic {ListGeneralAttributes.Count() + 1}";
        ListGeneralAttributes.Add(stat);
    }

    public AttributeListType GetAttributes()
    {
        var ans = new AttributeListType();
        if (ListGeneralAttributes != null)
        {
            while (GeneralAttributesValue.Count < ListGeneralAttributes.Count) GeneralAttributesValue.Add("");
            for (var i = 0; i < GeneralAttributesValue.Count; i++)
                ans.Add((IReturnValue)
                    Activator.CreateInstance(
                        ListGeneralAttributes[i].GetType(),
                        ListGeneralAttributes[i].Title,
                        GeneralAttributesValue[i]
                    )
                );
        }

        if (ListAttributes != null)
            foreach (var item in ListAttributes)
                ans.Add((IReturnValue)
                    Activator.CreateInstance(
                        item.GetType(),
                        item.Title,
                        item.Value
                    )
                );
        return ans;
    }

    public void Refrash(AttributeListType tmplist)
    {
        for (var i = 0; i < GeneralAttributesValue.Count; i++) GeneralAttributesValue[i] = tmplist[i].Value;
        for (var i = GeneralAttributesValue.Count; i < tmplist.Count; i++)
        {
            ListAttributes[i - GeneralAttributesValue.Count].Value = tmplist[i].Value;
            ListAttributes[i - GeneralAttributesValue.Count].Title = tmplist[i].Title;
        }
    }
}