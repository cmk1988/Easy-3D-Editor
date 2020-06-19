using System;
using System.Collections.Generic;
using System.Linq;
using WpfServices;
using static Easy_3D_Editor.Services.WavefrontExporter;

namespace Easy_3D_Editor.ViewModels
{
    class ListItem
    {
        public FlatWithPositions flat {get;set;}
        public string Text => $"{flat.Positions[0].X}/{flat.Positions[0].Y}/{flat.Positions[0].Z}";
    }

    class FlatListViewModel : ViewModelBase
    {
        public NotifiingProperty<ListItem> SelectedItem { get; set; } = new NotifiingProperty<ListItem>();

        public Command OK { get; set; } = new Command();
        public Command Cancel { get; set; } = new Command();

        public List<ListItem> List { get; set; }

        public bool IsOK { get; private set; }

        public FlatListViewModel(List<FlatWithPositions> list, Action<FlatWithPositions> selctionChanged)
        {
            this.List = list.Select(x => new ListItem {  flat = x }).ToList();

            SelectedItem.AdditionalAction = x =>
            {
                if(x != null)
                {
                    selctionChanged(x.flat);
                }
            };

            OK.ExecuteFunc = x =>
            {
                IsOK = true;
                Close();
            };

            Cancel.ExecuteFunc = x =>
            {
                IsOK = false;
                Close();
            };

            SetPropertyChangeForAll();
        }
    }
}
