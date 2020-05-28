using System;
using System.Windows;

namespace WpfServices
{
    public class ViewManager
    {
        public static Window RootView { get; private set; }

        public static bool ShowDialogRootView(Type viewType, ViewModelBase viewModel)
        {
            if (RootView != null)
                throw new Exception("Root view hast to be unique!");
            RootView = createView(viewType, viewModel);
            return !RootView.IsActive ? RootView.ShowDialog() ?? false : false;
        }

        public static bool ShowDialogView(Type viewType, ViewModelBase viewModel, int? x = null, int? y = null)
        {
            var view = createView(viewType, viewModel);
            if (x != null)
                view.Left = x.Value;
            if (y != null)
                view.Top = y.Value;
            return !view.IsActive ? view.ShowDialog() ?? false : false;
        }

        public static void ShowView(Type viewType, ViewModelBase viewModel, int? x = null, int? y = null)
        {
            var view = createView(viewType, viewModel);
            if (x != null)
                view.Left = x.Value;
            if (y != null)
                view.Top = y.Value;
            view.Show();
        }

        static Window createView(Type viewType, ViewModelBase viewModel)
        {
            if (!viewType.IsSubclassOf(typeof(Window)))
                throw new Exception("View type is not supported.");
            Window view = (Window)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            viewModel.Close = view.Close;
            return view;
        }
    }
}
