using System.Windows;
using System.Windows.Input;

namespace Easy_3D_Editor.ViewModels
{
    /// <summary>
    /// Interaktionslogik für TextureView.xaml
    /// </summary>
    public partial class TextureView : Window
    {
        public TextureView()
        {
            InitializeComponent();

            Loaded += (x, y) =>
            {
                ((TextureViewModel)DataContext).Can = can;
                ((TextureViewModel)DataContext).SetEvents();
                this.KeyDown += (a, b) =>
                {
                    if(b.Key == Key.Escape)
                        ((TextureViewModel)DataContext).ClearSelectedEdge();
                    if (b.Key == Key.Space)
                        ((TextureViewModel)DataContext).ResetEdges();
                };
            };
        }
    }
}
