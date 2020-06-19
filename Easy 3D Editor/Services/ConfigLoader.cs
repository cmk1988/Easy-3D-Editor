using System;
using System.IO;

namespace Easy_3D_Editor.Services
{
    [Serializable]
    class Config
    {
        public string OutputPath { get; set; }
        public int SphereLevel { get; set; }
        public string SavePath { get; set; }
        public string DefaultTexturepath { get; set; }

        //3D Engine settings:
        public int Width { get; set; }
        public int Height { get; set; }
        public int FpsLimit { get; set; }
    }

    class ConfigLoader
    {
        const string CONFIG_FILE = "config.dat";

        public Config Config { get; private set; }

        public static ConfigLoader Instance => new ConfigLoader();

        private void saveConfig()
        {
            Serializer.SerializeBinary(CONFIG_FILE, Config);
        }

        private ConfigLoader()
        {
            if (File.Exists(CONFIG_FILE))
            {
                Config = Serializer.DeserializeBinary<Config>(CONFIG_FILE);
            }
            else
            {
                Config = new Config
                {
                    OutputPath = @"",
                    DefaultTexturepath = @"",
                    SavePath = @"",
                    SphereLevel = 11
                };
                saveConfig();
            }
        }

        public void SetConfig(Config config)
        {
            this.Config = config;
            saveConfig();
        }
    }
}
