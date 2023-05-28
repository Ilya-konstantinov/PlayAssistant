using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using PSModules;
using ServiceLibrary;

namespace PlayAssistant;

using ModuleListDataType = List<Pair<Type, ReturnValue>>;
using CharacterListDataType = List<CharacterBase>;
using SessionDataType =
    Pair<Pair<List<CharacterBase>, List<Pair<Type, ReturnValue>>>, List<Pair<Type, ReturnValue>>>;

public struct CharacterBase
{
    public List<string> GeneralValues;
    public ModuleListDataType Attributes;
    public string Name;
    public string Pic_path;
}

internal static class SessionService
{
    public static string SessionName { get; set; }

    public static List<IReturnValue> GetParams()
    {
        var result = new List<IReturnValue>();
        foreach (var t in Assembly.GetAssembly(typeof(ControlClass)).GetTypes())
            if (t.GetInterface("IReturnValue") != null)
                result.Add((IReturnValue)Activator.CreateInstance(t));

        return result;
    }

    public static List<IReturnValue> GetAttributes()
    {
        var result = new List<IReturnValue>();
        foreach (var t in Assembly.GetAssembly(typeof(CHRSModules.ControlClass)).GetTypes())
            if (t.GetInterface("IReturnValue") != null)
                result.Add((IReturnValue)Activator.CreateInstance(t));

        return result;
    }

    public static void CreateSession(string sessionName)
    {
        SessionName = sessionName;
        Directory.CreateDirectory(SessionName);
        var serializer = new JsonSerializer();
        using (FileStream characterFile = File.Create($@"{SessionName}/Characters.json"),
               modulesFile = File.Create($@"{SessionName}/Modules.json"))
        {
            var characterList = new CharacterListDataType();
            var moduleList = new ModuleListDataType();
            serializer.Serialize(new StreamWriter(characterFile), characterList);
            serializer.Serialize(new StreamWriter(modulesFile), moduleList);
        }

        List<string> titleList;
        using (var fs = new StreamReader("titles.json"))
        {
            titleList = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;
        }

        if (titleList == null)
            titleList = new List<string>();
        titleList.Add(sessionName);
        using (var fs = new StreamWriter("titles.json"))
        {
            serializer.Serialize(fs, titleList);
        }
    }

    public static void SaveSession(SessionDataType chrAndMd)
    {
        var characterData = chrAndMd.First;
        var moduleData = chrAndMd.Second;
        try
        {
            using (StreamWriter chr = new(@$"{SessionName}/Characters.json"),
               md = new($@"{SessionName}/Modules.json"))
            {
                var serializer = new JsonSerializer();
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                serializer.Serialize(chr, characterData);
                serializer.Serialize(md, moduleData);
            }
        }
        catch (Exception ex)
        {
            return;
        }
    }

    public static SessionDataType LoadSession()
    {
        SessionDataType result;
        using (StreamReader characters = new(@$"{SessionName}/Characters.json"),
               modules = new($@"{SessionName}/Modules.json"))
        {
            var serializer = new JsonSerializer();
            var characterData =
                serializer.Deserialize(characters, typeof(Pair<CharacterListDataType, ModuleListDataType>)) as
                    Pair<CharacterListDataType, ModuleListDataType>;
            if (characterData == null) characterData = new Pair<CharacterListDataType, ModuleListDataType>();

            var moduleData = serializer.Deserialize(modules, typeof(ModuleListDataType)) as ModuleListDataType;
            if (moduleData == null) moduleData = new ModuleListDataType();

            result = new SessionDataType(characterData, moduleData);
        }

        return result;
    }

    public static CharacterBase SaveCharacter(Character chr)
    {
        var result = new CharacterBase();
        result.Name = chr.Name;
        result.Pic_path = chr.AvatarPath;
        result.Attributes = InterfaceToStructure(chr.ListAttributes);
        result.GeneralValues = chr.GeneralAttributesValue;
        return result;
    }

    public static Character LoadCharacter(CharacterBase character)
    {
        var result = new Character(character.Name, character.Pic_path);
        result.GeneralAttributesValue = character.GeneralValues;
        result.ListAttributes = StructureToInterface(character.Attributes);
        return result;
    }

    public static ModuleListDataType InterfaceToStructure(List<IReturnValue> values)
    {
        var result = new ModuleListDataType();
        foreach (var item in values)
        {
            var type = item.GetType();
            var parameters = new ReturnValue(item.Title, item.Value);
            result.Add(new Pair<Type, ReturnValue>(type, parameters));
        }

        return result;
    }

    public static List<IReturnValue> StructureToInterface(ModuleListDataType values)
    {
        var result = new List<IReturnValue>();
        foreach (var item in values)
        {
            var tmp = Activator.CreateInstance(
                item.First,
                item.Second.Title,
                item.Second.Value
            );
            result.Add(tmp as IReturnValue);
        }

        return result;
    }

    public static List<string> SessionsList()
    {
        var result = new List<string>();
        var serializer = new JsonSerializer();
        try
        {
            using var fs = new StreamReader("titles.json");
            result = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;
        }
        // На случай, если файла titles.json не существует
        catch (FileNotFoundException)
        {
            File.Create("titles.json");
            result = new List<string>();
        }

        if (result == null)
            result = new List<string>();
        return result;
    }

    public static void Delete_current_session(string game_name)
    {
        var result = new List<string>();
        var serializer = new JsonSerializer();
        try
        {
            using var fs = new StreamReader("titles.json");
            result = serializer.Deserialize(fs, typeof(List<string>)) as List<string>;
            fs.Close();
            result.Remove(game_name);
            using (var _fs = new StreamWriter("titles.json"))
            {
                serializer.Serialize(_fs, result);
                _fs.Close();
            }
            var dir = new DirectoryInfo($@"{game_name}/");
            foreach (string file in Directory.EnumerateFiles($@"./{game_name}/", "*.*", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            dir.Delete();
        }
        catch (FileNotFoundException)
        {
            File.Create("titles.json");
        }

    }
}