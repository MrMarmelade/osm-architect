using UnityEngine;


namespace UI
{
    /**
     * 
     */
    public class KeyFunctions : MonoBehaviour
    {
        /**
         * Closes the Application (connected to ExitOnClickKey => Exit button)
         */
        public static void ExitOnButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
    #endif
        }
    }
}