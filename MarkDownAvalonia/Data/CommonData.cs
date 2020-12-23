namespace MarkDownAvalonia.Data
{
    public static class CommonData
    {
        public static Configuration config = null;

        static CommonData()
        {
            config = ConfigManager.LoadConfig();
        }
    }
}