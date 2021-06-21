namespace MarkDownAvalonia.Data
{
    public static class CommonData
    {
        public static Configuration config = null;
        public static ThemeConfig theme = null;
        
        static CommonData()
        {
            // config = ConfigManager.LoadConfig();
            theme = ConfigManager.LoadTheme();
        }
    }
}