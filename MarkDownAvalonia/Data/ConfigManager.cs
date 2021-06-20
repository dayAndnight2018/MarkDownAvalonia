using System.IO;
using Newtonsoft.Json;

namespace MarkDownAvalonia.Data
{
    /**
     * class to manage config
     */
    public static class ConfigManager
    {
        /**
         * application config file name
         */
        private const string CONFIG = "config.json";

        /**
         * application theme config file name
         */
        private const string THEME = "theme.json";

        /**
         * application config
         */
        private const string SLN = "application.ma";

        /**
         * check whether application config file exists
         */
        private static bool SlnExists(string mainPath)
        {
            return File.Exists(Path.Combine(mainPath, SLN));
        }

        /// <summary>
        /// load sln file config
        /// </summary>
        /// <param name="mainPath"></param>
        /// <returns></returns>
        public static Configuration loadSln(string mainPath)
        {
            if (!SlnExists(mainPath))
            {
                // create sln file
                using (var sw = new StreamWriter(Path.Combine(mainPath, SLN)))
                {
                    var sln = new Configuration
                    {
                        GitAddress = null,
                        RootDirectory = mainPath,
                        PostDirectory = Path.Combine(mainPath, "editor", "source", "_posts")
                    };
                    sw.Write(JsonConvert.SerializeObject(sln));
                    return sln;
                }
            }

            return loadSlnConfig(mainPath);
        }

        /// <summary>
        /// load sln file content
        /// </summary>
        /// <param name="mainPath"></param>
        /// <returns></returns>
        private static Configuration loadSlnConfig(string mainPath)
        {
            using (var sr = new StreamReader(Path.Combine(mainPath, SLN)))
            {
                return  JsonConvert.DeserializeObject<Configuration>(sr.ReadToEnd());
            }
        }
        
        /// <summary>
        /// load application config
        /// </summary>
        /// <returns></returns>
        public static Configuration LoadConfig()
        {
            if (!File.Exists(CONFIG)) 
                return null;
            
            using (var sr = new StreamReader(CONFIG))
            {
                var config =  JsonConvert.DeserializeObject<Configuration>(sr.ReadToEnd());
                // calculate post directory
                config.PostDirectory = Path.Combine(config.RootDirectory, "editor", "source", "_posts");
                return config;
            }
        }
        
        /// <summary>
        /// load theme config from config file
        /// </summary>
        /// <returns></returns>
        public static ThemeConfig LoadTheme()
        {
            if (!File.Exists(THEME)) 
                return null;

            using (var sr = new StreamReader(THEME))
            {
                return JsonConvert.DeserializeObject<ThemeConfig>(sr.ReadToEnd());
            }
        }
        
        /// <summary>
        /// save application config
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