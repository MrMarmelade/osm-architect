using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;
using Utils;


namespace UI
{
    /**
     * Connected to Unity in New Button in MainMenu
     */
    public class StartOnClick : MonoBehaviour
    {
        /**
         * Create folders
         */
        private void Awake()
        {
            //disable VR
            XRSettings.enabled = false;

            //show cursor
            UiUtils.ShowCursor();

            FileUtils.CreateStartFolders();
            Main.Settings = SettingsSceneSet.LoadSettings();

            Main.Key = SettingsSceneSet.LoadKey();
            if (Main.Key.ApiKey == null)
            {
                Main.Key.ApiKey = "  ";
                Main.Key.Save();
            }
        }

        /**
         * Set text of start button
         */
        private void Start()
        {
            UiUtils.SetResourceText(gameObject.name, StringIds.IdMainMenuButtonStart);
        }

        /**
         * OnClick listener for START button in the Main menu
         */
        public void StartOnClickButton()
        {
            Initiate.Fade("MainMenuFilePicker", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
        }
    }
}