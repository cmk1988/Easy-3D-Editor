using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Services
{
    class ImageHelper
    {
        public static System.Windows.Controls.Image ConvertImageToWpfImage(System.Drawing.Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image darf nicht null sein.");

            using (System.Drawing.Bitmap dImg = new System.Drawing.Bitmap(image))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    dImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(ms.ToArray());
                    bImg.EndInit();

                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = bImg;

                    return img;
                }
            }
        }
    }
}
