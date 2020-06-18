using Easy_3D_Editor.Services;
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
            };
        }
    }
}
