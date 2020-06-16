using Easy_3D_Editor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ConfigViewModel : ViewModelBase
    {
        public NotifiingProperty<string> OutputPath { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> SphereLevel { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> SavePath { get; } = new NotifiingProperty<string>();

        Config config;

        public Command OutputPathCommand { get; } = new Command();
        public Command SaveCommand { get; } = new Command();
        public Command CancelCommand { get; } = new Command();

        public ConfigViewModel()
        {
            config = ConfigLoader.Instance.Config;

            OutputPath.Get = config.OutputPath;

            OutputPathCommand.ExecuteFunc = x =>
            {
                if(ViewManager.DirectoryDialog(out string path))
                {
                    OutputPath.Get = path;
                }
            };

            SaveCommand.ExecuteFunc = x =>
            {
                config.OutputPath = OutputPath.Get;

                ConfigLoader.Instance.SetConfig(config);
            };

            CancelCommand.ExecuteFunc = x => Close();

            SetPropertyChangeForAll();
        }
    }
}
