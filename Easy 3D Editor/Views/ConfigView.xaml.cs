using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Easy_3D_Editor.Views
{
    /// <summary>
    /// Interaktionslogik für ConfigView.xaml
    /// </summary>
    public partial class ConfigView : Window
    {
        public ConfigView()
        {
            InitializeComponent();
        }

        public void CheckInputForNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
