using System.IO;
using DataObjects.FileStructure;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    /**
     * Connected into scene MainSettingsScene in Canvas
     */
    public class SettingsSceneSet : MonoBehaviour
    {
        private Settings Settings;
        public const string FilePathSettings = "./settings.xml";
        public const string FilePathKey = "./apikey.xml";
        private static int DefaultNumberOfTrees = 100;

        //UNITY EDITOR variables
        public Dropdown LanguageDropdown;
        public Toggle TerrainToggle;
        public Toggle RoadObjectsToggle;
        public Text NumberOfTrees;
        public Slider NumberOfTreesSlider;
        public Dropdown ModelDropdown;
        public GameObject ModelUrl;
        public Toggle TutorialToggle;


        /**
         * 
         */
        private void Awake()
        {
            Settings = Main.Settings;

            SetDropDownOptions();
            //set language dropdown 
            LanguageDropdown.value = Settings.Language;
            LanguageDropdown.onValueChanged.AddListener(delegate { SaveSettings(); });

            TerrainToggle.isOn = Settings.TerrainToggle;
            TerrainToggle.onValueChanged.AddListener(delegate { SaveSettings(); });

            //set model dropdown
            ModelDropdown.value = 0;
            ModelDropdown.onValueChanged.AddListener(delegate { SaveSettings(); });

            //set road objects toggle 
            RoadObjectsToggle.isOn = Settings.RoadObjectsToggle;
            RoadObjectsToggle.onValueChanged.AddListener(delegate { SaveSettings(); });

            //set tutorial toggle 
            TutorialToggle.isOn = !Settings.Tutorial;
            TutorialToggle.onValueChanged.AddListener(delegate { SaveSettings(); });

            //set slider
            if (Settings.NumberOfTrees >= 0 && Settings.NumberOfTrees <= 800)
            {
                NumberOfTreesSlider.value = Settings.NumberOfTrees;
            }
            else
            {
                Settings.NumberOfTrees = DefaultNumberOfTrees;
                NumberOfTreesSlider.value = DefaultNumberOfTrees;
            }

            NumberOfTreesSlider.onValueChanged.AddListener(delegate { SetNumberOfTreesText(); });

            //set slider label
            NumberOfTrees.text = Settings.NumberOfTrees.ToString();

            //set url input field
            if (ModelDropdown.value != FileUtils.StorageNetwork)
            {
                ModelUrl.SetActive(false);
            }

            SetTextTitles();
        }

        /**
         * Set translated text into labels
         */
        private void SetTextTitles()
        {
            UiUtils.SetResourceText("ScreenTitleSettings", StringIds.IdSettingsTitle);
            UiUtils.SetResourceText("LanguageLabelSettings", StringIds.IdSettingsLanguageTitle);
            UiUtils.SetResourceText("GraphicsLabelSettings", StringIds.IdSettingsGraphicsTitle);
            UiUtils.SetResourceText("RoadObjectsLabelSettings", StringIds.IdSettingsRoadObjectsTitle);
            UiUtils.SetResourceText("TreesLabelSettings", StringIds.IdSettingsTreesTitle);
            UiUtils.SetResourceText("ModelLabelSettings", StringIds.IdSettingsModelsTitle);
            UiUtils.SetResourceText("ModelTypeLabelSettings", StringIds.IdSettingsModelsLoadingStorageTitle);
            UiUtils.SetResourceText("ShowTutorialLabel", StringIds.IdSettingsTutorialTitle);
            UiUtils.SetResourceText("TerrainLabelSettings", StringIds.IdSettingsTerrainTitle);
            UiUtils.SetResourceText("InfoTreesText", StringIds.IdSettingsTreesInfo);
            UiUtils.SetResourceText("InfoTerrainText", StringIds.IdSettingsTerrainInfo);
            VersionUtils.SetVersion();
        }

        /**
         * 
         */
        private void SetNumberOfTreesText()
        {
            var sliderValue = NumberOfTreesSlider.value;
            NumberOfTrees.text = ((int) sliderValue).ToString();
            SaveSettings();
        }

        /**
         * 
         */
        private void SaveSettings()
        {
            Settings.Tutorial = !TutorialToggle.isOn;
            //API key is wrong, disable toggle
            if (!FileUtils.CheckKey(Main.Key.ApiKey))
                TerrainToggle.isOn = false;
            Settings.TerrainToggle = TerrainToggle.isOn;
            Settings.Language = LanguageDropdown.value;
            Settings.StorageType = ModelDropdown.value;
            Settings.RoadObjectsToggle = RoadObjectsToggle.isOn;
            Settings.NumberOfTrees = int.Parse(NumberOfTrees.text);
            Settings.Save();

            SetTextTitles();
            SetDropDownOptions();
        }

        /**
         * 
         */
        private void SetDropDownOptions()
        {
            LanguageDropdown.ClearOptions();
            LanguageDropdown.AddOptions(LanguageUtils.GetLanguagesAsText());
            ModelDropdown.ClearOptions();
            ModelDropdown.AddOptions(LanguageUtils.GetModelsAsText());
        }

        /**
         * Load key from XML file
         */
        public static Key LoadKey()
        {
            var fullFilePath = Path.Combine(Application.dataPath, FilePathKey);
            if (File.Exists(fullFilePath))
            {
                return Key.Load(fullFilePath);
            }

            return new Key();
        }

        /**
         *  Load settings from XML file
         */
        public static Settings LoadSettings()
        {
            var fullFilePath = Path.Combine(Application.dataPath, FilePathSettings);
            if (File.Exists(fullFilePath))
            {
                return Settings.Load(fullFilePath);
            }

            //default settings
            return new Settings
            {
                NumberOfTrees = DefaultNumberOfTrees,
                Language = LanguageUtils.LangEng,
                TerrainToggle = false,
                RoadObjectsToggle = true,
                Tutorial = false
            };
        }
    }
}