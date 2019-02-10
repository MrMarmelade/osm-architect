using System.Collections.Generic;

namespace Utils
{
    /**
     * Class with methods for getting translated texts 
     */
    public class LanguageUtils : StringIds
    {
        public const byte LangCze = 0;
        public const byte LangEng = 1;


        /**
         * Get translated string
         */
        public static string Get(ushort stringId)
        {
            var currentLang = Main.Settings.Language;
            switch (currentLang)
            {
                //CZE
                case LangCze:
                    return Language.LangCze.Get(stringId);
                //ENG 
                case LangEng:
                    return Language.LangEng.Get(stringId);
                //ENG 
                default:
                    return Language.LangEng.Get(stringId);
            }
        }

        /**
         * Get translated tips for loading screen
         */
        public static List<string> GetTips(int language)
        {
            switch (language)
            {
                //CZE
                case LangCze:
                    return Language.LangCze.GetTips();
                //ENG 
                case LangEng:
                    return Language.LangEng.GetTips();
                //ENG 
                default:
                    return new List<string>
                    {
                        "NO TIP: LANGUAGE ERROR"
                    };
            }
        }

        /**
         * Get translated list of languages
         */
        public static List<string> GetLanguagesAsText()
        {
            var currentLang = Main.Settings.Language;
            switch (currentLang)
            {
                //CZE
                case LangCze:
                    return Language.LangCze.GetLanguagesAsCzechText();
                //ENG 
                case LangEng:
                    return Language.LangEng.GetLanguagesAsEnglishText();
                //ENG 
                default:
                    return Language.LangEng.GetLanguagesAsEnglishText();
            }
        }

        /**
         * Get translated list of model loading types
         */
        public static List<string> GetModelsAsText()
        {
            var currentLang = Main.Settings.Language;
            switch (currentLang)
            {
                //CZE
                case LangCze:
                    return Language.LangCze.GetModelsAsCzechText();
                //ENG 
                case LangEng:
                    return Language.LangEng.GetModelsAsEnglishText();
                //ENG 
                default:
                    return Language.LangEng.GetModelsAsEnglishText();
            }
        }
    }
}