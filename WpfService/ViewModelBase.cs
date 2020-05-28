using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfServices
{
    public class Command : ICommand
    {
        public Predicate<object> CanExecuteFunc { get; set; }

        public Action<object> ExecuteFunc { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc == null)
                return true;
            return CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteFunc(parameter);
        }
    }

    public interface INotifiingProperty
    {
        Action propertyChanges { get; set; }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Action Close;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetPropertyChange(string name, INotifiingProperty property)
        {
            property.propertyChanges = () => { this.OnPropertyChanged(name); };
        }

        public void SetPropertyChangeForAll()
        {
            this.GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
                    x.PropertyType.GetGenericTypeDefinition() == typeof(NotifiingProperty<>))
                .ToList()
                .ForEach(x => SetPropertyChange(x.Name, (INotifiingProperty)x.GetValue(this)));
        }

        public class NotifiingProperty<T> : INotifiingProperty
        {
            T property;
            Object lockObject = new Object();
            public Action<T> AdditionalAction { get; set; }

            public NotifiingProperty()
            {
            }

            public NotifiingProperty(T t)
            {
                property = t;
            }

            public T Get
            {
                get => property;
                set
                {
                    property = value;
                    propertyChanges?.Invoke();
                    AdditionalAction?.Invoke(property);
                }
            }

            public Action propertyChanges { get; set; }
            public async Task SetAsync(T value)
            {
                await Task.Run(() =>
                {
                    lock (lockObject)
                        property = value;
                });
                propertyChanges?.Invoke();
            }
        }
    }
}
