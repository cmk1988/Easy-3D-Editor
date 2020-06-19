using System.Windows;
using System.Windows.Input;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ModelViewWindowViewModel : ViewModelBase
    {
        public ModelViewer Engine { get; set; }

        Point? lastPoint;

        public void MouseMove_Event(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition((Window)sender);
                if (lastPoint != null)
                {
                    var x = (lastPoint.Value.X - point.X) / 100.0f;
                    var y = (lastPoint.Value.Y - point.Y) / 100.0f;

                    Engine.Rotate((float)(y), (float)(x), 0.0f);
                }
                else
                    lastPoint = point;
            }
            else
                lastPoint = null;
        }

        public void MouseRightDown_Event(object sender, MouseEventArgs e)
        {

        }

        public void MouseLeftDown_Event(object sender, MouseEventArgs e)
        {

        }

        public void MouseWheel_Event(object sender, MouseWheelEventArgs e)
        {
            var x = e.Delta / 100.0f;
            Engine.Move(x);
        }

        public ModelViewWindowViewModel()
        {
            SetPropertyChangeForAll();
        }
    }
}
