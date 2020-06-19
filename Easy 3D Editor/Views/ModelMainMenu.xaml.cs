using Easy_3D_Editor.Services;
using Easy_3D_Editor.ViewModels;
using System.Windows;
using System.Windows.Input;
using WpfServices;

namespace Easy_3D_Editor.Views
{
    /// <summary>
    /// Interaktionslogik für ModelMainMenu.xaml
    /// </summary>
    public partial class ModelMainMenu : Window
    {
        public ModelMainMenu()
        {            
            InitializeComponent();
            Loaded += (x, y) =>
            {
                var posi = PositionManager.Instance.GetPosition(Title);
                if (posi != null)
                {
                    Left = posi.X;
                    Top = posi.Y;
                }
            };
            ViewManager.RootView.WindowState = WindowState.Minimized;
            //this.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Window_MouseDown), true);
            Closing += (x, y) =>
            {
                ((ModelViewModel)DataContext).close2();
                ViewManager.RootView.WindowState = WindowState.Normal;
                PositionManager.Instance.SetPosition(Title, (int)Left, (int)Top);
            };
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
