namespace Utils
{
    /**
     * 
     */
    public static class VersionUtils
    {
        private const string Version = "1.181229";

        /**
         * Set version number into label in Settings
         */
        public static void SetVersion()
        {
            var versionText = LanguageUtils.Get(StringIds.IdSettingsVersionInfo);
            UiUtils.SetText("InfoVersionText", versionText + Version);
        }
    }
}