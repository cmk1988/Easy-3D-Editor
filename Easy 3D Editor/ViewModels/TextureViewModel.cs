using Easy_3D_Editor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfServices;
using static Easy_3D_Editor.Services.WavefrontExporter;

namespace Easy_3D_Editor.ViewModels
{
    class TextureViewModel : ViewModelBase
    {
        public NotifiingProperty<int> X { get; set; } = new NotifiingProperty<int>(200);
        public NotifiingProperty<int> Y { get; set; } = new NotifiingProperty<int>(200);

        public NotifiingProperty<Image> TextureImage { get; set; } = new NotifiingProperty<Image>();

        public System.Drawing.Image Image;


        public Canvas Can { get; set; }

        public TextureForFlat TextureForFlat { get; private set; }
        public bool IsOK { get; private set; }

        bool isMoveInProgress = false;
        int selectedIndex = -1;
        FlatWithPositions flat;

        Point oldPoint;

        Command OkCommand { get; set; }
        Command CancelCommand { get; set; }

        public void ClearSelectedEdge()
        {
            selectedIndex = -1;
        }

        public void ResetEdges()
        {
            var thirdX = Can.ActualWidth / 3.0f;
            var thirdY = Can.ActualHeight / 3.0f;
            this.TextureForFlat.Coordinates.Clear();
            this.TextureForFlat.Coordinates.Add(new TextureCoordinate
            {
                X = (float)thirdX,
                Y = (float)thirdY
            });
            this.TextureForFlat.Coordinates.Add(new TextureCoordinate
            {
                X = (float)thirdX * 2.0f,
                Y = (float)thirdY
            });
            this.TextureForFlat.Coordinates.Add(new TextureCoordinate
            {
                X = (float)thirdX * 2.0f,
                Y = (float)thirdY * 2.0f
            });
            if (flat.Positions.Count == 4)
                this.TextureForFlat.Coordinates.Add(new TextureCoordinate
                {
                    X = (float)thirdX,
                    Y = (float)thirdY * 2.0f
                });
            renderLines();
        }

        public void SetEvents()
        {
            Can.MouseLeftButtonDown += (a, b) =>
            {
                isMoveInProgress = true;
                oldPoint = b.GetPosition(Can);
            };

            Can.MouseMove += (a, b) =>
            {
                if(isMoveInProgress)
                {
                    if(b.LeftButton != MouseButtonState.Pressed)
                    {
                        isMoveInProgress = false;
                        return;
                    }
                    var posi = b.GetPosition(Can);

                    var x = oldPoint.X - posi.X;
                    var y = oldPoint.Y - posi.Y;
                    if (selectedIndex < 0)
                    {
                        foreach (var p in this.TextureForFlat.Coordinates)
                        {
                            p.X -= (float)x;
                            p.Y -= (float)y;
                        }
                    }
                    else
                    {
                        this.TextureForFlat.Coordinates[selectedIndex].X -= (float)x;
                        this.TextureForFlat.Coordinates[selectedIndex].Y -= (float)y;
                    }
                    renderLines();
                    oldPoint = posi;
                }
            };

            Can.MouseRightButtonUp += (a, b) =>
            {
                var posi = b.GetPosition(Can);

                double d = double.MaxValue;

                int i = 0;
                foreach (var position in this.TextureForFlat.Coordinates)
                {
                    var _a = position.X - posi.X;
                    var _b = position.Y - posi.Y;
                    var dist = Math.Sqrt(_a * _a + _b * _b); 
                    if (dist < d)
                    {
                        selectedIndex = i;
                        d = dist;
                    }
                    ++i;
                }
            };
            ResetEdges();
        }

        public TextureViewModel(string filename, FlatWithPositions flat)
        {
            this.TextureForFlat = new TextureForFlat
            {
                FlatId = flat.FlatId
            };

            this.flat = flat;

            OkCommand = new Command
            {
                ExecuteFunc = x =>
                {
                    IsOK = true;
                    Close();
                }
            };

            CancelCommand = new Command
            {
                ExecuteFunc = x =>
                {
                    IsOK = false;
                    Close();
                }
            };

            SetPropertyChangeForAll();
            Image = System.Drawing.Image.FromFile(filename);
            TextureImage.Get = ImageHelper.ConvertImageToWpfImage(Image);
        }

        void renderLines()
        {
            Can.Children.Clear();
            var lines = new List<Line>();

            lines.Add(new Line 
            {
                X1 = this.TextureForFlat.Coordinates[0].X,
                Y1 = this.TextureForFlat.Coordinates[0].Y,
                X2 = this.TextureForFlat.Coordinates[this.TextureForFlat.Coordinates.Count - 1].X,
                Y2 = this.TextureForFlat.Coordinates[this.TextureForFlat.Coordinates.Count - 1].Y,
                Stroke = Brushes.White,
                StrokeThickness = 3
            });
            lines.Add(new Line
            {
                X1 = this.TextureForFlat.Coordinates[0].X,
                Y1 = this.TextureForFlat.Coordinates[0].Y,
                X2 = this.TextureForFlat.Coordinates[this.TextureForFlat.Coordinates.Count - 1].X,
                Y2 = this.TextureForFlat.Coordinates[this.TextureForFlat.Coordinates.Count - 1].Y,
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 1
            });

            for (int i = 0; i < this.TextureForFlat.Coordinates.Count - 1; i++)
            {
                lines.Add(new Line
                {
                    X1 = this.TextureForFlat.Coordinates[i].X,
                    Y1 = this.TextureForFlat.Coordinates[i].Y,
                    X2 = this.TextureForFlat.Coordinates[i + 1].X,
                    Y2 = this.TextureForFlat.Coordinates[i + 1].Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 3
                });
                lines.Add(new Line
                {
                    X1 = this.TextureForFlat.Coordinates[i].X,
                    Y1 = this.TextureForFlat.Coordinates[i].Y,
                    X2 = this.TextureForFlat.Coordinates[i + 1].X,
                    Y2 = this.TextureForFlat.Coordinates[i + 1].Y,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 1
                });
            }

            foreach (var line in lines)
                Can.Children.Add(line);
        }
    }
}
