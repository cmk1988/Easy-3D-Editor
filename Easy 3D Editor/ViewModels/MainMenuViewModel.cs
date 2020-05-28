using Easy_3D_Editor.Services;
using Easy_3D_Editor.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;
using WpfServices;
using Xml2CSharp;

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
            SetPropertyChangeForAll();
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
