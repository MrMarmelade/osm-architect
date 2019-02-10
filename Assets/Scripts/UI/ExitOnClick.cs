using UnityEngine;
using Utils;

namespace UI
{
    /**
     * 
     */
    public class ExitOnClick : MonoBehaviour
    {
        /**
         * Set text of exit button
         */
        private void Start()
        {
            UiUtils.SetResourceText(gameObject.name, StringIds.IdMainMenuButtonExit);
        }

        /**
         * OnClick listener for EXIT button in the Main menu
         */
        public void ExitOnClickKey()
        {
            KeyFunctions.ExitOnButton();
        }
    }
}