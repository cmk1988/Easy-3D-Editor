using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Services
{
    class Config
    {
        public string OutputPath { get; set; }
    }

    class ConfigLoader
    {
        public Config Config { get; }

        public static ConfigLoader Instance => new ConfigLoader();

        private ConfigLoader()
        {
            Config = new Config
            {
                OutputPath = @"D:\Develop\Cpp\3DEngine\Engine\modell\"
            };
        }
    }
}
