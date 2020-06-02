using Easy_3D_Editor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class TextureViewModel : ViewModelBase
    {
        public NotifiingProperty<int> X { get; set; } = new NotifiingProperty<int>(200);
        public NotifiingProperty<int> Y { get; set; } = new NotifiingProperty<int>(200);

        public NotifiingProperty<Image> TextureImage { get; set; } = new NotifiingProperty<Image>();

        public System.Drawing.Image Image;

        public TextureViewModel(string filename)
        {
            Image = System.Drawing.Image.FromFile(filename);
            TextureImage.Get = ImageHelper.ConvertImageToWpfImage(Image);
            SetPropertyChangeForAll();
        }
    }
}
