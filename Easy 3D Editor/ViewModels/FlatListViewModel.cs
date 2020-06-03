using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;
using static Easy_3D_Editor.Services.WavefrontExporter;

namespace Easy_3D_Editor.ViewModels
{
    class FlatListViewModel : ViewModelBase
    {
        public NotifiingProperty<FlatWithPositions> SelectedItem { get; set; }
    }
}
