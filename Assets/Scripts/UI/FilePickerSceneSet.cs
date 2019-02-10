using UnityEngine;
using Utils;

namespace UI
{
    /**
     * Connected into the editor in MainMenuFilePicker Canvas
     */
    public class FilePickerSceneSet : MonoBehaviour
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
            UiUtils.SetResourceText("ScreenTitleFilePicker", StringIds.IdFilePickerTitle);
            //set error message text
            var errorMessage = GameObject.Find("MapFileErrorMessageFilePicker");
            if (errorMessage != null)
            {
                UiUtils.SetResourceText("MapFileErrorMessageFilePicker", StringIds.IdFilePickerErrorMessage);
            }
        }
    }
}