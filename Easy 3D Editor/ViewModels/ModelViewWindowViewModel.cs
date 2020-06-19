using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ModelViewWindowViewModel : ViewModelBase
    {
        public ModelViewer Engine { get; set; }

        public ModelViewWindowViewModel()
        {
            SetPropertyChangeForAll();
        }
    }
}
