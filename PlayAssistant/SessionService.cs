using ServiceLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlayAssistant
{
    using MdListDataType = List<Pair<Type, ReturnValue>>;
    using ChrListDataType = List<CharacterBase>;
    using SessinDataType = Pair<Pair<List<CharacterBase>, List<Pair<Type, ReturnValue>>>, List<Pair<Type, ReturnValue>>>;

    public struct CharacterBase {
        public List<string> GenVal;
        public MdListDataType Attr;
        public string name;
    }
    internal static class SessionService
    {
        public static string SessionName {get; set; }
        public static List<IReturnValue> GetParams()
        {
            var ans = new List<IReturnValue>();
            foreach(Type t in Assembly.GetAssembly(typeof(PSModules.ControlClass)).GetTypes())
            {
                if (t.GetInterface("IReturnValue") != null)
                    ans.Add((IReturnValue)Activator.CreateInstance(t));
            }
            return ans;
        }
        public static List<IReturnValue> GetAttributes()
        {
            var ans = new List<IReturnValue>();
            foreach (Type t in Assembly.GetAssembly(typeof(CHRSModules.ControlClass)).GetTypes())
            {
                if (t.GetInterface("IReturnValue") != null)
                    ans.Add((IReturnValue)Activator.CreateInstance(t));
            }
            return ans;
        }

        public static void CreateSession(string _SessionName)
        {
            SessionName = _SessionName;
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
            using (StreamReader fs = new StreamReader("titles.json"))
                tmpTl = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;  
            if (tmpTl == null)
                tmpTl = new List<string>();
            tmpTl.Add(_SessionName);
            using (StreamWriter fs = new StreamWriter("titles.json"))
                serializer.Serialize(fs, tmpTl);
        }
        public static void SaveSession(SessinDataType ChrAndMd)
        {
            var ChrData = ChrAndMd.First;
            var MdData = ChrAndMd.Second;
            using (StreamWriter chr = new StreamWriter(@$"{SessionName}/Characters.json"), md = new StreamWriter($@"{SessionName}/Modules.json"))
            {
                var serializer = new JsonSerializer();
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                serializer.Serialize(chr, ChrData);
                serializer.Serialize(md, MdData);
            }
            return;
        }
        public static SessinDataType LoadSession()
        {

            SessinDataType ans;
            using (StreamReader chr = new StreamReader(@$"{SessionName}/Characters.json"), md = new StreamReader($@"{SessionName}/Modules.json"))
            {
                var serializer =new JsonSerializer();
                var ChrData = serializer.Deserialize(chr, typeof(Pair<ChrListDataType, MdListDataType>)) as Pair<ChrListDataType, MdListDataType>;
                if (ChrData == null ) { ChrData = new Pair<ChrListDataType, MdListDataType>(); }
                var MdData = serializer.Deserialize(md, typeof(MdListDataType)) as MdListDataType;
                if (MdData == null ) { MdData = new MdListDataType(); }
                ans = new SessinDataType(ChrData, MdData);
            }
            return ans;
        }
        public static CharacterBase ChrSave(Character chr)
        {
            var ans = new CharacterBase();
            ans.name = chr.Name;
            ans.Attr = IntRVtoStruct(chr.ListAttributes);
            ans.GenVal = chr.GeneralAttributesValue; ;
            return ans;
        }
        public static Character ChrLoad(CharacterBase chr)
        {
            var ans = new Character(chr.name);
            ans.GeneralAttributesValue = chr.GenVal;
            ans.ListAttributes = StructRVToInt(chr.Attr);
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
        public static List<IReturnValue> StructRVToInt(MdListDataType values)
        {
            var ans = new List<IReturnValue>();
            if (values != null) { 
                foreach(var item in values)
                {
                    var tmp = Activator.CreateInstance(
                        item.First,
                        item.Second.Title,
                        item.Second.Value
                        );
                    ans.Add(tmp as IReturnValue);
                }
            }   
            return ans;
        }
        public static List<string> SessionsList()
        {
            var ans = new List<string>();
            var serializer = new JsonSerializer();
            using (var fs = new StreamReader("titles.json"))
            {
                ans = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;
            }
            if (ans == null)
                ans = new List<string>();
            return ans;
        }

    }    
}
