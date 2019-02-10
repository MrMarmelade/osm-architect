using DataObjects.FileStructure;
using UnityEngine;

namespace Utils
{
    /**
     * Methods for working with tutorial's stuff
     */
    public static class TutorialUtils
    {
        private static bool TutorialShowed;

        /**
         * Hide tutorial image, if 'Show tutorial' in settings was disabled
         */
        public static void InitTutorial(GameObject tutorialCanvas, Settings settings)
        {
            if (settings.Tutorial && tutorialCanvas != null)
                tutorialCanvas.SetActive(false);
        }

        /**
         * Show tutorial image depends on active controller
         *
         * When any key was pressed, then hide tutorial
         */
        public static void ShowTutorial(GameObject tutorialCanvas, Settings settings)
        {
            if (!settings.Tutorial)
            {
                tutorialCanvas.SetActive(true);
                //hide tutorial
                if (Input.anyKey)
                {
                    TutorialShowed = false;
                    settings.Tutorial = true;
                    settings.Save();
                    tutorialCanvas.SetActive(false);
                    return;
                }

                if (TutorialShowed)
                    return;

                var currentLanguage = Main.Settings.Language;
                switch (currentLanguage)
                {
                    case LanguageUtils.LangEng:
                        //show keyboard tutorial
                        if (CanvasActionUtils.ActiveControlType == CanvasActionUtils.KeyboardControlType)
                        {
                            UiUtils.SetImageTexture("TutorialImage", "keyboard_en");
                            TutorialShowed = true;
                        }
                        //show gamepad tutorial
                        else if (CanvasActionUtils.ActiveControlType == CanvasActionUtils.XboxControlType)
                        {
                            UiUtils.SetImageTexture("TutorialImage", "xbox_en");
                            TutorialShowed = true;
                        }

                        break;
                    case LanguageUtils.LangCze:
                        //show keyboard tutorial
                        if (CanvasActionUtils.ActiveControlType == CanvasActionUtils.KeyboardControlType)
                        {
                            UiUtils.SetImageTexture("TutorialImage", "keyboard_cz");
                            TutorialShowed = true;
                        }
                        //show gamepad tutorial
                        else if (CanvasActionUtils.ActiveControlType == CanvasActionUtils.XboxControlType)
                        {
                            UiUtils.SetImageTexture("TutorialImage", "xbox_cz");
                            TutorialShowed = true;
                        }

                        break;
                }
            }
        }
    }
}