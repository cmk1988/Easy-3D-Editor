using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class MapEditorViewModel : ViewModelBase
    {
        public int X { get; }
        public int Y { get; }

        List<List<int>> map = new List<List<int>>(); 

        public MapEditorViewModel(int x, int y)
        {
            X = x;
            Y = y;

            for (int i = 0; i < y; i++)
            {
                var m = new List<int>();
                for (int j = 0; j < x; j++)
                {
                    m.Add(0);
                }
                map.Add(m);
            }
        }


    }
}
