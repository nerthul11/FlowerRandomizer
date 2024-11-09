using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace FlowerRandomizer.Manager
{
    public class LanguageManager
    {
        public static Dictionary<string, Dictionary<string, string>> languageKeys;
        public static void Hook()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            JsonSerializer jsonSerializer = new() {TypeNameHandling = TypeNameHandling.Auto};
            using Stream languageStream = assembly.GetManifestResourceStream("FlowerRandomizer.Resources.Data.Language.json");
            StreamReader languageReader = new(languageStream);
            languageKeys = jsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(new JsonTextReader(languageReader));
        }

        public string Get(string key, string sheet)
        {
            try 
            {
                return languageKeys[sheet][key];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}