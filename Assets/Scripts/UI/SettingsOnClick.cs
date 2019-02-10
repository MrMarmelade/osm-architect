using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using Utils;

namespace UI
{
    /**
     * Connected to Unity in Settings button in MainMenu
     */
    public class SettingsOnClick : MonoBehaviour
    {
        /**
         * Set text of open settings button in MainMenu
         */
        private void Start()
        {
            UiUtils.SetResourceText(gameObject.name, StringIds.IdMainMenuButtonSettings);
        }

        /**
         * 
         */
        public void SettingsOnClickButton()
        {
            Initiate.Fade("MainSettingsScene", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
        }
    }
}