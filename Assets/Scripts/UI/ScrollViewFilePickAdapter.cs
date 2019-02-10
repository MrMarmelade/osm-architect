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
     * Connected into the editor in MainMenuFilePicker ScrollView
     */
    public class ScrollViewFilePickAdapter : MonoBehaviour
    {
        public GameObject FileMessage;
        public GameObject Content;
        public GameObject ScrollItem;

        private int ItemCount;
        private string SelectedMap;
        private List<string> MapFileNames;
        private List<GameObject> ScrollItems = new List<GameObject>();


        /**
         * 
         */
        private void Awake()
        {
            string MapDirPath = Path.Combine(Application.dataPath, FileUtils.MapsPath);
            var Maps = Directory.GetFiles(MapDirPath);
            MapFileNames = new List<string>();
            for (var j = 0; j < Maps.Length; j++)
            {
                var fileName = Path.GetFileName(Maps[j]);
                if (!fileName.EndsWith(".meta"))
                {
                    MapFileNames.Add(fileName);
                }
            }

            ItemCount = MapFileNames.Count;

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
            {
                FileMessage.SetActive(false);
            }
        }

        /**
         * 
         */
        private void Start()
        {
            for (var i = 0; i < ItemCount; i++)
            {
                var scrollViewItem = Instantiate(ScrollItem);
                scrollViewItem.GetComponentInChildren<Text>().text = MapFileNames[i].Substring(0, MapFileNames[i].Length - 4);
                scrollViewItem.name = "item_" + i;
                scrollViewItem.transform.SetParent(Content.transform, false);
                scrollViewItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickScrollItem(scrollViewItem); });
                ScrollItems.Add(scrollViewItem);
            }

            //destroy first preview item
            ItemCount--;
            Destroy(ScrollItem);
        }

        /**
         *  OnClick listener for scroll view item
         */
        private void OnClickScrollItem(GameObject scrollItem)
        {
            var position = int.Parse(Regex.Match(scrollItem.name, @"\d+").Value);
            SelectedMap = MapFileNames[position];
            foreach (var item in ScrollItems)
            {
                item.GetComponentInChildren<Button>().GetComponent<Image>().color = Color.white;
                item.GetComponent<Image>().color = Color.white;
            }

            scrollItem.GetComponentInChildren<Button>().GetComponent<Image>().color = Color.gray;
            scrollItem.GetComponent<Image>().color = Color.gray;

            //WRONG API KEY
            if (Main.Settings.TerrainToggle && !FileUtils.CheckKey(Main.Key.ApiKey))
                return;

            //open map
            Main.Map = new Map {MapName = SelectedMap};
            SelectedMap = null;
            Initiate.Fade("MainLoading", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
        }
    }
}