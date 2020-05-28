using Easy_3D_Editor.Resources;
using Easy_3D_Editor.Services;
using Easy_3D_Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaktionslogik für MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Window
    {

        public MapEditor()
        {
            InitializeComponent();
            Loaded += (l, m) =>
            {
                var x = ((MapEditorViewModel)DataContext).X;
                var y = ((MapEditorViewModel)DataContext).Y;

                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        var img = ImageHelper.ConvertImageToWpfImage(Resource.X);
                        img.Width = 30.0f;
                        img.Height = 30.0f;
                        img.MouseRightButtonDown += (g, h) =>
                        {
                            
                        };

                        Canvas.SetLeft(img, i * 30);
                        Canvas.SetTop(img, j * 30);
                        can.Children.Add(img);
                    }
                }
            };
        }


    }
}
