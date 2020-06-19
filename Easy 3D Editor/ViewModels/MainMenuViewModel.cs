using Easy_3D_Editor.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        public NotifiingProperty<Canvas> Can { get; set; } = new NotifiingProperty<Canvas>();

        public NotifiingProperty<List<Line>> Canvas { get; set; } = new NotifiingProperty<List<Line>>();

        public Command Test { get; set; }
        public Command NewModel { get; set; }
        public Command NewMap { get; set; }
        public Command Test2 { get; set; }
        public Command NewMapElement { get; set; }
        public Command SetConfig { get; set; }

        public MainMenuViewModel()
        {
            Test = new Command
            {
                //ExecuteFunc = x => test()
            };
            NewModel = new Command
            {
                ExecuteFunc = x => newModel()
            };
            NewMap = new Command
            {
                ExecuteFunc = x => newMap()
            };
            NewMapElement = new Command
            {
                ExecuteFunc = x => newMapElement()
            };
            SetConfig = new Command
            {
                ExecuteFunc = x => setConfig()
            };
            SetPropertyChangeForAll();
        }

        void setConfig()
        {
            var vm = new ConfigViewModel();
            ViewManager.ShowDialogView(typeof(ConfigView), vm);
        }

        void newMapElement()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie die größe an (1,1)";
            bool b = false;
            while (!b)
            {
                ViewManager.ShowDialogView(typeof(Input), vm);
                if (vm.IsOK)
                {
                    var split = vm.Output.Get.Split(',');
                    if (split.Length == 2)
                    {
                        if(int.TryParse(split[0], out int i1) && int.TryParse(split[1], out int i2))
                        {
                            var vmXY = new ViewModelXY(1);
                            var vmXZ = new ViewModelXY(2);
                            var vmYZ = new ViewModelXY(3);

                            var mVm = new ModelViewModel(vmXY, vmXZ, vmYZ, i1, i2);

                            ViewManager.ShowView(typeof(ModelMainMenu), mVm);
                            ViewManager.ShowView(typeof(ViewXY), vmXY);
                            ViewManager.ShowView(typeof(ViewXY), vmXZ);
                            ViewManager.ShowView(typeof(ViewXY), vmYZ);
                            b = true;
                        }
                        else
                            MessageBox.Show(ViewManager.RootView, "Die Eingebe konnte nicht zu INT geparst werden!");
                    }
                    else
                        MessageBox.Show(ViewManager.RootView, "Die Eingebe war fehlerhaft! Bitte geben Sie zwei Zahlen ein, getrennt mit einem Komma.");
                }
                else b = true;
            }
        }

        void newMap()
        {
            var vm = new MapEditorViewModel(10,10);
            ViewManager.ShowView(typeof(MapEditor), vm);
        }

        void newModel()
        {
            var vmXY = new ViewModelXY(1);
            var vmXZ = new ViewModelXY(2);
            var vmYZ = new ViewModelXY(3);

            var vm = new ModelViewModel(vmXY, vmXZ, vmYZ);

            ViewManager.ShowView(typeof(ModelMainMenu), vm);
            ViewManager.ShowView(typeof(ViewXY), vmXY);
            ViewManager.ShowView(typeof(ViewXY), vmXZ);
            ViewManager.ShowView(typeof(ViewXY), vmYZ);
        }
    }
}
