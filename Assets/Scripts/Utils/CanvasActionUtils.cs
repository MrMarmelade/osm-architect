namespace Utils
{
    /**
     *  Constants for control options
     */
    public static class CanvasActionUtils
    {
        public const byte RotateAction = 0;
        public const byte SizeAction = 1;

        public const byte KeyboardControlType = 0;
        public const byte XboxControlType = 1;
        public const byte PlaystationControlType = 2;
        public const byte VrControlType = 3;

        public static byte ActiveControlType;
    }
}