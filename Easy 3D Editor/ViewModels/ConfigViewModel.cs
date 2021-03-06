﻿using Easy_3D_Editor.Services;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ConfigViewModel : ViewModelBase
    {
        public NotifiingProperty<string> OutputPath { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> SphereLevel { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> SavePath { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> DefaultTexturePath { get; } = new NotifiingProperty<string>();

        Config config;

        public Command OutputPathCommand { get; } = new Command();
        public Command SavePathCommand { get; } = new Command();
        public Command DefaultTexturePathCommand { get; } = new Command();
        public Command SaveCommand { get; } = new Command();
        public Command CancelCommand { get; } = new Command();

        public ConfigViewModel()
        {
            config = ConfigLoader.Instance.Config;

            OutputPath.Get = config.OutputPath;
            SphereLevel.Get = config.SphereLevel.ToString();
            SavePath.Get = config.SavePath;
            DefaultTexturePath.Get = config.DefaultTexturepath;

            OutputPathCommand.ExecuteFunc = x =>
            {
                if(ViewManager.DirectoryDialog(out string path))
                {
                    OutputPath.Get = path;
                }
            };

            SavePathCommand.ExecuteFunc = x =>
            {
                if (ViewManager.DirectoryDialog(out string path))
                {
                    SavePath.Get = path;
                }
            };

            DefaultTexturePathCommand.ExecuteFunc = x =>
            {
                if (ViewManager.DirectoryDialog(out string path))
                {
                    DefaultTexturePath.Get = path;
                }
            };

            SaveCommand.ExecuteFunc = x =>
            {
                config.OutputPath = OutputPath.Get ?? "";
                config.SavePath = SavePath.Get ?? "";
                config.SphereLevel = int.Parse(SphereLevel.Get ?? "11");
                config.DefaultTexturepath = DefaultTexturePath.Get ?? "";

                ConfigLoader.Instance.SetConfig(config);
            };

            CancelCommand.ExecuteFunc = x => Close();

            SetPropertyChangeForAll();
        }
    }
}
