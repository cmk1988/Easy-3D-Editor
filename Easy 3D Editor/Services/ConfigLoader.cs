using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Services
{
    [Serializable]
    class Config
    {
        public string OutputPath { get; set; }
        public int SphereLevel { get; set; }
        public string SavePath { get; set; }
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
                    OutputPath = @""
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
