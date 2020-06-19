using Easy_3D_Editor.Services;
using Easy_3D_Editor.ViewModels;
using System.Windows;

namespace Easy_3D_Editor.Views
{
    /// <summary>
    /// Interaktionslogik für ModelViewWindow.xaml
    /// </summary>
    public partial class ModelViewWindow : Window
    {
        public ModelViewWindow()
        {
            InitializeComponent();
            Closing += (s, e) =>
            {
                PositionManager.Instance.SetPosition(Title, (int)Left, (int)Top);
            };
            Loaded += (x, y) =>
            {
                var posi = PositionManager.Instance.GetPosition(Title);
                if (posi != null)
                {
                    Left = posi.X;
                    Top = posi.Y;
                }

                var vm = (ModelViewWindowViewModel)DataContext;
                MouseMove += vm.MouseMove_Event;
                MouseLeftButtonDown += vm.MouseLeftDown_Event;
                MouseRightButtonDown += vm.MouseRightDown_Event;
                MouseWheel += vm.MouseWheel_Event;
                MouseLeftButtonUp += vm.MouseLeftUp_Event;
            };
        }
    }
}
