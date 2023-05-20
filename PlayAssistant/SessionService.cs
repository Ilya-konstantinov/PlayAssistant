using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using PSModules;
using ServiceLibrary;

namespace PlayAssistant;

using MdListDataType = List<Pair<Type, ReturnValue>>;
using ChrListDataType = List<CharacterBase>;
using SessinDataType =
    Pair<Pair<List<CharacterBase>, List<Pair<Type, ReturnValue>>>, List<Pair<Type, ReturnValue>>>;

public struct CharacterBase
{
    public List<string> GenVal;
    public MdListDataType Attr;
    public string Name;
}

internal static class SessionService
{
    public static string SessionName { get; set; }

    public static List<IReturnValue> GetParams()
    {
        var ans = new List<IReturnValue>();
        foreach (var t in Assembly.GetAssembly(typeof(PSModules.ControlClass)).GetTypes())
            if (t.GetInterface("IReturnValue") != null)
                ans.Add((IReturnValue)Activator.CreateInstance(t));

        return ans;
    }

    public static List<IReturnValue> GetAttributes()
    {
        var ans = new List<IReturnValue>();
        foreach (var t in Assembly.GetAssembly(typeof(CHRSModules.ControlClass)).GetTypes())
            if (t.GetInterface("IReturnValue") != null)
                ans.Add((IReturnValue)Activator.CreateInstance(t));

        return ans;
    }

    public static void CreateSession(string sessionName)
    {
        SessionName = sessionName;
        Directory.CreateDirectory(SessionName);
        var serializer = new JsonSerializer();
        using (FileStream chr = File.Create($@"{SessionName}/Characters.json"),
               md = File.Create($@"{SessionName}/Modules.json"))
        {
            var tmpChr = new ChrListDataType();
            var tmpMd = new MdListDataType();
            serializer.Serialize(new StreamWriter(chr), tmpChr);
            serializer.Serialize(new StreamWriter(md), tmpMd);
        }

        List<string> tmpTl;
        using (var fs = new StreamReader("titles.json"))
        {
            tmpTl = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;
        }

        if (tmpTl == null)
            tmpTl = new List<string>();
        tmpTl.Add(sessionName);
        using (var fs = new StreamWriter("titles.json"))
        {
            serializer.Serialize(fs, tmpTl);
        }
    }

    public static void SaveSession(SessinDataType chrAndMd)
    {
        var chrData = chrAndMd.First;
        var mdData = chrAndMd.Second;
        using (StreamWriter chr = new(@$"{SessionName}/Characters.json"),
               md = new($@"{SessionName}/Modules.json"))
        {
            var serializer = new JsonSerializer();
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Serialize(chr, chrData);
            serializer.Serialize(md, mdData);
        }
    }

    public static SessinDataType LoadSession()
    {
        SessinDataType ans;
        using (StreamReader chr = new(@$"{SessionName}/Characters.json"),
               md = new($@"{SessionName}/Modules.json"))
        {
            var serializer = new JsonSerializer();
            var chrData =
                serializer.Deserialize(chr, typeof(Pair<ChrListDataType, MdListDataType>)) as
                    Pair<ChrListDataType, MdListDataType>;
            if (chrData == null) chrData = new Pair<ChrListDataType, MdListDataType>();

            var mdData = serializer.Deserialize(md, typeof(MdListDataType)) as MdListDataType;
            if (mdData == null) mdData = new MdListDataType();

            ans = new SessinDataType(chrData, mdData);
        }

        return ans;
    }

    public static CharacterBase ChrSave(Character chr)
    {
        var ans = new CharacterBase();
        ans.Name = chr.Name;
        ans.Attr = IntRVtoStruct(chr.ListAttributes);
        ans.GenVal = chr.GeneralAttributesValue;
        ;
        return ans;
    }

    public static Character ChrLoad(CharacterBase chr)
    {
        var ans = new Character(chr.Name);
        ans.GeneralAttributesValue = chr.GenVal;
        ans.ListAttributes = StructRvToInt(chr.Attr);
        return ans;
    }

    public static MdListDataType IntRVtoStruct(List<IReturnValue> values)
    {
        var ans = new MdListDataType();
        foreach (var item in values)
        {
            var t = item.GetType();
            var pr = new ReturnValue(item.Title, item.Value);
            ans.Add(new Pair<Type, ReturnValue>(t, pr));
        }

        return ans;
    }

    public static List<IReturnValue> StructRvToInt(MdListDataType values)
    {
        var ans = new List<IReturnValue>();
        if (values != null)
            foreach (var item in values)
            {
                var tmp = Activator.CreateInstance(
                    item.First,
                    item.Second.Title,
                    item.Second.Value
                );
                ans.Add(tmp as IReturnValue);
            }

        return ans;
    }

    public static List<string> SessionsList()
    {
        var ans = new List<string>();
        var serializer = new JsonSerializer();
        try
        {
            using (var fs = new StreamReader("titles.json"))
            {
                ans = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;
            }
        }
        // На случай, если файла titles.json не существует
        catch (FileNotFoundException e)
        {
            File.Create("titles.json");
            return new List<string>();
        }

        if (ans == null)
            ans = new List<string>();
        return ans;
    }
}