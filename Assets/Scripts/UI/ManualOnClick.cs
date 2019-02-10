using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using Utils;

namespace UI
{
    /**
     * 
     */
    public class ManualOnClick : MonoBehaviour
    {
        /**
         * 
         */
        public void ManualOnClickButton()
        {
            Initiate.Fade("MainSettingsManual", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
        }
    }
}