using Easy_3D_Editor.Models;
using Easy_3D_Editor.Services;
using Easy_3D_Editor.Views;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfServices;
using Xml2CSharp;

namespace Easy_3D_Editor.ViewModels
{
    class ModelViewModel : ViewModelBase
    {
        public NotifiingProperty<List<Vertex>> Vertices { get; set; } = new NotifiingProperty<List<Vertex>>();
        public NotifiingProperty<List<Vertex>> Normals { get; set; } = new NotifiingProperty<List<Vertex>>();
        public NotifiingProperty<List<Vertex>> TexturePositions { get; set; } = new NotifiingProperty<List<Vertex>>();
        public NotifiingProperty<List<Flat>> Flats { get; set; } = new NotifiingProperty<List<Flat>>();

        public NotifiingProperty<List<Element>> Elements { get; set; } = new NotifiingProperty<List<Element>>();

        ViewModelXY xy;
        ViewModelXY xz;
        ViewModelXY yz;

        public NotifiingProperty<Image> Image0 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image1 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image2 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image3 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image4 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image5 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image6 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image7 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image8 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image9 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image10 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image11 { get; set; } = new NotifiingProperty<Image>();

        public Command InsertCommand { get; set; } = new Command();
        public Command SelectCommand { get; set; } = new Command();
        public Command SelectSingleCommand { get; set; } = new Command();
        public Command CopyCommand { get; set; } = new Command();
        public Command CubeCommand { get; set; } = new Command();
        public Command NewModelCommand { get; set; } = new Command();
        public Command ListCommand { get; set; } = new Command();
        public Command CloseCommand { get; set; } = new Command();
        public Command SaveCommand { get; set; } = new Command();
        public Command LoadCommand { get; set; } = new Command();
        public Command ExportCommand { get; set; } = new Command();
        public Command MoveCommand { get; set; } = new Command();
        public Command ResizeCommand { get; set; } = new Command();

        public ModelViewModel(ViewModelXY xy, ViewModelXY xz, ViewModelXY yz)
        {
            Elements.Get = new List<Element>();

            this.xy = xy;
            this.xz = xz;
            this.yz = yz;

            InsertCommand.ExecuteFunc = x => insert();
            SelectCommand.ExecuteFunc = x => select();
            SelectSingleCommand.ExecuteFunc = x => selectSingle();
            CopyCommand.ExecuteFunc = x => copy();
            CubeCommand.ExecuteFunc = x => cube();
            NewModelCommand.ExecuteFunc = x => newModel();
            ListCommand.ExecuteFunc = x => list();
            CloseCommand.ExecuteFunc = x => close();
            SaveCommand.ExecuteFunc = x => save();
            LoadCommand.ExecuteFunc = x => load();
            ExportCommand.ExecuteFunc = x => export();
            MoveCommand.ExecuteFunc = x => move();
            ResizeCommand.ExecuteFunc = x => resize();

            Image0.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.einfügen);
            Image1.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.auswahl2);
            Image2.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.auswahl4);
            Image3.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.kopie);
            Image4.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.würfel2);
            Image5.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.würfelplus);
            Image6.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.liste2);
            Image7.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.diskette);
            Image8.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.close);
            Image9.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.export);
            Image10.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.schieben);
            Image11.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.größe_ändern);

            this.xy.DrawCubeAction = drawCube;
            this.xz.DrawCubeAction = drawCube;
            this.yz.DrawCubeAction = drawCube;

            this.xy.NewCubeAction = newCube;
            this.xz.NewCubeAction = newCube;
            this.yz.NewCubeAction = newCube;

            this.xy.MoveAction = move;
            this.xz.MoveAction = move;
            this.yz.MoveAction = move;

            this.xy.ResizeAction = resize;
            this.xz.ResizeAction = resize;
            this.yz.ResizeAction = resize;

            this.xy.AddAction = add;
            this.xz.AddAction = add;
            this.yz.AddAction = add;

            this.xy.SelectAction = select;
            this.xz.SelectAction = select;
            this.yz.SelectAction = select;

            SetPropertyChangeForAll();
        }

        void clearSelection(bool clearAll = true)
        {
            Elements.Get.ForEach(e =>
            {
                if(clearAll)
                    e.IsSelected = false;
                foreach (var point in e.Positions)
                {
                    point.IsSelected = false;
                }
            });
        }

        void select(int xyz, int _x, int _y)
        {
            clearSelection(false);
            var selectedElement = Elements.Get.FirstOrDefault(x => x.IsSelected);
            if (selectedElement == null)
                return;
            List<Position3D> list = null;
            if (xyz == 1)
            {
                list = selectedElement.Positions.Where(p => p.X == _x && p.Y == _y)
                    .ToList();
            }
            else if (xyz == 2)
            {
                list = selectedElement.Positions.Where(p => p.X == _x && p.Z == _y)
                    .ToList();
            }
            else if (xyz == 3)
            {
                list = selectedElement.Positions.Where(p => p.Z == _x && p.Y == _y)
                    .ToList();
            }
            list?.ForEach(point =>
                    {
                        point.IsSelected = true;
                    });
        }

        private void resize()
        {
            xy.ClickMode = CLICK_MODE.RESIZE;
            xz.ClickMode = CLICK_MODE.RESIZE;
            yz.ClickMode = CLICK_MODE.RESIZE;
        }

        private void move()
        {
            xy.ClickMode = CLICK_MODE.MOVE;
            xz.ClickMode = CLICK_MODE.MOVE;
            yz.ClickMode = CLICK_MODE.MOVE;
        }

        void insert()
        {
        }

        void select()
        {
            xy.ClickMode = CLICK_MODE.SELECT_AREA;
            xz.ClickMode = CLICK_MODE.SELECT_AREA;
            yz.ClickMode = CLICK_MODE.SELECT_AREA;
            removeRectangles();
        }

        void selectSingle()
        {
            xy.ClickMode = CLICK_MODE.SELECT_SINGLE;
            xz.ClickMode = CLICK_MODE.SELECT_SINGLE;
            yz.ClickMode = CLICK_MODE.SELECT_SINGLE;
            removeRectangles();
        }

        void copy()
        {

        }

        void cube()
        {
            removeRectangles();
            var vm = new ShapeViewModel();
            ViewManager.ShowDialogView(typeof(ShapeView), vm);

            if (vm.Result == 0)
                return;

            if (vm.Result == 1)
            {
                xy.ClickMode = CLICK_MODE.NEW_CUBE;
                xz.ClickMode = CLICK_MODE.NEW_CUBE;
                yz.ClickMode = CLICK_MODE.NEW_CUBE;
            }

            if (vm.Result == 2)
            {
                xy.ClickMode = CLICK_MODE.NEW_SPHERE;
                xz.ClickMode = CLICK_MODE.NEW_SPHERE;
                yz.ClickMode = CLICK_MODE.NEW_SPHERE;
            }
        }

        void newModel()
        {

        }

        ListViewModel listVm = null;

        void list()
        {
            if (listVm == null)
                listVm = new ListViewModel(Elements.Get.Select(x => x.GetListElement()).ToList(), selectElement, remove);
            ViewManager.ShowView(typeof(Views.ListView), listVm);
        }

        void resetList()
        {
            if (listVm != null)
                listVm.SetList(Elements.Get.Select(x => x.GetListElement()).ToList());
        }

        void selectElement(int id)
        {
            clearSelection();
            Elements.Get.ForEach(x => x.IsSelected = false);
            Elements.Get.First(x => x.Id == id).IsSelected = true;
            removeRectangles();
            setLines();
        }

        void remove(int id)
        {
            Elements.Get = Elements.Get.Where(x => x.Id != id).ToList();
            setLines();
            resetList();
        }

        void export()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie den Dateinamen ein";
            ViewManager.ShowDialogView(typeof(Input), vm);
            if(vm.IsOK)
            {
                var exporter = new WavefrontExporter(vm.Output.Get + ".obj", Elements.Get);
                exporter.Export();
            }
        }

        void save()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie den Dateinamen ein";
            ViewManager.ShowDialogView(typeof(Input), vm);
            if (vm.IsOK)
            {
                var data = new SerializableElements
                {
                    Id = Element.id,
                    Elements = Elements.Get
                };
                Serializer.SerializeBinary(vm.Output.Get + ".model", data);
            }
        }

        void load()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie den Dateinamen ein";
            ViewManager.ShowDialogView(typeof(Input), vm);
            if (vm.IsOK)
            {
                var data = Serializer.DeserializeBinary<SerializableElements>(vm.Output.Get + ".model");
                Element.id = data.Id;
                Elements.Get = data.Elements;
            }
            setLines();
        }

        public void close2()
        {
            xy.Close();
            xz.Close();
            yz.Close();
        }

        public void close()
        {
            xy.Close();
            xz.Close();
            yz.Close();
            Close();
        }

        bool isNewCube = false;
        int cube_x;
        int cube_y;
        int cube_z;
        int cube_width;
        int cube_height;
        int cube_depth;

        void newCube(int caller, int x, int y, int width, int height, int raster)
        {
            isNewCube = true;
            if (caller == 1)
            {
                cube_x = x - x % raster;
                cube_y = y - y % raster;
                cube_z = raster;
                cube_width = width - width % raster;
                cube_height = height - height % raster;
                cube_depth = raster;
            }
            else if (caller == 2)
            {
                cube_x = x - x % raster;
                cube_y = raster;
                cube_z = y - y % raster;
                cube_width = width - width % raster;
                cube_height = raster;
                cube_depth = height - height % raster;
            }
            else if (caller == 3)
            {
                cube_x = raster;
                cube_y = y - y % raster;
                cube_z = x - x % raster;
                cube_width = raster;
                cube_height = height - height % raster;
                cube_depth = width - width % raster;
            }
            drawRectangles(raster);
        }

        void drawRectangles(int raster)
        {
            xy.DrawRectangle(cube_x, cube_y, cube_width, cube_height, raster);
            xz.DrawRectangle(cube_x, cube_z, cube_width, cube_depth, raster);
            yz.DrawRectangle(cube_z, cube_y, cube_depth, cube_height, raster);
        }

        void removeRectangles()
        {
            xy.removeRectangle();
            xz.removeRectangle();
            yz.removeRectangle();
            isNewCube = false;
        }

        void move(int caller, int x, int y, int raster)
        {
            if (isNewCube)
            {
                if (caller == 1)
                {
                    cube_x -= x;
                    cube_y -= y;
                    cube_x = cube_x / raster * raster;
                    cube_y = cube_y / raster * raster;
                }
                else if (caller == 2)
                {
                    cube_x -= x;
                    cube_z -= y;
                    cube_x = cube_x / raster * raster;
                    cube_z = cube_z / raster * raster;
                }
                else if (caller == 3)
                {
                    cube_y -= y;
                    cube_z -= x;
                    cube_y = cube_y / raster * raster;
                    cube_z = cube_z / raster * raster;
                }
                drawRectangles(raster);
            }
            else
            {
                if (caller == 1)
                {
                    moveSelected(x, y, 0, raster);
                }
                else if (caller == 2)
                {
                    moveSelected(x, 0, y, raster);
                }
                else if (caller == 3)
                {
                    moveSelected(0, y, x, raster);
                }
            }
        }

        void moveSelected(int x, int y, int z, int raster)
        {
            Elements.Get.ForEach(e =>
            {
                if (e.IsSelected)
                {
                    var isAllSelected = !e.Positions.Any(p => p.IsSelected);
                    foreach (var posi in e.Positions)
                    {
                        if (isAllSelected || posi.IsSelected)
                        {
                            posi.X -= x;
                            posi.Y -= y;
                            posi.Z -= z;
                            posi.X = posi.X / raster * raster;
                            posi.Y = posi.Y / raster * raster;
                            posi.Z = posi.Z / raster * raster;
                        }
                    }
                }
            });
            setLines();
        }

        void resize(int caller, int side, int x, int y, int raster)
        {
            int _x = 0;
            int _y = 0;
            int width = 0;
            int height = 0;


            if (side == 0)
            {
                _x = x;
                _y = y;
                width = -x;
                height = -y;
            }
            else if (side == 1)
            {
                _y = y;
                width = x;
                height = -y;
            }
            else if (side == 2)
            {
                _x = x;
                width = -x;
                height = y;
            }
            else if (side == 3)
            {
                width = x;
                height = y;
            }

            if (caller == 1)
            {
                cube_x -= _x;
                cube_y -= _y;
                cube_width -= width;
                cube_height -= height;
            }
            else if (caller == 2)
            {
                cube_x -= _x;
                cube_z -= _y;
                cube_depth -= height;
                cube_width -= width;
            }
            else if (caller == 3)
            {
                cube_y -= _y;
                cube_z -= _x;
                cube_depth -= width;
                cube_height -= height;
            }
            round(raster);

            if (cube_width <= 0)
                cube_width = raster;
            if (cube_height <= 0)
                cube_height = raster;
            if (cube_depth <= 0)
                cube_depth = raster;

            drawRectangles(raster);
        }

        void round(int raster)
        {
            cube_x = cube_x / raster * raster;
            cube_y = cube_y / raster * raster;
            cube_z = cube_z / raster * raster;
            cube_width = cube_width / raster * raster;
            cube_height = cube_height / raster * raster;
            cube_depth = cube_depth / raster * raster;
            if (cube_x % raster > raster / 2)
                cube_x += raster;
            if (cube_y % raster > raster / 2)
                cube_y += raster;
            if (cube_z % raster > raster / 2)
                cube_z += raster;
            if (cube_width % raster > raster / 2)
                cube_width += raster;
            if (cube_height % raster > raster / 2)
                cube_height += raster;
            if (cube_depth % raster > raster / 2)
                cube_depth += raster;
        }

        void add()
        {
            if (xy.ClickMode == CLICK_MODE.NEW_CUBE)
                addCube();
            else if (xy.ClickMode == CLICK_MODE.NEW_SPHERE)
                addSphere();

            resetList();
            removeRectangles();
            setLines();
        }

        void addSphere()
        {
            List<int> l = new List<int>();
            List<float> l2 = new List<float>();

            int level = 11;
            var sphere = new Sphere(level);

            float middleX = (float)cube_x + (cube_width / 2.0f);
            float middleY = (float)cube_y + (cube_height / 2.0f);
            float middleZ = (float)cube_z + (cube_depth / 2.0f);

            float alpha = (float)(2 * Math.PI / (sphere.positionPerLevelCount));

            int f = 2;

            sphere.Positions[0].X = middleX;
            sphere.Positions[0].Y = cube_y + cube_height;
            sphere.Positions[0].Z = middleZ;

            sphere.Positions[sphere.positionCount - 1].X = middleX;
            sphere.Positions[sphere.positionCount - 1].Y = cube_y;
            sphere.Positions[sphere.positionCount - 1].Z = middleZ;

            var curLvl = 1;
            for (int i = level - 1; i > 1; --i)
            {
                var a = alpha * curLvl;// + alpha;
                var y = Math.Cos(a) * (cube_height / 2.0f);
                l2.Add((float)y);
                var radius = Math.Sin(a) * (cube_width / 2.0f);

                for (int j = 0; j < sphere.positionPerLevelCount; j++)
                {
                    a = alpha * j;
                    var x = Math.Cos(a) * radius;
                    var z = Math.Sin(a) * radius;

                    var id = (curLvl - 1) * sphere.positionPerLevelCount + j + 1;

                    sphere.Positions[id].X = (float)x + middleX;
                    sphere.Positions[id].Y = (float)y + middleY;
                    sphere.Positions[id].Z = (float)z + middleZ;
                    f++;
                    l.Add(id);
                }
                curLvl++;
            }
            Elements.Get.Add(sphere);
        }

        void addCube()
        {
            var cube = new Cube();

            cube.Positions[0].X = cube_x;
            cube.Positions[0].Y = cube_y;
            cube.Positions[0].Z = cube_z;

            cube.Positions[1].X = cube_x + cube_width;
            cube.Positions[1].Y = cube_y;
            cube.Positions[1].Z = cube_z;

            cube.Positions[2].X = cube_x + cube_width;
            cube.Positions[2].Y = cube_y + cube_height;
            cube.Positions[2].Z = cube_z;

            cube.Positions[3].X = cube_x;
            cube.Positions[3].Y = cube_y + cube_height;
            cube.Positions[3].Z = cube_z;

            cube.Positions[4].X = cube_x;
            cube.Positions[4].Y = cube_y;
            cube.Positions[4].Z = cube_z + cube_depth;

            cube.Positions[5].X = cube_x + cube_width;
            cube.Positions[5].Y = cube_y;
            cube.Positions[5].Z = cube_z + cube_depth;

            cube.Positions[6].X = cube_x + cube_width;
            cube.Positions[6].Y = cube_y + cube_height;
            cube.Positions[6].Z = cube_z + cube_depth;

            cube.Positions[7].X = cube_x;
            cube.Positions[7].Y = cube_y + cube_height;
            cube.Positions[7].Z = cube_z + cube_depth;

            Elements.Get.Add(cube);
        }

        Line createLine(Element element, int i, int j, int xyz)
        {
            if (xyz == 1)
            {
                return new Line
                {
                    X1 = element.Positions[i].X,
                    Y1 = element.Positions[i].Y,
                    X2 = element.Positions[j].X,
                    Y2 = element.Positions[j].Y
                };
            }
            else if (xyz == 2)
            {
                return new Line
                {
                    X1 = element.Positions[i].X,
                    Y1 = element.Positions[i].Z,
                    X2 = element.Positions[j].X,
                    Y2 = element.Positions[j].Z
                };
            }
            else if (xyz == 3)
            {
                return new Line
                {
                    X1 = element.Positions[i].Z,
                    Y1 = element.Positions[i].Y,
                    X2 = element.Positions[j].Z,
                    Y2 = element.Positions[j].Y
                };
            }
            return null;
        }

        List<KeyValuePair<Shape, Brush>> addLines(Element element, int xyz)
        {
            List<KeyValuePair<Shape, Brush>> lines = new List<KeyValuePair<Shape, Brush>>();

            var brush = element.IsSelected ? Brushes.Green : Brushes.Blue;

            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 0, 1, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 1, 2, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 2, 3, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 3, 0, xyz), brush));

            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 4, 5, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 5, 6, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 6, 7, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 7, 4, xyz), brush));

            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 0, 4, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 1, 5, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 2, 6, xyz), brush));
            lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 3, 7, xyz), brush));

            return lines;
        }

        void setLines()
        {
            xy.Lines = new List<KeyValuePair<Shape, Brush>>();
            xz.Lines = new List<KeyValuePair<Shape, Brush>>();
            yz.Lines = new List<KeyValuePair<Shape, Brush>>();

            var lines1 = new List<KeyValuePair<Shape, Brush>>();
            var lines2 = new List<KeyValuePair<Shape, Brush>>();
            var lines3 = new List<KeyValuePair<Shape, Brush>>();

            var lines21 = new List<KeyValuePair<Shape, Brush>>();
            var lines22 = new List<KeyValuePair<Shape, Brush>>();
            var lines23 = new List<KeyValuePair<Shape, Brush>>();

            foreach (var cube in Elements.Get)
            {
                if (cube.IsSelected)
                {
                    lines21.AddRange(addLines(cube, 1));
                    lines22.AddRange(addLines(cube, 2));
                    lines23.AddRange(addLines(cube, 3));
                }
                else
                {
                    lines1.AddRange(addLines(cube, 1));
                    lines2.AddRange(addLines(cube, 2));
                    lines3.AddRange(addLines(cube, 3));
                }
            }
            xy.Lines.AddRange(lines1);
            xz.Lines.AddRange(lines2);
            yz.Lines.AddRange(lines3);
            xy.Lines.AddRange(lines21);
            xz.Lines.AddRange(lines22);
            yz.Lines.AddRange(lines23);

            xy.LoadCan();
            xz.LoadCan();
            yz.LoadCan();
        }

        void drawCube(float sx, float sy, float sz, float x, float y, float z)
        {
        }
    }
}
