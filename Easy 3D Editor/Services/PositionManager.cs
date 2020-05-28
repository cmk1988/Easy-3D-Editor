using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Services
{
    [Serializable]
    public class WindowPositions
    {
        public List<WindowPosition> Positions { get; set; }
    }

    [Serializable]
    public class WindowPosition
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    class PositionManager
    {
        WindowPositions positions;
        public static PositionManager Instance { get; } = new PositionManager();

        public PositionManager()
        {
            if(File.Exists("positions.dat"))
            {
                positions = Serializer.DeserializeBinary<WindowPositions>("positions.dat");
            }
            else
            {
                positions = new WindowPositions { Positions = new List<WindowPosition>() };
            }
        }

        public WindowPosition GetPosition(string name)
        {
            return positions.Positions.FirstOrDefault(x => x.Name == name);
        }

        public void SetPosition(string name, int x, int y)
        {
            var posi = positions.Positions.FirstOrDefault(a => a.Name == name);
            if(posi != null)
            {
                posi.X = x;
                posi.Y = y;
            }
            else
            {
                positions.Positions.Add(new WindowPosition
                {
                    Name = name,
                    X = x,
                    Y = y
                });
            }
            Serializer.SerializeBinary("positions.dat", positions);
        }
    }
}
