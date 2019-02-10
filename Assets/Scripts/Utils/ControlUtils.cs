using UnityEngine;

namespace Utils
{
    /**
     * 
     */
    public static class ControlUtils
    {
        private static float LastDPadClick;

        /**
         * 
         */
        public static bool BackAction()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button1))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool HelpAction()
        {
            if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button3))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool ConfirmAction()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool UpAction()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetKey(KeyCode.Joystick1Button3))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool DownAction()
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetKey(KeyCode.Joystick1Button2))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool ChangeAction()
        {
            if (Input.GetMouseButtonDown(1))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button5))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool PreviousBuildingAction()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetAxisRaw("DPadX") == -1 && (Time.time - LastDPadClick) > 0.3)
            {
                LastDPadClick = Time.time;
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }

        /**
         * 
         */
        public static bool NextBuildingAction()
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.KeyboardControlType;
                return true;
            }

            if (Input.GetAxisRaw("DPadX") == 1 && (Time.time - LastDPadClick) > 0.3)
            {
                LastDPadClick = Time.time;
                CanvasActionUtils.ActiveControlType = CanvasActionUtils.XboxControlType;
                return true;
            }

            return false;
        }
    }
}