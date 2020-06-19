using Easy_3D_Editor.Resources;
using Easy_3D_Editor.Services;
using Easy_3D_Editor.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
