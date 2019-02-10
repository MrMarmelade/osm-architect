using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    /**
     * Methods for working with UI object parameters
     */
    public static class UiUtils
    {
        public const string ImageFolder = "Images/";


        /**
         * Hide cursor
         */
        public static void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /**
         * Show cursor
         */
        public static void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /**
         * Set resource text to the UI game object
         */
        public static void SetResourceText(string uiItemName, ushort resourceTextId)
        {
            var uiObject = GameObject.Find(uiItemName);
            //for text gameObjects
            if (uiObject.GetComponent<Text>() != null)
                uiObject.GetComponent<Text>().text = LanguageUtils.Get(resourceTextId);
            //for buttons
            else
                uiObject.GetComponentInChildren<Text>().text = LanguageUtils.Get(resourceTextId);
        }

        /**
         * Set text to the InputField game object
         */
        public static void SetText(InputField inputField, string text)
        {
            inputField.text = text;
        }

        /**
         * Set text to the UI game object
         */
        public static void SetText(string uiItemName, string text)
        {
            var uiObject = GameObject.Find(uiItemName);
            //for text gameObjects
            if (uiObject.GetComponent<Text>() != null)
                uiObject.GetComponent<Text>().text = text;
            //for buttons
            else
                uiObject.GetComponentInChildren<Text>().text = text;
        }

        /**
         * Set text to the UI game object
         */
        public static void SetText(GameObject gameObject, string text)
        {
            //for text gameObjects
            if (gameObject.GetComponent<Text>() != null)
                gameObject.GetComponent<Text>().text = text;
            //for buttons
            else
                gameObject.GetComponentInChildren<Text>().text = text;
        }

        /**
         * Set image to the UI game object
         */
        public static void SetImageTexture(string gameObjectName, string imageName)
        {
            if (GameObject.Find(gameObjectName) == null)
                return;

            var image = GameObject.Find(gameObjectName).GetComponent<RawImage>();
            image.texture = Resources.Load<Texture2D>(ImageFolder + imageName);
        }

        /**
         * Set default position of camera
         */
        public static void SetDefaultCameraPos(Vector3 cameraPosPoint, Transform cameraTransform)
        {
            cameraPosPoint.y = cameraPosPoint.y + 2;
            cameraTransform.position = cameraPosPoint;
        }
    }
}