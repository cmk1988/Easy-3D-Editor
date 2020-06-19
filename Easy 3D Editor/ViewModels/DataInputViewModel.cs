using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class DataInputViewModel : ViewModelBase
    {
        public NotifiingProperty<string> Text { get; set; } = new NotifiingProperty<string>();
        public bool IsOK { get; set; }
        public NotifiingProperty<string> Output { get; set; } = new NotifiingProperty<string>();

        public Command OK { get; set; } = new Command();
        public Command Cancel { get; set; } = new Command();

        public DataInputViewModel()
        {
            OK.ExecuteFunc = x => ok();
            Cancel.ExecuteFunc = x => cancel();
            SetPropertyChangeForAll();
        }

        void ok()
        {
            IsOK = true;
            Close();
        }

        void cancel()
        {
            IsOK = false;
            Close();
        }
    }
}
