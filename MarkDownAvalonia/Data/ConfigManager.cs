using System.IO;
using Newtonsoft.Json;

namespace MarkDownAvalonia.Data
{
    public static class ConfigManager
    {
        // config file name
        private static readonly string CONFIG = "config.json";
        private static readonly string THEME = "theme.json";
        
        /// <summary>
        /// load config
        /// </summary>
        /// <returns></returns>
        public static Configuration LoadConfig()
        {
            if (File.Exists(CONFIG))
            {
                using (StreamReader sr = new StreamReader(CONFIG))
                {
                    string data = sr.ReadToEnd();
                    Configuration config =  JsonConvert.DeserializeObject<Configuration>(data);
                    config.PostDirectory = Path.Combine(config.RootDirectory, "editor", "source", "_posts");
                    return config;
                }
            }

            return null;
        }
        
        public static ThemeConfig LoadTheme()
        {
            if (File.Exists(THEME))
            {
                using (StreamReader sr = new StreamReader(THEME))
                {
                    string data = sr.ReadToEnd();
                    ThemeConfig theme =  JsonConvert.DeserializeObject<ThemeConfig>(data);
                    return theme;
                }
            }

            return null;
        }
        
        /// <summary>
        /// save config
        /// </summary>
        /// <param name="config"></param>
        public static void SaveConfig(Configuration config)
        {
            if (config != null)
            {
                using (StreamWriter sw = new StreamWriter(CONFIG))
                {
                    sw.Write(JsonConvert.SerializeObject(config));
                }
            }
        }
    }
}