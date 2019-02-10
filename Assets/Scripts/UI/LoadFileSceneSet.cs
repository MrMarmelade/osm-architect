using UnityEngine;
using Utils;

namespace UI
{
    /**
     * Connected into the editor in MainMenuLoadFile Canvas
     */
    public class LoadFileSceneSet : MonoBehaviour
    {
        /**
         * 
         */
        private void Awake()
        {
            SetTextTitles();
        }

        /**
         * 
         */
        private void SetTextTitles()
        {
            //set title text
            UiUtils.SetResourceText("ScreenTitleLoadFile", StringIds.IdLoadFileTitle);
            //set error message text
            var errorMessage = GameObject.Find("MapFileErrorMessageLoadFile");
            if (errorMessage != null)
            {
                UiUtils.SetResourceText("MapFileErrorMessageLoadFile", StringIds.IdLoadFileErrorMessage);
            }
        }
    }
}