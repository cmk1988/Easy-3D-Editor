using Easy_3D_Editor.Models;
using Easy_3D_Editor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class ListElementWithCommand
    {
        ListElement listElement;

        public int Id => listElement.Id;
        public string Text => listElement.Text;

        public Command RemoveCommand { get; } = new Command();
        public Command PropertiesCommand { get; } = new Command();

        public ListElementWithCommand(ListElement listElement, Action<int> removeAction, Action<int> propertyAction)
        {
            this.listElement = listElement;
            RemoveCommand.ExecuteFunc = x =>
            {
                removeAction(Id);
            };
            PropertiesCommand.ExecuteFunc = x =>
            {
                propertyAction(Id);
            };
        }
    }

    class ListViewModel : ViewModelBase
    {

        public NotifiingProperty<List<ListElementWithCommand>> List { get; } = new NotifiingProperty<List<ListElementWithCommand>>();
        public NotifiingProperty<ListElementWithCommand> SelectedItem { get; } = new NotifiingProperty<ListElementWithCommand>();

        Action<int> removeItem;

        List<Element> list;

        public ListViewModel(List<Element> list, Action<int> selectedItemChanged, Action<int> removeItem)
        {
            this.removeItem = removeItem;
            this.list = list;

            SetPropertyChangeForAll();

            SelectedItem.AdditionalAction = x =>
            {
                if(x != null)
                    selectedItemChanged(x.Id);
            };

            

            SetList(list);
        }

        public void showProperties(int id)
        {
            var element = list.First(x => x.Id == id);
            if(element.GetType() == typeof(Bone))
            {
                var vm = new BonePropertiesViewModel((Bone)element, list.Where(x => x.GetType() != typeof(Bone)), removeItem);
                ViewManager.ShowDialogView(typeof(BoneProperties), vm);
            }
            SetList(list);
        }

        public void SetList(List<Element> list)
        {
            this.list = list;
            List.Get = list.Select(x => new ListElementWithCommand(x.GetListElement(), removeItem, showProperties)).ToList();
        }
    }
}
