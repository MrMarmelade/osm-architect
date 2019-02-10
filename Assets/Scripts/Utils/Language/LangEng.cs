using System.Collections.Generic;

namespace Utils.Language
{
    /**
     * Translated strings for ENGLISH language
     */
    public class LangEng : StringIds
    {
        /**
         * 
         */
        public static string Get(ushort stringId)
        {
            switch (stringId)
            {
                case IdMainMenuButtonStart:
                    return "new";
                case IdMainMenuButtonLoad:
                    return "load";
                case IdMainMenuButtonSettings:
                    return "settings";
                case IdMainMenuButtonExit:
                    return "exit";
                case IdSettingsTitle:
                    return "Settings";
                case IdSettingsLanguageTitle:
                    return "Language";
                case IdSettingsGraphicsTitle:
                    return "Graphics";
                case IdSettingsRoadObjectsTitle:
                    return "Road objects";
                case IdSettingsTreesTitle:
                    return "Number of trees";
                case IdFilePickerTitle:
                    return "Select map";
                case IdFilePickerButtonPick:
                    return "select";
                case IdFilePickerErrorMessage:
                    return "Put your exported OSM files into Maps folder, please";
                case IdLoadingTitle:
                    return "Loading map";
                case IdSettingsModelsTitle:
                    return "Models";
                case IdSettingsModelsLoadingStorageTitle:
                    return "Storage type";
                case IdLoadFileTitle:
                    return "Load map";
                case IdLoadFileButtonPick:
                    return "load";
                case IdLoadFileErrorMessage:
                    return "No saved maps. Put your saves into Saves folder, please";
                case IdSettingsTutorialTitle:
                    return "Show tutorial";
                case IdMainSceneMenuSave:
                    return "SAVE";
                case IdMainSceneMenuLoad:
                    return "HELP";
                case IdMainSceneMenuExit:
                    return "EXIT WITHOUT \nSAVING";
                case IdSettingsTerrainTitle:
                    return "Real terrain";
                case IdSettingsTerrainInfo:
                    return "Before selecting read manual, please";
                case IdSettingsTreesInfo:
                    return "For custom positions of trees read manual, please";
                case IdSettingsVersionInfo:
                    return "Version ";
                case IdMainSceneEmpty3DFolder:
                    return "Empty folder 3DObjects";
                case IdManualTerrainText:
                    return
                        "To enable loading of real terrain, you need to get the so-called API key from Google Cloud Platform, specifically the Maps Elevation API. Put this key in the input field below this text or directly into the apikey.xml file in the program folder.";
                case IdManualTreesTitle:
                    return "Custom tree positions";
                case IdManualTreesText:
                    return
                        "To enable loading of your own tree positions, just insert a file named trees_data.xml into the Trees folder. \nThe file will be loaded automatically. \nThe data in the file must be stored in the following structure:";
                case IdMainSceneUnknownAddress:
                    return "Unknown";
                default: return "";
            }
        }

        /**
         * 
         */
        public static List<string> GetTips()
        {
            return new List<string>
            {
                "Press 'SPACEBAR' for opening building picker",
                "You can switch between rotation and object size changing using right click on your mouse",
                "You can switch into 'Dark mode' and back using key 'E'",
                "You can place building using key 'SPACEBAR'",
                "You can cancel operation or return back using key 'ESCAPE'"
            };
        }

        /**
         * 
         */
        public static List<string> GetLanguagesAsEnglishText()
        {
            return new List<string>()
            {
                "Czech",
                "English"
            };
        }

        /**
         * 
         */
        public static List<string> GetModelsAsEnglishText()
        {
            return new List<string>()
            {
                "Local storage"
            };
        }
    }
}