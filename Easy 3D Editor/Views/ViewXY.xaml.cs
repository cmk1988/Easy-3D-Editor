using Easy_3D_Editor.Services;
using Easy_3D_Editor.ViewModels;
using System.Windows;

namespace Easy_3D_Editor.Views
{
    /// <summary>
    /// Interaktionslogik für ViewXY.xaml
    /// </summary>
    public partial class ViewXY : Window
    {
        public ViewXY()
        {
            InitializeComponent();
            Closing += (s, e) =>
            {
                PositionManager.Instance.SetPosition(((ViewModelXY)DataContext).Title.Get, (int)Left, (int)Top);
            };
            Loaded += (x, y) =>
            {
                var posi = PositionManager.Instance.GetPosition(((ViewModelXY)DataContext).Title.Get);
                if (posi != null)
                {
                    Left = posi.X;
                    Top = posi.Y;
                }
                ((ViewModelXY)DataContext).Can.Get = can;
                ((ViewModelXY)DataContext).LoadCan((int)can.ActualWidth, (int)can.ActualHeight);
            };
            SizeChanged += (x, y) =>
            {
                ((ViewModelXY)DataContext).Can.Get = can;
                ((ViewModelXY)DataContext).LoadCan((int)can.ActualWidth, (int)can.ActualHeight);
            };
        }
    }
}
