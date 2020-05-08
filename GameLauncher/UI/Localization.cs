using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace GameLauncher.UI
{
    internal static class Localization
    {
        static Localization()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith("localization.xml"));

            XmlDocument document = new XmlDocument();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                document.Load(stream);
            }

            ensure(document.DocumentElement.ChildNodes
                        .Cast<XmlElement>()
                        .Any(x => x.HasAttribute("isFallback") && x.GetAttribute("isFallback") == "true"),
                   "No fallback language");

            foreach (XmlElement language in document.DocumentElement.ChildNodes)
            {
                ensure(language.LocalName == "lang", "Invalid node on language level");
                ensure(language.HasAttribute("key"), "Language is missing key");

                string languageKey = language.GetAttribute("key");

                if (language.HasAttribute("isFallback") && language.GetAttribute("isFallback") == "true")
                {
                    fallbackLanguage = languageKey;
                }

                Dictionary<string, string> localizations = new Dictionary<string, string>();

                foreach (XmlElement entry in language.ChildNodes)
                {
                    ensure(entry.LocalName == "entry", "Invalid node on entry level");
                    ensure(entry.HasAttribute("key"), "Entry is missing key");
                    
                    string entryKey = entry.GetAttribute("key");

                    localizations.Add(entryKey, entry.InnerText);
                }

                localization.Add(languageKey, localizations);
            }

            void ensure(bool condition, string message)
            {
                if (!condition)
                {
                    throw new InvalidDataException($"Error in localization file!\n{message}");
                }
            }
        }

        private static readonly string fallbackLanguage;
        private static readonly Dictionary<string, Dictionary<string, string>> localization = new Dictionary<string, Dictionary<string, string>>();

        public static string GetText(string key)
        {
            string langKey;

            if (localization.ContainsKey(CultureInfo.CurrentUICulture.IetfLanguageTag))
            {
                langKey = CultureInfo.CurrentUICulture.IetfLanguageTag;
            }
            else if (localization.ContainsKey(CultureInfo.CurrentCulture.IetfLanguageTag))
            {
                langKey = CultureInfo.CurrentCulture.IetfLanguageTag;
            }
            else
            {
                langKey = fallbackLanguage;
            }

            if (localization[langKey].ContainsKey(key))
            {
                return localization[langKey][key];
            }

            return key;
        }
    }
}
