using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    /**
     * 
     */
    public class ManualSceneSet : MonoBehaviour
    {
        public InputField TerrainKey;


        /**
         * 
         */
        private void Awake()
        {
            SetTextTitles();
            SetInputFieldUpdate();
        }

        /**
         * 
         */
        private void SetInputFieldUpdate()
        {
            UiUtils.SetText(TerrainKey, Main.Key.ApiKey);
            TerrainKey.onValueChanged.AddListener(delegate { SaveApiKeyInputFieldChanges(); });
        }

        /**
         * 
         */
        private void SaveApiKeyInputFieldChanges()
        {
            Main.Key.ApiKey = TerrainKey.text;
            Main.Key.Save();
        }

        /**
         * 
         */
        private void SetTextTitles()
        {
            UiUtils.SetResourceText("ManualTerrainTitle", StringIds.IdSettingsTerrainTitle);
            UiUtils.SetResourceText("ManualTerrainText", StringIds.IdManualTerrainText);
            UiUtils.SetResourceText("ManualTreesTitle", StringIds.IdManualTreesTitle);
            UiUtils.SetResourceText("ManualTreesText", StringIds.IdManualTreesText);
        }
    }
}