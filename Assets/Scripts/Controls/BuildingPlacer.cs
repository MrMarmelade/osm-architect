using DataObjects.FileStructure;
using UnityEngine;
using Utils;
using _3D;

namespace Controls
{
    /**
     * Metody s implementací umísťování custom objektů do mapy
     */
    public class BuildingPlacer : MonoBehaviour
    {
        public static GameObject customObject;
        public const float defaultObjectSize = 0.005f;
        public static float customObjectSize = defaultObjectSize;

        public static int activeAction = CanvasActionUtils.RotateAction;
        public static bool putted;


        /**
         * Nastavení kláves a myši pro rotaci objektu, jeho posun, načtení a jeho umístění
         */
        public static void PlaceNewBuildingListener()
        {
            if (customObject == null)
                return;

            var customObjectTransform = customObject.transform;

            //realtime object position update
            customObjectTransform.position = GetMousePos();

            //custom object ROTATION -- MOUSE
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && activeAction == CanvasActionUtils.RotateAction)
            {
                customObjectTransform.Rotate(Vector3.up * Time.deltaTime * 150f);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f && activeAction == CanvasActionUtils.RotateAction)
            {
                customObject.transform.Rotate(Vector3.down * Time.deltaTime * 150f);
            }

            //custom object ROTATION -- GAMEPAD
            if (Input.GetAxisRaw("Horizontal") > 0 && activeAction == CanvasActionUtils.RotateAction)
            {
                customObjectTransform.Rotate(Vector3.up * Time.deltaTime * 150f);
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && activeAction == CanvasActionUtils.RotateAction)
            {
                customObjectTransform.Rotate(Vector3.down * Time.deltaTime * 150f);
            }

            //custom object SIZE changing -- MOUSE
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && activeAction == CanvasActionUtils.SizeAction)
            {
                customObjectSize = customObjectSize + defaultObjectSize * Time.deltaTime * 2;
                customObjectTransform.localScale = new Vector3(customObjectSize,
                    customObjectSize, customObjectSize);
            }

            if ((Input.GetAxis("Mouse ScrollWheel") < 0f && activeAction == CanvasActionUtils.SizeAction))
            {
                customObjectSize = customObjectSize - defaultObjectSize * Time.deltaTime * 2;
                customObjectTransform.localScale = new Vector3(customObjectSize,
                    customObjectSize, customObjectSize);
            }

            //custom object SIZE changing -- GAMEPAD
            if (Input.GetAxisRaw("Horizontal") > 0 && activeAction == CanvasActionUtils.SizeAction)
            {
                customObjectSize = customObjectSize + defaultObjectSize * Time.deltaTime * 2;
                customObjectTransform.localScale = new Vector3(customObjectSize,
                    customObjectSize, customObjectSize);
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && activeAction == CanvasActionUtils.SizeAction)
            {
                customObjectSize = customObjectSize - defaultObjectSize * Time.deltaTime * 2;
                customObjectTransform.localScale = new Vector3(customObjectSize,
                    customObjectSize, customObjectSize);
            }


            //put custom object (with spacebar or gamepad A button)
            if (ControlUtils.ConfirmAction() && customObject != null)
            {
                putted = true;
                var objectFinalPosition = GetMousePos();
                //set terrain height as y coordinate
                objectFinalPosition.y = TerrainRender.Get().Terrain.SampleHeight(objectFinalPosition);
                //remove "clone" text in the name
                var customObjectName = customObject.name.Substring(0, customObject.name.LastIndexOf("("));
                //save added object into List
                var addedObject = new CustomObject
                {
                    Position = objectFinalPosition,
                    ObjectName = customObjectName,
                    Rotation = customObjectTransform.rotation.eulerAngles.y,
                    Size = customObjectSize
                };
                Main.AddedObjects.Add(addedObject);

                //place custom object to the final position
                customObjectTransform.position = objectFinalPosition;
                customObject = null;
                customObjectSize = defaultObjectSize;
            }
        }

        /**
         * Get mouse position
         */
        public static Vector3 GetMousePos()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 3;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
}