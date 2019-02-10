using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using Utils;

namespace UI
{
    /**
     * 
     */
    public class LoadOnClick : MonoBehaviour
    {
        /**
         * Set text of load saved file button in MainMenu
         */
        private void Start()
        {
            UiUtils.SetResourceText(gameObject.name, StringIds.IdMainMenuButtonLoad);
        }

        /**
         * 
         */
        public void LoadOnClickButton()
        {
            Initiate.Fade("MainMenuLoadFile", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
        }
    }
}