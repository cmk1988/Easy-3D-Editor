using Easy_3D_Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            ViewManager.RootView.Visibility = Visibility.Hidden;
            //this.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Window_MouseDown), true);
            Closing += (x, y) =>
            {
                ((ModelViewModel)DataContext).close2();
                ViewManager.RootView.Visibility = Visibility.Visible;
            };
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
