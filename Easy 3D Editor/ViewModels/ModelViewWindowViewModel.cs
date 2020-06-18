using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
