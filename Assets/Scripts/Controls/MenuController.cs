using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataObjects;
using Libraries.Simple_Scene_Fade_Load_System.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using _3D;

namespace Controls
{
    /**
     * 
     */
    public class MenuController : MonoBehaviour
    {
        //SINGLETON
        private static MenuController menuController;

        private const float RotationDefault = 0f;

        private string[] resourceBuildings;
        private int indexResourceList;

        public GameObject AddressBackground;
        public GameObject AddressText;

        //pomocna promenna proti paralelnimu nacitani custom budov
        public static bool CanRenderCustomObject = true;


        /**
         * 
         */
        public static MenuController Get()
        {
            if (menuController == null)
                menuController = (new GameObject("MenuController")).AddComponent<MenuController>();
            return menuController;
        }

        /**
         * Change visibility of address info GameObjects
         */
        public void ChangeAddressBarVisibility(bool visibility)
        {
            AddressBackground.SetActive(visibility);
            AddressText.SetActive(visibility);
        }

        /**
         * Set address text into address bar game object
         */
        public void SetAddressBarText(AddressObject buildingAddress)
        {
            if (buildingAddress == null)
            {
                UiUtils.SetText(AddressText, LanguageUtils.Get(StringIds.IdMainSceneUnknownAddress));
                return;
            }

            var addressBarText = buildingAddress.Street + " " + buildingAddress.StreetNumber;
            UiUtils.SetText(AddressText, addressBarText);
        }

        /**
         * Add GameObjects using for showing address info into variables, hide them
         */
        public void AddAddressGameObjects(GameObject addressBackground, GameObject addressText)
        {
            AddressBackground = addressBackground;
            AddressBackground.SetActive(false);
            AddressText = addressText;
            AddressText.SetActive(false);
        }

        /**
         * Nastavení ovládání
         */
        public void SetCanvasListener(GameObject tutorialCanvas, GameObject buildingPickerCanvas, Text buildingName, GameObject exitCanvasDialog)
        {
            if (exitCanvasDialog.activeSelf && ControlUtils.DownAction())
            {
                ReturnToPreviousScene();
                return;
            }

            SetEscapeKeyListener(buildingPickerCanvas, exitCanvasDialog);

            SetRightClickListener();

            SetSpaceBarKeyListener(buildingPickerCanvas, exitCanvasDialog, buildingName);

            SetAltKeyListener(tutorialCanvas, exitCanvasDialog);

            BuildingPlacer.putted = false;

            SetArrowKeysListener(buildingName);
        }

        /**
         * Set click listener for alt/Y key
         */
        private void SetAltKeyListener(GameObject tutorialCanvas, GameObject exitCanvasDialog)
        {
            if (!ControlUtils.HelpAction())
                return;

            if (exitCanvasDialog.activeSelf)
            {
                Main.Settings.Tutorial = false;
                TutorialUtils.ShowTutorial(tutorialCanvas, Main.Settings);
                exitCanvasDialog.SetActive(false);
            }
        }

        /**
         * Set click listener for Escape/B key
         */
        private void SetEscapeKeyListener(GameObject buildingPickerCanvas, GameObject exitCanvasDialog)
        {
            //ESC key
            if (!ControlUtils.BackAction())
                return;

            //exit - dismiss dialog
            if (exitCanvasDialog.activeSelf)
            {
                exitCanvasDialog.SetActive(false);
                return;
            }

            //show exit dialog
            if (!buildingPickerCanvas.activeSelf && BuildingPlacer.customObject == null)
            {
                exitCanvasDialog.SetActive(true);
                SetMenuIcons();
                SetTextTitles();
            }

            //exit placing new object
            if (!buildingPickerCanvas.activeSelf && BuildingPlacer.customObject != null)
            {
                Destroy(BuildingPlacer.customObject);
                BuildingPlacer.customObject = null;
                BuildingPlacer.customObjectSize = BuildingPlacer.defaultObjectSize;
            }

            //exit of canvas
            if (buildingPickerCanvas.activeSelf)
                buildingPickerCanvas.SetActive(false);
        }

        /**
         * 
         */
        private void SetRightClickListener()
        {
            //Right click, change action with scroll wheel
            if (!ControlUtils.ChangeAction())
                return;

            if (BuildingPlacer.activeAction == CanvasActionUtils.RotateAction)
            {
                BuildingPlacer.activeAction = CanvasActionUtils.SizeAction;
                UiUtils.SetImageTexture("ScrollActiveActionIcon", "size_icon");
            }
            else
            {
                BuildingPlacer.activeAction = CanvasActionUtils.RotateAction;
                UiUtils.SetImageTexture("ScrollActiveActionIcon", "rotate_icon");
            }
        }

        /**
         * Set touch listener for SpaceBar/A key
         */
        private void SetSpaceBarKeyListener(GameObject buildingPickerCanvas, GameObject exitCanvasDialog, Text buildingNameLabel)
        {
            if (!ControlUtils.ConfirmAction())
                return;

            //exit of scene
            if (exitCanvasDialog.activeSelf)
            {
                FileUtils.CheckSaveFolder();
                Main.Map.Save();
                //todo wait, ukazat vysledek a pak vykonat zbytek
                ReturnToPreviousScene();
                return;
            }

            //hide building picker
            //load object
            if (buildingPickerCanvas.activeSelf)
            {
                var loadObjectIEnumerator = LoadObjectFromDisk(
                    resourceBuildings[indexResourceList],
                    Main.Settings.StorageType,
                    BuildingPlacer.GetMousePos(),
                    BuildingPlacer.customObjectSize,
                    RotationDefault,
                    true
                );
                StartCoroutine(loadObjectIEnumerator);
                buildingPickerCanvas.SetActive(false);
                return;
            }

            //show building picker
            if (BuildingPlacer.customObject == null && !BuildingPlacer.putted)
            {
                var directoryInfo = new DirectoryInfo(FileUtils.GetFullCustomObjectPath());
                FileInfo[] info = directoryInfo.GetFiles("*.*");
                var fileNames = new List<string>();
                foreach (var fileInfo in info)
                {
                    if (!fileInfo.Name.EndsWith(".meta"))
                        fileNames.Add(fileInfo.Name);
                }

                if (fileNames.Count != 0)
                {
                    buildingPickerCanvas.SetActive(true);
                    resourceBuildings = fileNames.ToArray();
                    indexResourceList = 0;
                    buildingNameLabel.text = resourceBuildings[indexResourceList];
                }
                //show message -- empty 3DObjects folder
                else
                {
                    AddressBackground.SetActive(true);
                    AddressText.SetActive(true);
                    UiUtils.SetText(AddressText, LanguageUtils.Get(StringIds.IdMainSceneEmpty3DFolder));
                }
            }
        }

        /**
         * 
         */
        private void SetArrowKeysListener(Text buildingNameLabel)
        {
            //RIGHT arrow, show next building
            if (ControlUtils.NextBuildingAction())
            {
                indexResourceList = indexResourceList + 1;
                if (indexResourceList >= resourceBuildings.Length)
                    indexResourceList = 0;

                buildingNameLabel.text = resourceBuildings[indexResourceList];
            }

            //LEFT arrow, show next buildings
            if (ControlUtils.PreviousBuildingAction())
            {
                indexResourceList = indexResourceList - 1;
                if (indexResourceList == resourceBuildings.Length)
                    indexResourceList = 0;

                if (indexResourceList < 0)
                    indexResourceList = resourceBuildings.Length - 1;

                buildingNameLabel.text = resourceBuildings[indexResourceList];
            }
        }

        /**
         * Load custom map object from save into the map
         */
        public void LoadCustomObjectIntoMap(string customObjectName, int storageType, Vector3 customObjectPos, float customObjectScale,
            float customObjectRotation)
        {
            var loadObjectOperation =
                LoadObjectFromDisk(customObjectName, storageType, customObjectPos, customObjectScale, customObjectRotation, false);
            StartCoroutine(loadObjectOperation);
        }

        /**
         * Load custom object from file into the map
         */
        IEnumerator LoadObjectFromDisk(string customObjectName, int storageType, Vector3 customObjectPos, float customObjectScale,
            float customObjectRotation, bool placeWithMouse)
        {
            //check if custom object exists
            if (!FileUtils.IsCustomObjectExist(customObjectName))
                yield break;

            while (!CanRenderCustomObject)
                yield return new WaitForSeconds(0.1f);
            CanRenderCustomObject = false;

            WWW www = null;
            if (storageType == FileUtils.StorageLocal)
                www = WWW.LoadFromCacheOrDownload(FileUtils.GetFullCustomObjectPath() + customObjectName, 1);
            else if (storageType == FileUtils.StorageNetwork)
                www = WWW.LoadFromCacheOrDownload("https://files.jigg.cz/" + customObjectName, 1);
            yield return www;

            var assetBundle = www.assetBundle;
            var assetBundleRequest = assetBundle.LoadAssetAsync(customObjectName, typeof(GameObject));
            yield return assetBundleRequest;

            BuildingPlacer.customObject = Instantiate(assetBundleRequest.asset) as GameObject;
            //cache transform parameter into variable
            var customObjectTransform = BuildingPlacer.customObject.transform;
            //set position
            customObjectTransform.position = customObjectPos;
            //set rotation
            if (customObjectRotation != RotationDefault)
                customObjectTransform.eulerAngles = new Vector3(0, customObjectRotation, 0);
            //set scale
            customObjectTransform.localScale = new Vector3(customObjectScale, customObjectScale, customObjectScale);
            BuildingRender.Get().ChangeBuildingSurfaceColor(BuildingPlacer.customObject, ColorUtils.DefaultBuilding);
            assetBundle.Unload(false);
            www.Dispose();
            if (!placeWithMouse)
                BuildingPlacer.customObject = null;
            CanRenderCustomObject = true;
        }

        /**
         *  Return to previous Map Picker scene
         */
        private static void ReturnToPreviousScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name.Equals("MainScene"))
            {
                Main.Map = null;
                Initiate.Fade("MainMenuScene", AnimUtils.AnimationColor, AnimUtils.AnimationLength);
            }
        }

        /**
         * Set icons to the exit panel items in MainScene
         */
        private static void SetMenuIcons()
        {
            if (CanvasActionUtils.ActiveControlType == CanvasActionUtils.KeyboardControlType)
            {
                UiUtils.SetImageTexture("MainSceneSaveIcon", "spacebar_key");
                UiUtils.SetImageTexture("MainSceneLoadIcon", "alt");
                UiUtils.SetImageTexture("MainSceneExitIcon", "ctrl");
            }

            if (CanvasActionUtils.ActiveControlType == CanvasActionUtils.XboxControlType)
            {
                UiUtils.SetImageTexture("MainSceneSaveIcon", "a_xbox");
                UiUtils.SetImageTexture("MainSceneLoadIcon", "y_xbox");
                UiUtils.SetImageTexture("MainSceneExitIcon", "x_xbox");
            }
        }

        /**
         * Set titles to the exit panel items in MainScene
         */
        private void SetTextTitles()
        {
            UiUtils.SetResourceText("MainSceneSaveLabel", StringIds.IdMainSceneMenuSave);
            UiUtils.SetResourceText("MainSceneLoadLabel", StringIds.IdMainSceneMenuLoad);
            UiUtils.SetResourceText("MainSceneExitLabel", StringIds.IdMainSceneMenuExit);
        }
    }
}