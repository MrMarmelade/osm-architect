using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Random = System.Random;

namespace UI
{
    /**
     * Connected with Unity Editor in Canvas (MainLoading.scene)
     * 
     * Start loading Main scene after picking map
     */
    public class LoadSceneSet : MonoBehaviour
    {
        public Text TipText;

        private List<string> Tips;
        private int PreviousTip = -1;


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
        private void Start()
        {
            SetTips();
            InvokeRepeating("SetNewTip", 0.0f, 5.0f);
            StartCoroutine(LoadMainSceneAsync());
        }

        /**
         * 
         */
        private void SetTextTitles()
        {
            UiUtils.SetResourceText("ScreenTitleLoading", StringIds.IdLoadingTitle);
        }

        /**
         * Load Main scene asynchronously
         */
        IEnumerator LoadMainSceneAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        /**
         * 
         */
        private void SetNewTip()
        {
            var randomTip = new Random().Next(Tips.Count);
            if (PreviousTip == randomTip && Tips.Count > 1)
            {
                SetNewTip();
                return;
            }

            PreviousTip = randomTip;
            TipText.text = Tips[randomTip];
        }

        /**
         * 
         */
        private void SetTips()
        {
            Tips = LanguageUtils.GetTips(Main.Settings.Language);
        }
    }
}