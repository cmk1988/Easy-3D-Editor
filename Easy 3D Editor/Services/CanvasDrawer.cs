using System.Collections.Generic;
using System.Windows.Shapes;

namespace Easy_3D_Editor.Services
{
    class CanvasDrawer
    {
        public static List<Line> DrawGrid(int size, int x, int y)
        {
            var lines = new List<Line>();
            int _x, _y;
            _x = x / size;
            _y = y / size;
            for (int i = 0; i<_x; ++i)
            {
                Line line = new Line();
                line.X1 = i * size;
                line.X2 = i * size;
                line.Y1 = 0;
                line.Y2 = y;
                if(i == _x / 2)
                    line.Stroke = System.Windows.Media.Brushes.Gray;
                else
                    line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                lines.Add(line);
            }
            for (int j = 0; j < _y; ++j)
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = x;
                line.Y1 = j * size;
                line.Y2 = j * size;
                if (j == _y / 2)
                    line.Stroke = System.Windows.Media.Brushes.Gray;
                else
                    line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lines.Add(line);
            }
            return lines;
        }
    }
}
