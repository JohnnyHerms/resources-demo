using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace resources_demo
{
    class CustomResources
    {

        private static Dictionary<string, string> _ResourcesEnglish;
        private static Dictionary<string, string> ResourcesEnglish
        {
            get
            {
                if (_ResourcesEnglish == null)
                {
                    var folderIndentifier = ConfigurationManager.AppSettings["CustomResources.Folder"]; ;
                    var baseResources = GetDictionaryFromEmbedded("resources_demo.Properties.Resource", "en");
                    var customResources = GetDictionaryFromFile($"\\Properties\\{folderIndentifier}\\Resource.resx");
                    _ResourcesEnglish = OverwriteDictionary(baseResources, customResources);
                }

                return _ResourcesEnglish;
            }
        }

        private static Dictionary<string, string> _ResourcesSpanish;
        private static Dictionary<string, string> ResourcesSpanish
        {
            get
            {
                if (_ResourcesSpanish == null)
                {
                    var folderIndentifier = ConfigurationManager.AppSettings["CustomResources.Folder"];
                    var baseResources = GetDictionaryFromEmbedded("resources_demo.Properties.Resource", "es");
                    var customResources = GetDictionaryFromFile($"\\Properties\\{folderIndentifier}\\Resource.es.resx");
                    _ResourcesSpanish = OverwriteDictionary(baseResources, customResources);
                }

                return _ResourcesSpanish;
            }
        }

        private static Dictionary<string, string> OverwriteDictionary(Dictionary<string, string> currentDictionary, Dictionary<string, string> newDictionary, bool addIfDoesntExist = false)
        {
            var identifier = ConfigurationManager.AppSettings["CustomResources.Folder"];
            if (String.IsNullOrEmpty(identifier))
                return currentDictionary;

            foreach (var item in newDictionary)
            {
                try
                {
                    currentDictionary[item.Key] = item.Value;
                }
                catch (Exception)
                {
                    if (addIfDoesntExist)
                        currentDictionary.Add(item.Key, item.Value);
                }
            }

            return currentDictionary;
        }

        private static Dictionary<string, string> GetDictionaryFromEmbedded(string embedded, string cultureInfoCode)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            try
            {
                ResourceManager rm = new ResourceManager(embedded, Assembly.GetExecutingAssembly());
                var resourceSet = rm.GetResourceSet(new CultureInfo(cultureInfoCode), true, true);

                var resourceDictionary = resourceSet.Cast<DictionaryEntry>()
                                    .ToDictionary(r => r.Key.ToString(),
                                                  r => r.Value.ToString());
                res = resourceDictionary;
            }
            catch (Exception e)
            {
                string a = e.Message;
                // Error getting resource file
            }

            return res;
        }

        private static Dictionary<string, string> GetDictionaryFromFile(string file)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            string currentPath = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + file).Replace("file:\\", "");

            try
            {
                using (ResXResourceReader resxReader = new ResXResourceReader(currentPath))
                {
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        res.Add((string)entry.Key, (string)entry.Value);
                    }
                }
            }
            catch (Exception e)
            {
                string a = e.Message;
                // Error getting resource file
            }

            return res;
        }

        private static string GetText(string key, string language)
        {
            try
            {
                switch (language)
                {
                    case "es":
                        return ResourcesSpanish[key];
                    case "en":
                    default:
                        return ResourcesEnglish[key];
                }
            }
            catch (Exception)
            {
                return $"No value with key: {key} and language: {language}";
            }
        }

        public static string GetText(string key)
        {
            try
            {
                return GetText(key, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            }
            catch (Exception)
            {
                return $"No value with key: {key} and CurrentCulture: {Thread.CurrentThread.CurrentCulture.Name}";
            }
        }
    }
}
