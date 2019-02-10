using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    /**
     * Pro pohyb v menu v MainMenuScene, MainMenuFilePicker, MainMenuLoadScene and MainSettingsManual
     */
    public class PeripheryMenuInput : MonoBehaviour
    {
        public EventSystem EventSystem;
        public GameObject SelectedObject;

        private bool ButtonSelected;


        /**
         * 
         */
        private void Update()
        {
            var activeSceneName = SceneManager.GetActiveScene().name;

            //exit from application in main menu 
            if (activeSceneName.Equals("MainMenuScene") && ControlUtils.BackAction())
            {
                KeyFunctions.ExitOnButton();
                return;
            }

            //return from manual to settings
            if (activeSceneName.Equals("MainSettingsManual") && ControlUtils.BackAction())
            {
                Initiate.Fade("MainSettingsScene", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
                return;
            }

            //return from settings to main menu
            if (activeSceneName.Equals("MainSettingsScene") && ControlUtils.BackAction())
            {
                Initiate.Fade("MainMenuScene", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
                return;
            }

            //return from file picker to main menu
            if (activeSceneName.Equals("MainMenuFilePicker") && ControlUtils.BackAction())
            {
                Initiate.Fade("MainMenuScene", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
                return;
            }

            //return from save file picker to main menu
            if (activeSceneName.Equals("MainMenuLoadFile") && ControlUtils.BackAction())
            {
                Initiate.Fade("MainMenuScene", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
                return;
            }

            //move in menu with keyboard
            if (Input.GetAxisRaw("Vertical") != 0 && ButtonSelected == false)
            {
                EventSystem.SetSelectedGameObject(SelectedObject);
                ButtonSelected = true;
            }
        }

        /**
         * 
         */
        private void OnDisable()
        {
            ButtonSelected = false;
        }
    }
}