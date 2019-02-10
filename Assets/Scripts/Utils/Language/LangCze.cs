using System.Collections.Generic;

namespace Utils.Language
{
    /**
     * Translated strings for CZECH language
     */
    public class LangCze : StringIds
    {
        /**
         * 
         */
        public static string Get(ushort stringId)
        {
            switch (stringId)
            {
                case IdMainMenuButtonStart:
                    return "nový";
                case IdMainMenuButtonLoad:
                    return "načíst";
                case IdMainMenuButtonSettings:
                    return "nastavení";
                case IdMainMenuButtonExit:
                    return "ukončit";
                case IdSettingsTitle:
                    return "Nastavení";
                case IdSettingsLanguageTitle:
                    return "Jazyk";
                case IdSettingsGraphicsTitle:
                    return "Grafika";
                case IdSettingsRoadObjectsTitle:
                    return "Doprava";
                case IdSettingsTreesTitle:
                    return "Počet stromů";
                case IdFilePickerTitle:
                    return "Vybrat mapu";
                case IdFilePickerButtonPick:
                    return "vybrat";
                case IdFilePickerErrorMessage:
                    return "Exportované OSM soubory vložte do složky Maps, prosím";
                case IdLoadingTitle:
                    return "Načítání mapy";
                case IdSettingsModelsTitle:
                    return "Modely";
                case IdSettingsModelsLoadingStorageTitle:
                    return "Typ úložiště";
                case IdLoadFileTitle:
                    return "Načíst mapu";
                case IdLoadFileButtonPick:
                    return "načíst";
                case IdLoadFileErrorMessage:
                    return "Žádné uložené mapy. Vložte vaše uložené mapy do složky Saves, prosím";
                case IdSettingsTutorialTitle:
                    return "Zobrazit tutorial";
                case IdMainSceneMenuSave:
                    return "ULOŽIT";
                case IdMainSceneMenuLoad:
                    return "NÁPOVĚDA";
                case IdMainSceneMenuExit:
                    return "ODEJÍT BEZ \nULOŽENÍ";
                case IdSettingsTerrainTitle:
                    return "Skutečný terén";
                case IdSettingsTerrainInfo:
                    return "Před označením čtěte návod, prosím";
                case IdSettingsTreesInfo:
                    return "Pro vlastní pozice stromů čtěte návod, prosím";
                case IdSettingsVersionInfo:
                    return "Verze ";
                case IdMainSceneEmpty3DFolder:
                    return "Prázdná složka 3DObjects";
                case IdManualTerrainText:
                    return
                        "Pro povolení funkce načtení terénu je potřeba získat tzv. API klíč ze služby Google Cloud Platform, konkrétně Maps Elevation API. Tento klíč vložte do pole pod tímto textem, nebo přímo do souboru apikey.xml ve složce programu.";
                case IdManualTreesTitle:
                    return "Vlastní pozice stromů";
                case IdManualTreesText:
                    return
                        "Pro načtení vlastních pozic stromů stačí vložit soubor s názvem trees_data.xml do složky Trees. \nSoubor se načte automaticky. \nData v souboru musí být uložena v této struktuře:";
                case IdMainSceneUnknownAddress:
                    return "Neznámá";
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
                "Stiskem 'SPACEBAR' vyvoláte menu pro výběr budovy",
                "Při umísťování budovy je možné přepínat mezi rotací a velikostí objektu pravým tlačítkem myši",
                "Do 'Dark mode' a zpět se přepínáte klávesou 'E'",
                "Vaši budovu umístíte klávesou 'SPACEBAR'",
                "Klávesou 'ESCAPE' rušíte operace, nebo se vracíte zpět"
            };
        }

        /**
         * 
         */
        public static List<string> GetLanguagesAsCzechText()
        {
            return new List<string>()
            {
                "Čeština",
                "Angličtina"
            };
        }

        /**
         * 
         */
        public static List<string> GetModelsAsCzechText()
        {
            return new List<string>()
            {
                "Lokální úložiště"
            };
        }
    }
}