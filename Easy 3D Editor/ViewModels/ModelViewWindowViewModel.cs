using System.Windows;
using System.Windows.Input;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ModelViewWindowViewModel : ViewModelBase
    {
        public ModelViewer Engine { get; set; }

        Point lastPoint;
        bool isMovement = false;

        public void MouseMove_Event(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition((Window)sender);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(lastPoint != null)
                {
                    var x = (lastPoint.X - point.X) / 100.0f;
                    var y = (lastPoint.Y - point.Y) / 100.0f;

                    Engine.Rotate((float)(y), (float)(x), 0.0f);
                }
            }
            else
                lastPoint = point;
        }

        public void MouseRightDown_Event(object sender, MouseEventArgs e)
        {

        }

        public void MouseLeftDown_Event(object sender, MouseEventArgs e)
        {

        }

        public void MouseWheel_Event(object sender, MouseEventArgs e)
        {

        }

        public ModelViewWindowViewModel()
        {
            SetPropertyChangeForAll();
        }
    }
}
