using Easy_3D_Editor.ViewModels;
using Easy_3D_Editor.Views;
using System.Windows;
using WpfServices;

namespace Easy_3D_Editor
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {

        public void StartUp(object sender, StartupEventArgs args)
        {
            var vm = new MainMenuViewModel();
            ViewManager.ShowDialogRootView(typeof(MainMenu), vm);
        }
    }
}
