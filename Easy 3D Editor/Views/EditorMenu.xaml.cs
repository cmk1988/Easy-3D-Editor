using System.Windows;

namespace Easy_3D_Editor.ViewModels
{
    /// <summary>
    /// Interaktionslogik für EditorMenu.xaml
    /// </summary>
    public partial class EditorMenu : Window
    {
        public EditorMenu()
        {
            InitializeComponent();
            Deactivated += (x, y) =>
            {
                Close();
            };
        }
    }
}
