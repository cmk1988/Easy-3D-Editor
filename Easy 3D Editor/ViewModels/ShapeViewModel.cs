using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ShapeViewModel : ViewModelBase
    {
        public Command CubeCommand { get; set; } = new Command();
        public Command SphereCommand { get; set; } = new Command();
        public Command AbbortCommand { get; set; } = new Command();

        public ShapeViewModel()
        {
            CubeCommand.ExecuteFunc = x => setResult(1);
            SphereCommand.ExecuteFunc = x => setResult(2);
            AbbortCommand.ExecuteFunc = x => setResult(0);
        }

        void setResult(int result)
        {
            Result = result;
            Close();
        }

        public int Result = 0;
    }
}
