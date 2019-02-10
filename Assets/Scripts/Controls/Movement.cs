using UnityEngine;
using Utils;


namespace Controls
{
    /**
     * Metody pro nastavení různých druhů ovládání
     */
    public abstract class Movement
    {
        private const float Speed = 2.0f;
        private const float Sensitivity = 10f;
        private const float MaxYAngle = 80f;
        private static Vector2 CurrentRotation;


        /**
         * Set listeners for keyboard arrows for movement
         */
        public static void SetKeyboard(Transform cameraTransform)
        {
            //UP
            if (ControlUtils.UpAction())
            {
                cameraTransform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));
            }

            //DOWN
            if (ControlUtils.DownAction())
            {
                cameraTransform.Translate(new Vector3(0, -Speed * Time.deltaTime, 0));
            }

            //S, Down arrow
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                cameraTransform.Translate(new Vector3(0, 0, -Speed * Time.deltaTime));
            }

            //W, Up arrow
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                cameraTransform.Translate(new Vector3(0, 0, Speed * Time.deltaTime));
            }
        }

        /**
         * Set listener for mouse control
         */
        public static void SetMouse(Transform cameraTransform)
        {
            CurrentRotation.x += Input.GetAxis("Mouse X") * Sensitivity;
            CurrentRotation.y -= Input.GetAxis("Mouse Y") * Sensitivity;
            CurrentRotation.x = Mathf.Repeat(CurrentRotation.x, 360);
            CurrentRotation.y = Mathf.Clamp(CurrentRotation.y, -MaxYAngle, MaxYAngle);
            cameraTransform.rotation = Quaternion.Euler(CurrentRotation.y, CurrentRotation.x, 0);
        }

        /**
         * Set listener for GamePad control
         */
        public static void SetGamePad(Transform cameraTransform)
        {
            //pohyb dopredu a dozadu
            if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
            {
                cameraTransform.Translate(new Vector3(0, 0, Input.GetAxisRaw("Vertical") * Sensitivity * Time.deltaTime));
            }

            //rozhlizeni se kamerou kolem
            if (Input.GetAxisRaw("VerticalRight") != 0 || Input.GetAxisRaw("HorizontalRight") != 0)
            {
                CurrentRotation.x += Input.GetAxisRaw("HorizontalRight") * Sensitivity;
                CurrentRotation.y -= Input.GetAxisRaw("VerticalRight") * Sensitivity;
                CurrentRotation.x = Mathf.Repeat(CurrentRotation.x, 360);
                CurrentRotation.y = Mathf.Clamp(CurrentRotation.y, -MaxYAngle, MaxYAngle);
                cameraTransform.rotation = Quaternion.Euler(CurrentRotation.y, CurrentRotation.x, 0);
            }
        }
    }
}