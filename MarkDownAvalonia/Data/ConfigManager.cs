using System.IO;
using Newtonsoft.Json;

namespace MarkDownAvalonia.Data
{
    public static class ConfigManager
    {
        // config file name
        private static readonly string CONFIG = "config.json";
        
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