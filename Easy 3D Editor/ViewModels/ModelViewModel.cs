using Easy_3D_Editor.Models;
using Easy_3D_Editor.Services;
using Easy_3D_Editor.Views;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfServices;
using Xml2CSharp;
using static Easy_3D_Editor.Services.WavefrontExporter;

namespace Easy_3D_Editor.ViewModels
{
    class ModelViewModel : ViewModelBase
    {
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
        public NotifiingProperty<Image> Image12 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image13 { get; set; } = new NotifiingProperty<Image>();
        public NotifiingProperty<Image> Image14 { get; set; } = new NotifiingProperty<Image>();

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
        public Command BoneCommand { get; set; } = new Command();
        public Command TextureCommand { get; set; } = new Command();

        WavefrontExporter exporter;

        bool isMapElement = false;
        int mapSizeX = 0;
        int mapSizeY = 0;

        public Skeletton Skeletton { get; set; } = new Skeletton();

        CLICK_MODE lastClickMode = CLICK_MODE.SELECT_AREA;

        ModelViewer engine;

        public ModelViewModel(ViewModelXY xy, ViewModelXY xz, ViewModelXY yz, int sizeX = 0, int sizeY = 0)
        {
            if(sizeX != 0 && sizeY != 0)
            {
                isMapElement = true;
                mapSizeX = sizeX;
                mapSizeY = sizeY;
            }

            Elements.Get = new List<Element>();

            exporter = new WavefrontExporter(Elements.Get);

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
            BoneCommand.ExecuteFunc = x => bone();
            TextureCommand.ExecuteFunc = x => texture();

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
            Image12.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.load);
            Image13.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.texture);
            Image14.Get = ImageHelper.ConvertImageToWpfImage(Resources.Resource.bone);

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

            this.xy.LoadedAction = loaded;
            this.xz.LoadedAction = loaded;
            this.yz.LoadedAction = loaded;

            this.xy.BoneAction = bone;
            this.xz.BoneAction = bone;
            this.yz.BoneAction = bone;

            engine = new ModelViewer();

            SetPropertyChangeForAll();
        }

        bool[] isLoaded = new bool[3];
        void loaded(int xyz)
        {
            isLoaded[xyz - 1] = true;
            if(isLoaded[0] && isLoaded[1] && isLoaded[2])
                setLines();
        }

        private void texture()
        {
            xy.ClickMode = CLICK_MODE.TEXTURE;
            xz.ClickMode = CLICK_MODE.TEXTURE;
            yz.ClickMode = CLICK_MODE.TEXTURE;
        }

        private void bone()
        {
            xy.ClickMode = CLICK_MODE.NEW_BONE;
            xz.ClickMode = CLICK_MODE.NEW_BONE;
            yz.ClickMode = CLICK_MODE.NEW_BONE;
            isBone = false;
        }

        void clearSelection(bool clearAll = true)
        {
            Elements.Get.ForEach(e =>
            {
                if (clearAll)
                    e.IsSelected = false;
                foreach (var point in e.Positions)
                {
                    point.IsSelected = false;
                }
            });
        }

        bool isBone = false;
        int bone_x = 0;
        int bone_y = 0;
        int bone_z = 0;
        void bone(int xyz, int raster, int x, int y)
        {
            if(isBone)
            {
                Bone bone = new Bone();
                bone.Positions[0].X = bone_x;
                bone.Positions[0].Y = bone_y;
                bone.Positions[0].Z = bone_z;
                bone.Positions[1].X = x / raster * raster;
                bone.Positions[1].Y = y / raster * raster;
                bone.Positions[1].Z = bone_z;

                Skeletton.Bones.Add(bone);
                Elements.Get.Add(bone);
                isBone = false;
            }
            else
            {
                bone_x = x / raster * raster;
                bone_y = y / raster * raster;
                isBone = true;
            }
            setLines();
            resetList();
        }

        string textureFile = null;

        void select(int xyz, int _x, int _y)
        {
            if(xy.ClickMode == CLICK_MODE.TEXTURE)
            {
                List<FlatWithPositions> result = null;
                FlatWithPositions selectedFlat = null;
                if (xyz == 1)
                {
                    result = exporter.GetFlatBetweenXY(_x, _y);
                }
                else if (xyz == 2)
                {
                    result = exporter.GetFlatBetweenXY(_x, _y);
                }
                else if (xyz == 3)
                {
                    result = exporter.GetFlatBetweenXY(_x, _y);
                }
                if (result?.Count() > 1)
                {
                    setLines(result);
                    var vm = new FlatListViewModel(result, x =>
                    {
                        selectedFlat = x;
                        setLines(result, x); 
                    });
                    ViewManager.ShowDialogView(typeof(FlatListView), vm);
                    if (!vm.IsOK)
                        selectedFlat = null;
                }
                if (result?.Count() == 1)
                    selectedFlat = result.First();

                if(selectedFlat != null)
                {
                    setLines(null, selectedFlat);
                    if (textureFile == null)
                    {
                        if (ViewManager.FileDialog(
                            out string filename,
                            "Image Files (*.png, *.jpg)|*.png;*.jpg",
                            ConfigLoader.Instance.Config.DefaultTexturepath))
                        {
                            textureFile = filename;
                            setTexture(selectedFlat);
                        }
                    }
                    else
                    {
                        setTexture(selectedFlat);
                    }
                }
                setLines();

                return;
            }

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

        void setTexture(FlatWithPositions flat)
        {
            var vm = new TextureViewModel(textureFile, flat);
            ViewManager.ShowDialogView(typeof(TextureView), vm);
            if (vm.IsOK)
            {
                exporter.SetTextureforFlat(vm.TextureForFlat.FlatId, vm.TextureForFlat);
                //update3DView();
            }
        }

        void update3DView()
        {
            if (textureFile != null)
            {
                exporter.Export("tmp.obj");
                engine.Load(System.IO.Path.GetFullPath("tmp.obj"), textureFile);
            }
        }

        private void resize()
        {
            if (xy.ClickMode != CLICK_MODE.MOVE && xy.ClickMode != CLICK_MODE.RESIZE)
                lastClickMode = xy.ClickMode;
            xy.ClickMode = CLICK_MODE.RESIZE;
            xz.ClickMode = CLICK_MODE.RESIZE;
            yz.ClickMode = CLICK_MODE.RESIZE;
        }

        private void move()
        {
            if (xy.ClickMode != CLICK_MODE.MOVE && xy.ClickMode != CLICK_MODE.RESIZE)
                lastClickMode = xy.ClickMode;
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
                listVm = new ListViewModel(Elements.Get, selectElement, remove);
            ViewManager.ShowView(typeof(Views.ListView), listVm);
        }

        void resetList()
        {
            if (listVm != null)
                listVm.SetList(Elements.Get);
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
            var element = Elements.Get.First(x => x.Id == id);
            Elements.Get = Elements.Get.Where(x => x.Id != id).ToList();
            if (element.GetType() == typeof(Bone))
                Skeletton.Bones.Remove((Bone)element);
            setLines();
            resetList();
        }

        void export()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie den Dateinamen ein";
            ViewManager.ShowDialogView(typeof(Input), vm);
            if (vm.IsOK)
            {
                exporter.Export(System.IO.Path.Combine(ConfigLoader.Instance.Config.OutputPath, vm.Output.Get + ".obj"));
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
                Serializer.SerializeBinary(System.IO.Path.Combine(ConfigLoader.Instance.Config.SavePath, vm.Output.Get + ".model"), data);
            }
        }

        void load()
        {
            var vm = new DataInputViewModel();
            vm.Text.Get = "Bitte geben Sie den Dateinamen ein";
            ViewManager.ShowDialogView(typeof(Input), vm);
            if (vm.IsOK)
            {
                var fileName = System.IO.Path.Combine(ConfigLoader.Instance.Config.SavePath, vm.Output.Get + ".model");
                if (!File.Exists(fileName))
                {
                    MessageBox.Show(ViewManager.RootView, $"File \"{fileName}\" does not exist!");
                    return;
                }
                var data = Serializer.DeserializeBinary<SerializableElements>(fileName);
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
            engine.Dispose();
        }

        public void close()
        {
            xy.Close();
            xz.Close();
            yz.Close();
            engine.Dispose();
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
            //round(raster, x, y, z, out int _x, out int _y, out int _z);
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
                            posi.X = ((int)posi.X / raster * raster);
                            posi.Y = (int)posi.Y / raster * raster;
                            posi.Z = (int)posi.Z / raster * raster;
                        }
                    }
                }
            });
            setLines();
        }

        void resizeSelected()
        {

        }

        void resize(int caller, int side, int x, int y, int raster)
        {
            //if(isNewCube)
            //{

            //    return;
            //}

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

        void round(int raster, int x, int y, int z, out int _x, out int _y, out int _z)
        {
            _x = x / raster * raster;
            _y = y / raster * raster;
            _z = z / raster * raster;
            //if (cube_x % raster > raster / 2)
            //    cube_x += raster;
            //if (cube_y % raster > raster / 2)
            //    cube_y += raster;
            //if (cube_z % raster > raster / 2)
            //    cube_z += raster;
            //if (cube_width % raster > raster / 2)
            //    cube_width += raster;
            //if (cube_height % raster > raster / 2)
            //    cube_height += raster;
            //if (cube_depth % raster > raster / 2)
            //    cube_depth += raster;
        }

        void add()
        {
            if (xy.ClickMode == CLICK_MODE.NEW_CUBE || lastClickMode == CLICK_MODE.NEW_CUBE)
                addCube();
            else if (xy.ClickMode == CLICK_MODE.NEW_SPHERE || lastClickMode == CLICK_MODE.NEW_SPHERE)
                addSphere();

            lastClickMode = CLICK_MODE.SELECT_AREA;
            resetList();
            removeRectangles();
            setLines();
        }

        void addSphere()
        {
            List<int> l = new List<int>();
            List<float> l2 = new List<float>();

            int level = 21;
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
                var a = alpha * curLvl;
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

        Line createLine(Position3D a, Position3D b, int xyz)
        {
            if (xyz == 1)
            {
                return new Line
                {
                    X1 = a.X,
                    Y1 = a.Y,
                    X2 = b.X,
                    Y2 = b.Y
                };
            }
            else if (xyz == 2)
            {
                return new Line
                {
                    X1 = a.X,
                    Y1 = a.Z,
                    X2 = b.X,
                    Y2 = b.Z
                };
            }
            else if (xyz == 3)
            {
                return new Line
                {
                    X1 = a.Z,
                    Y1 = a.Y,
                    X2 = b.Z,
                    Y2 = b.Y
                };
            }
            return null;
        }

        Line createLine(Element element, int i, int j, int xyz)
        {
            return createLine(element.Positions[i], element.Positions[j], xyz);
        }

        List<KeyValuePair<Shape, Brush>> addLines(Element element, int xyz)
        {
            List<KeyValuePair<Shape, Brush>> lines = new List<KeyValuePair<Shape, Brush>>();

            var brush = element.IsSelected ? Brushes.Green : Brushes.Blue;

            if (element.GetType() == typeof(Cube))
            {
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
            }
            else if (element.GetType() == typeof(Sphere))
            {
                var e = (Sphere)element;
                int f = 0;

                for (int i = 1; i < e.positionPerLevelCount; ++i)
                {
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 0, i, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, i, i + 1, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, i + 1, i, xyz), brush));

                    f++;
                }
                lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 0, e.positionPerLevelCount, xyz), brush));
                lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, e.positionPerLevelCount, 1, xyz), brush));
                lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 1, e.positionPerLevelCount, xyz), brush));
                f++;

                for (int i = 0; i < e.level - 3; ++i)
                {
                    var level = e.positionPerLevelCount * i + 1;
                    for (int j = 0; j < e.positionPerLevelCount - 1; ++j)
                    {
                        lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 
                            level + j, level + j + e.positionPerLevelCount, xyz), brush));
                        lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                            level + j + e.positionPerLevelCount, level + j + e.positionPerLevelCount + 1, xyz), brush));
                        lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                            level + j + e.positionPerLevelCount + 1, level + j + 1, xyz), brush));
                        lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                            level + j + 1, level + j, xyz), brush));
                        f++;
                    }
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                        level, level + e.positionPerLevelCount, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                        level + e.positionPerLevelCount, level + e.positionPerLevelCount * 2 - 1, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                        level + e.positionPerLevelCount * 2 - 1, level + e.positionPerLevelCount - 1, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                        level + e.positionPerLevelCount - 1, level, xyz), brush));
                    f++;
                }


                for (int i = e.positionCount - e.positionPerLevelCount - 1;
                    i < e.positionCount - 1;
                    ++i)
                {
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, i, i + 1, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, i + 1, e.positionCount - 1, xyz), brush));
                    lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, e.positionCount - 1, i, xyz), brush));
                    f++;
                }
                lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 
                    e.positionCount - e.positionPerLevelCount - 1, e.positionCount - 2, xyz), brush));
                lines.Add(new KeyValuePair<Shape, Brush>(createLine(element,
                    e.positionCount - 2, e.positionCount - 1, xyz), brush));
                lines.Add(new KeyValuePair<Shape, Brush>(createLine(element, 
                    e.positionCount - 1, e.positionCount - e.positionPerLevelCount - 1, xyz), brush));
                f++;
            }

            return lines;
        }

        void setLines(List<FlatWithPositions> flats = null, FlatWithPositions selectedFlat = null)
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

            if(isMapElement)
            {
                for (int i = 0; i < mapSizeX; ++i)
                {
                    for (int j = 0; j < mapSizeY; ++j)
                    {
                        int _x1 = i * 128;
                        int _y1 = j * 128;
                        int _x2 = (i + 1) * 128;
                        int _y2 = (j + 1) * 128;
                        int _zOffset = 128 * 2;

                        var p1 = new Position3D
                        {
                            X = _x1,
                            Y = _zOffset,
                            Z = _y1
                        };
                        var p2 = new Position3D
                        {
                            X = _x2,
                            Y = _zOffset,
                            Z = _y1
                        };
                        var p3 = new Position3D
                        {
                            X = _x2,
                            Y = _zOffset,
                            Z = _y2
                        };
                        var p4 = new Position3D
                        {
                            X = _x1,
                            Y = _zOffset,
                            Z = _y2
                        };

                        lines1.Add(new KeyValuePair<Shape, Brush>(createLine(p1, p2, 1), Brushes.Black));
                        lines1.Add(new KeyValuePair<Shape, Brush>(createLine(p2, p3, 1), Brushes.Black));
                        lines1.Add(new KeyValuePair<Shape, Brush>(createLine(p3, p4, 1), Brushes.Black));
                        lines1.Add(new KeyValuePair<Shape, Brush>(createLine(p1, p4, 1), Brushes.Black));

                        lines2.Add(new KeyValuePair<Shape, Brush>(createLine(p1, p2, 2), Brushes.Black));
                        lines2.Add(new KeyValuePair<Shape, Brush>(createLine(p2, p3, 2), Brushes.Black));
                        lines2.Add(new KeyValuePair<Shape, Brush>(createLine(p3, p4, 2), Brushes.Black));
                        lines2.Add(new KeyValuePair<Shape, Brush>(createLine(p1, p4, 2), Brushes.Black));

                        lines3.Add(new KeyValuePair<Shape, Brush>(createLine(p1, p2, 3), Brushes.Black));
                        lines3.Add(new KeyValuePair<Shape, Brush>(createLine(p2, p3, 3), Brushes.Black));
                        lines3.Add(new KeyValuePair<Shape, Brush>(createLine(p3, p4, 3), Brushes.Black));
                        lines3.Add(new KeyValuePair<Shape, Brush>(createLine(p1, p4, 3), Brushes.Black));

                        foreach (var line in lines1)
                            line.Key.StrokeThickness = 4;
                        foreach (var line in lines2)
                            line.Key.StrokeThickness = 4;
                        foreach (var line in lines3)
                            line.Key.StrokeThickness = 4;
                    }
                }
            }

            var boneList1 = new List<KeyValuePair<Shape, Brush>>();
            var boneList2 = new List<KeyValuePair<Shape, Brush>>();
            var boneList3 = new List<KeyValuePair<Shape, Brush>>();
            int boneColor = 0;
            foreach (var bone in Skeletton.Bones)
            {
                boneColor++;
                boneList1.Add(new KeyValuePair<Shape, Brush>(createLine(bone.Positions[0], bone.Positions[1], 1), boneColor % 2 == 0 ? Brushes.Green : Brushes.DarkRed));
                boneList2.Add(new KeyValuePair<Shape, Brush>(createLine(bone.Positions[0], bone.Positions[1], 2), boneColor % 2 == 0 ? Brushes.Green : Brushes.DarkRed));
                boneList3.Add(new KeyValuePair<Shape, Brush>(createLine(bone.Positions[0], bone.Positions[1], 3), boneColor % 2 == 0 ? Brushes.Green : Brushes.DarkRed));
            }

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
            xy.Lines.AddRange(boneList1);
            xz.Lines.AddRange(boneList2);
            yz.Lines.AddRange(boneList3);

            if (flats != null)
            {
                foreach (var flat in flats)
                {
                    xy.Lines.Add(new KeyValuePair<Shape, Brush>
                        (createLine(flat.Positions[0], flat.Positions[flat.Positions.Count - 1], 1),
                        Brushes.Cyan));
                    xz.Lines.Add(new KeyValuePair<Shape, Brush>
                        (createLine(flat.Positions[0], flat.Positions[flat.Positions.Count - 1], 2),
                        Brushes.Cyan));
                    yz.Lines.Add(new KeyValuePair<Shape, Brush>
                        (createLine(flat.Positions[0], flat.Positions[flat.Positions.Count - 1], 3),
                        Brushes.Cyan));

                    for (int i = 0; i < flat.Positions.Count - 1; i++)
                    {
                        xy.Lines.Add(new KeyValuePair<Shape, Brush>
                            (createLine(flat.Positions[i], flat.Positions[i + 1], 1),
                            Brushes.Cyan));
                        xz.Lines.Add(new KeyValuePair<Shape, Brush>
                            (createLine(flat.Positions[i], flat.Positions[i + 1], 2),
                            Brushes.Cyan));
                        yz.Lines.Add(new KeyValuePair<Shape, Brush>
                            (createLine(flat.Positions[i], flat.Positions[i + 1], 3),
                            Brushes.Cyan));
                    }
                }
            }

            if (selectedFlat != null)
            {
                xy.Lines.Add(new KeyValuePair<Shape, Brush>
                    (createLine(selectedFlat.Positions[0], selectedFlat.Positions[selectedFlat.Positions.Count - 1], 1),
                    Brushes.Red));
                xz.Lines.Add(new KeyValuePair<Shape, Brush>
                    (createLine(selectedFlat.Positions[0], selectedFlat.Positions[selectedFlat.Positions.Count - 1], 2),
                    Brushes.Red));
                yz.Lines.Add(new KeyValuePair<Shape, Brush>
                    (createLine(selectedFlat.Positions[0], selectedFlat.Positions[selectedFlat.Positions.Count - 1], 3),
                    Brushes.Red));

                for (int i = 0; i < selectedFlat.Positions.Count - 1; i++)
                {
                    xy.Lines.Add(new KeyValuePair<Shape, Brush>
                        (createLine(selectedFlat.Positions[i], selectedFlat.Positions[i + 1], 1),
                        Brushes.Red));
                    xz.Lines.Add(new KeyValuePair<Shape, Brush>
                        (createLine(selectedFlat.Positions[i], selectedFlat.Positions[i + 1], 2),
                        Brushes.Red));
                    yz.Lines.Add(new KeyValuePair<Shape, Brush>
                        (createLine(selectedFlat.Positions[i], selectedFlat.Positions[i + 1], 3),
                        Brushes.Red));
                }
            }

            xy.LoadCan();
            xz.LoadCan();
            yz.LoadCan();
            exporter.LoadData(Elements.Get);
            update3DView();
        }

        void drawCube(float sx, float sy, float sz, float x, float y, float z)
        {
        }
    }
}
