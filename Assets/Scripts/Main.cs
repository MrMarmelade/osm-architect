using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Controls;
using DataObjects;
using DataObjects.FileStructure;
using Parser;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using _3D;


/**
 * Assigned in MainScene -> MainCamera
 */
public class Main : MonoBehaviour
{
    //Variables for SAVING INTO FILE
    public static Map Map;
    public static Key Key;
    public static List<ThreeDimObject> RemovedObjects = new List<ThreeDimObject>();
    public static List<CustomObject> AddedObjects = new List<CustomObject>();
    public static Settings Settings;

    private Transform CameraTransform;

    //Variable for elevation coroutine, false = download data, true = render buildings but don't download data
    public static bool CanRenderObjects;

    //_ _ _ _ _ _
    //UNITY EDITOR variables
    public GameObject TutorialCanvas;
    public GameObject BuildingPickerCanvas;
    public Text BuildingNameLabel;

    public GameObject ExitCanvasDialog;

    //address dialog -- background
    public GameObject AddressBackground;

    //address dialog -- text
    public GameObject AddressText;
    //_ _ _ _ _ _


    /**
     * 
     */
    private void Awake()
    {
        PrepareSavedObjectsForFutureSave();
        CanRenderObjects = !Settings.TerrainToggle;
        CameraTransform = Camera.main.transform;
    }

    /**
     * Parse file and render buildings when scene starts
     */
    private void Start()
    {
        Caching.ClearCache();
        //hide and lock cursor into center
        UiUtils.HideCursor();

        TutorialUtils.InitTutorial(TutorialCanvas, Settings);
        BuildingPickerCanvas.SetActive(false);
        ExitCanvasDialog.SetActive(false);

        //load XML file tags
        var xmlFile = new XmlDocument();
        //load file
        xmlFile.Load(FileUtils.GetFullMapPath(Map.MapName));
        //get node and way tags
        var nodeTags = xmlFile.SelectNodes(".//node");
        var wayTags = xmlFile.SelectNodes(".//way");
        //cache IDs in node tag for coordinate pairing
        var nodeTagIds = CacheNodeTagIds(nodeTags);

        //parse building and roads
        var buildings = BuildingLoader.Get().LoadFile(nodeTags, nodeTagIds, wayTags);
        var roads = RoadLoader.Get().LoadFile(nodeTags, nodeTagIds, wayTags);
        var trees = TreeLoader.Get().LoadFile();
        var mapMiddlePoint = GetMiddlePoint(buildings, roads);

        //download elevation data
        AltitudeLoader.Get().SetAltitude(TerrainUtils.GetLatLngGrid(mapMiddlePoint));
        //render terrain, buildings, roads etc.
        StartCoroutine(RenderObjects(mapMiddlePoint, buildings, roads, trees));
    }

    /**
     * Set controls
     */
    private void Update()
    {
        TutorialUtils.ShowTutorial(TutorialCanvas, Settings);
        EnvironmentRender.Get().SetSkyBox();
        BuildingPlacer.PlaceNewBuildingListener();
        MenuController.Get().SetCanvasListener(TutorialCanvas, BuildingPickerCanvas, BuildingNameLabel, ExitCanvasDialog);
        Movement.SetKeyboard(CameraTransform);
        Movement.SetMouse(CameraTransform);
        Movement.SetGamePad(CameraTransform);
    }

    /**
     * Clear added/removed objects in lists when scene will be destroyed
     */
    private void OnDestroy()
    {
        RemovedObjects.Clear();
        AddedObjects.Clear();
    }

    /**
     * Render 3D objects into the scene
     */
    IEnumerator RenderObjects(LatLngObject mapMiddlePoint, List<BuildingObject> buildings, List<RoadObject> roads, List<TreeObject> trees)
    {
        while (!CanRenderObjects)
            yield return new WaitForSeconds(0.1f);
        //render terrain
        TerrainRender.Get().GenerateTerrain(mapMiddlePoint, transform);
        //render buildings
        BuildingRender.Get().Render(transform, buildings);
        //render roads
        RoadRender.Get().Render(mapMiddlePoint, transform, roads);
        //render trees
        TreeRender.Get().GenerateTrees(mapMiddlePoint, trees);
        //init address info game objects
        MenuController.Get().AddAddressGameObjects(AddressBackground, AddressText);
        CanRenderObjects = false;
    }

    /**
     * Get map middle point and set default camera position
     */
    private LatLngObject GetMiddlePoint(List<BuildingObject> buildings, List<RoadObject> roads)
    {
        LatLngObject middleMapPointLatLng = null;
        if (buildings.Count != 0)
            middleMapPointLatLng = TerrainRender.GetMiddlePoint(buildings);
        else if (roads.Count != 0)
            middleMapPointLatLng = TerrainRender.GetMiddlePoint(roads);

        //set default camera position
        var middlePoint = Converter.ConvertLatLngToXyz(middleMapPointLatLng);
        UiUtils.SetDefaultCameraPos(middlePoint, transform);
        return middleMapPointLatLng;
    }

    /**
     * Add previously added/removed buildings into array for writing to the new save file
     */
    private void PrepareSavedObjectsForFutureSave()
    {
        if (Map == null || Map.AddedCustomObjects == null)
            return;
        foreach (var customObject in Map.AddedCustomObjects)
        {
            AddedObjects.Add(customObject);
        }

        foreach (var removedMapObject in Map.RemovedMapObjects)
        {
            var threeDimObject = new ThreeDimObject(removedMapObject.Position);
            RemovedObjects.Add(threeDimObject);
        }
    }

    /**
     * Save and convert IDs of <node> tags into long array
     */
    private List<long> CacheNodeTagIds(XmlNodeList nodeTags)
    {
        var nodeTagIDs = new List<long>();
        foreach (XmlNode nodeTag in nodeTags)
        {
            nodeTagIDs.Add(long.Parse(nodeTag.Attributes["id"].Value));
        }

        return nodeTagIDs;
    }
}