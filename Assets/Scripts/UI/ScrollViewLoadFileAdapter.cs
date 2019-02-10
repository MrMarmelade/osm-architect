using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DataObjects.FileStructure;
using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    /**
     * Connected into the editor in MainMenuLoadFile ScrollView
     */
    public class ScrollViewLoadFileAdapter : MonoBehaviour
    {
        public GameObject FileMessage;
        public GameObject Content;
        public GameObject ScrollItem;

        private int ItemCount;
        private string SelectedMap;
        private List<string> SaveFileNames;
        private List<GameObject> ScrollItems = new List<GameObject>();


        /**
         * 
         */
        private void Awake()
        {
            string SavesDirPath = Path.Combine(Application.dataPath, FileUtils.SavesPath);
            var Saves = Directory.GetFiles(SavesDirPath);
            SaveFileNames = new List<string>();
            for (var j = 0; j < Saves.Length; j++)
            {
                var fileName = Path.GetFileName(Saves[j]);
                if (!fileName.EndsWith(".meta"))
                    SaveFileNames.Add(fileName);
            }

            Sort(SaveFileNames);
            ItemCount = SaveFileNames.Count;

            //message = put files into folder, exit
            if (ItemCount == 0)
            {
                //set white background behind message
                var panel = gameObject.transform.parent.gameObject.GetComponent<Image>();
                panel.color = new Color(255, 255, 255, 0.7f);
                gameObject.SetActive(false);
            }
            //select files from list, start
            else
                FileMessage.SetActive(false);
        }

        /**
         * Sort file names in descending order
         */
        private void Sort(List<string> saveFileNames)
        {
            saveFileNames.Sort((a, b) => -1 * string.Compare(a, b, StringComparison.Ordinal));
        }

        /**
         * 
         */
        private void Start()
        {
            for (var i = 0; i < ItemCount; i++)
            {
                var scrollViewItem = Instantiate(ScrollItem);
                scrollViewItem.GetComponentInChildren<Text>().text = SaveFileNames[i].Substring(0, SaveFileNames[i].Length - 4);
                scrollViewItem.name = "item_" + i;
                scrollViewItem.transform.SetParent(Content.transform, false);
                scrollViewItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickScrollItem(scrollViewItem); });
                ScrollItems.Add(scrollViewItem);
            }

            //destroy first preview item
            --ItemCount;
            Destroy(ScrollItem);
        }

        /**
         * OnClick listener for scroll view item
         */
        private void OnClickScrollItem(GameObject scrollItem)
        {
            var position = int.Parse(Regex.Match(scrollItem.name, @"\d+").Value);
            SelectedMap = Path.Combine(Application.dataPath, FileUtils.SavesPath + SaveFileNames[position]);
            foreach (var item in ScrollItems)
            {
                item.GetComponentInChildren<Button>().GetComponent<Image>().color = Color.white;
                item.GetComponent<Image>().color = Color.white;
            }

            scrollItem.GetComponentInChildren<Button>().GetComponent<Image>().color = Color.gray;
            scrollItem.GetComponent<Image>().color = Color.gray;

            //load map
            var savedMap = Map.Load(SelectedMap);
            //map name in saved file does not exist
            if (!FileUtils.IsMapNameExist(savedMap.MapName))
                return;
            SelectedMap = null;
            Main.Map = savedMap;
            Initiate.Fade("MainLoading", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
        }
    }
}