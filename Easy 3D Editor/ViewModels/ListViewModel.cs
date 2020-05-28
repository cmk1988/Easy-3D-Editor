using Easy_3D_Editor.Models;
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

        public ListElementWithCommand(ListElement listElement, Action<int> removeAction)
        {
            this.listElement = listElement;
            RemoveCommand.ExecuteFunc = x =>
            {
                removeAction(Id);
            };
        }
    }

    class ListViewModel : ViewModelBase
    {

        public NotifiingProperty<List<ListElementWithCommand>> List { get; } = new NotifiingProperty<List<ListElementWithCommand>>();
        public NotifiingProperty<ListElementWithCommand> SelectedItem { get; } = new NotifiingProperty<ListElementWithCommand>();

        public Command RemoveCommand { get; } = new Command();
        Action<int> removeItem;

        public ListViewModel(List<ListElement> list, Action<int> selectedItemChanged, Action<int> removeItem)
        {
            this.removeItem = removeItem;

            SetPropertyChangeForAll();

            SelectedItem.AdditionalAction = x =>
            {
                if(x != null)
                    selectedItemChanged(x.Id);
            };

            RemoveCommand.ExecuteFunc = x =>
            {
                if(x != null)
                    removeItem(((ListElement)x).Id);
            };

            SetList(list);
        }

        public void SetList(List<ListElement> list)
        {
            List.Get = list.Select(x => new ListElementWithCommand(x, removeItem)).ToList();
        }
    }
}
