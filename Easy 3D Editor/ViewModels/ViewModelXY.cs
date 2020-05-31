using Easy_3D_Editor.Services;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfServices;
using static WpfServices.ViewModelBase;

namespace Easy_3D_Editor.ViewModels
{

    enum CLICK_MODE
    {
        NEW_CUBE,
        NEW_SPHERE,
        SELECT_AREA,
        SELECT_SINGLE,
        RESIZE,
        MOVE
    }

    class ViewModelXY : ViewModelBase
    {
        public CLICK_MODE ClickMode { get; set; } = CLICK_MODE.SELECT_SINGLE;

        public float fRaster { get; set; } = 16.0f;
        public int Raster => (int)fRaster;

        public NotifiingProperty<Canvas> Can { get; set; } = new NotifiingProperty<Canvas>();
        public NotifiingProperty<List<Line>> Canvas { get; set; } = new NotifiingProperty<List<Line>>();

        public Action<float, float, float, float, float, float> DrawCubeAction { get; set; }

        public Action<int, int, int, int, int, int> NewCubeAction { get; set; }
        public Action<int, int, int, int> MoveAction { get; set; }
        public Action<int, int, int, int, int> ResizeAction { get; set; }
        public Action AddAction { get; set; }
        public Action<int, int, int> SelectAction { get; set; }

        public Command NewCube { get; set; } = new Command();
        public Command RasterPlus { get; set; } = new Command();
        public Command RasterMinus { get; set; } = new Command();
        public Command OK { get; set; } = new Command();

        public NotifiingProperty<string> Title { get; set; } = new NotifiingProperty<string>();

        public List<KeyValuePair<Shape, Brush>> Lines { get; set; } = new List<KeyValuePair<Shape, Brush>>();

        int xyz;

        public ViewModelXY(int xyz)
        {
            this.xyz = xyz;
            if(xyz == 1)
                Title.Get = "XY";
            else if (xyz == 2)
                Title.Get = "XZ";
            else if (xyz == 3)
                Title.Get = "ZY";
            NewCube.ExecuteFunc = x => newCube();
            RasterPlus.ExecuteFunc = x => rasterPlus();
            RasterMinus.ExecuteFunc = x => rasterMinus();
            OK.ExecuteFunc = x => AddAction();
            SetPropertyChangeForAll();
        }

        bool loaded = false;

        System.Windows.Point lastPoint;
        
        void rasterPlus()
        {
            if (fRaster < 64.0f)
                fRaster *= 2;
            LoadCan(x, y);
        }

        void rasterMinus()
        {
            if(fRaster > 2.0f)
                fRaster /= 2;
            LoadCan(x, y);
        }

        int x, y;

        TextBlock X = new TextBlock();

        System.Windows.Shapes.Rectangle rectangle = null;

        public void DrawShapes()
        {
            foreach (var line in Lines)
            {
                Can.Get.Children.Add(line.Key);
                line.Key.Stroke = line.Value;
            }
        }

        public void DrawRectangle(int _x, int _y, int width, int height, int raster = 0)
        {
            if (raster != 0)
            {
                _x = _x - _x % raster;
                _y = _y - _y % raster;
                width = width - width % raster;
                height = height - height % raster;
            }
            rectangle = new System.Windows.Shapes.Rectangle();
            rectangle.Stroke = System.Windows.Media.Brushes.Red;
            rectangle.StrokeThickness = 2;
            System.Windows.Controls.Canvas.SetLeft(rectangle, _x);
            System.Windows.Controls.Canvas.SetTop(rectangle, _y);
            rectangle.Width = width;
            rectangle.Height = height;
            LoadCan(x, y);
        }

        public void LoadCan()
        {
            LoadCan(x, y);
        }

        public void LoadCan(int x, int y)
        {
            this.x = x;
            this.y = y;

            Can.Get.Children.Clear();

            var lines = CanvasDrawer.DrawGrid(Raster, x, y);

            foreach (var line in lines)
                Can.Get.Children.Add(line);

            DrawShapes();

            Can.Get.Children.Add(X);

            if(rectangle != null)
            {
                Can.Get.Children.Add(rectangle);
            }

            if (!loaded)
            {
                Can.Get.MouseLeftButtonDown += (a, b) =>
                {
                    if(ClickMode == CLICK_MODE.SELECT_AREA 
                        || ClickMode == CLICK_MODE.MOVE
                        || ClickMode == CLICK_MODE.RESIZE)
                    {
                        if(!isStarted)
                            startPosition = b.GetPosition(Can.Get);
                        isStarted = true;
                    }
                    else if (ClickMode == CLICK_MODE.SELECT_SINGLE)
                    {
                        select(b);
                    }
                    else if (ClickMode == CLICK_MODE.NEW_CUBE ||
                        ClickMode == CLICK_MODE.NEW_SPHERE)
                    {
                        if (!isStarted)
                            startPosition = b.GetPosition(Can.Get);
                        isStarted = true;
                    }
                };

                Can.Get.MouseMove += (a, b) =>
                {
                    if (isStarted)
                    {
                        if (ClickMode == CLICK_MODE.SELECT_AREA)
                        {
                            selectArea(b);
                        }
                        else if (ClickMode == CLICK_MODE.NEW_CUBE ||
                        ClickMode == CLICK_MODE.NEW_SPHERE)
                        {
                            cube(b);
                        }
                        else if (ClickMode == CLICK_MODE.MOVE)
                        {
                            move(b);
                        }
                        else if (ClickMode == CLICK_MODE.RESIZE)
                        {
                            resize(b);
                        }
                    }
                };

                Can.Get.MouseRightButtonDown += (a, b) =>
                {
                    lastPoint = b.GetPosition(Can.Get);
                    var posi = System.Windows.Forms.Control.MousePosition;
                    ViewManager.ShowView(typeof(EditorMenu), this, posi.X - 10, posi.Y - 10);
                };

                loaded = true;
            }
        }

        System.Windows.Point startPosition;
        bool isStarted = false;

        public void select(MouseButtonEventArgs e)
        {
            var posi = e.GetPosition(Can.Get);
            X.Foreground = System.Windows.Media.Brushes.Black;
            X.Text = "X";
            int _x = (int)(posi.X);
            int _y = (int)(posi.Y);
            _x = _x - _x % Raster - 3;
            _y = _y - _y % Raster - 10;
            System.Windows.Controls.Canvas.SetTop(X, _y);
            System.Windows.Controls.Canvas.SetLeft(X, _x);
            int xx = (int)(posi.X);
            int yy = (int)(posi.Y);
            xx = xx / Raster * Raster;
            yy = yy / Raster * Raster;
            SelectAction(xyz, xx, yy);
        }

        public void removeRectangle()
        {
            rectangle = null;
        }

        void move(System.Windows.Input.MouseEventArgs e)
        {
            if (!isStarted)
                return;
            var posi = e.GetPosition(Can.Get);
            int _x = (int)(startPosition.X - posi.X);
            int _y = (int)(startPosition.Y - posi.Y);
            startPosition = posi;
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                rectangle = null;
                isStarted = false;
                MoveAction(xyz, _x, _y, Raster);
                return;
            }
            MoveAction(xyz, _x, _y, 1);
        }

        int side = 0;
        void resize(System.Windows.Input.MouseEventArgs e)
        {
            if (!isStarted)
                return;
            var posi = e.GetPosition(Can.Get);
            if (side == 0)
            {

                var xx = (int)posi.X / (x / 2);
                var yy = (int)posi.Y / (y / 2);
                side = xx + yy * 2;
            }
            int _x = (int)(startPosition.X - posi.X);
            int _y = (int)(startPosition.Y - posi.Y);
            startPosition = posi;
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                ResizeAction(xyz, side, _x, _y, Raster);
                rectangle = null;
                isStarted = false;
                side = 0;
                return;
            }
            ResizeAction(xyz, side, _x, _y, 1);
        }

        public void selectArea(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                rectangle = null;
                isStarted = false;
                return;
            }
            if (!isStarted)
                return;
            var posi = e.GetPosition(Can.Get);
            var width = (int)(startPosition.X - posi.X);
            var height = (int)(startPosition.Y - posi.Y);
            int _x;
            int _y;
            if (width == 0 || height == 0)
            {
                rectangle = null;
                return;
            }
            if (width < 0)
            {
                width = (int)(posi.X - startPosition.X);
                _x = (int)startPosition.X;
            }
            else
            {
                _x = (int)posi.X;
            }
            if (height < 0)
            {
                height = (int)(posi.Y - startPosition.Y);
                _y = (int)startPosition.Y;
            }
            else
            {
                _y = (int)posi.Y;
            }
            DrawRectangle(_x, _y, width, height);
        }

        public void cube(System.Windows.Input.MouseEventArgs e)
        {
            if (!isStarted)
                return;
            var posi = e.GetPosition(Can.Get);
            var width = (int)(startPosition.X - posi.X);
            var height = (int)(startPosition.Y - posi.Y);
            int _x;
            int _y;
            if (width == 0 || height == 0)
            {
                rectangle = null;
                return;
            }
            if (width < 0)
            {
                width = (int)(posi.X - startPosition.X);
                _x = (int)(startPosition.X - startPosition.X % Raster);
            }
            else
            {
                _x = (int)(posi.X - posi.X % Raster);
            }
            if (height < 0)
            {
                height = (int)(posi.Y - startPosition.Y);
                _y = (int)(startPosition.Y - startPosition.Y % Raster);
            }
            else
            {
                _y = (int)(posi.Y - posi.Y % Raster);
            }
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                isStarted = false;
                return;
            }
            width = width - width % Raster;
            height = height - height % Raster;
            //DrawRectangle(_x, _y, width, height);
            NewCubeAction(xyz, _x, _y, width, height, Raster);
        }

        public void DrawCube(int x, int y, int z)
        {

        }

        void newCube()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie die Größe an (x,y,z)";
            ViewManager.ShowDialogView(typeof(Input), vm);
            if(vm.IsOK)
            {
                var split = vm.Output.Get.Split(',').Select(x => float.Parse(x)).ToList();
                if(split.Count == 3)
                {
                    DrawCubeAction((float)lastPoint.X, (float)lastPoint.Y, 0.0f, split[0], split[1], split[2]);
                }
            }
        }
    }
}
