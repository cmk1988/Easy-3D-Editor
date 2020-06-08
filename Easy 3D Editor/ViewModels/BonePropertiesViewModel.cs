using Easy_3D_Editor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class BonePropertiesViewModel : ViewModelBase
    {
        public NotifiingProperty<string> Id { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> BoneId { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> ParentId { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> From { get; } = new NotifiingProperty<string>();
        public NotifiingProperty<string> To { get; } = new NotifiingProperty<string>();

        public Command SaveCommand { get; } = new Command();
        public Command CancelCommand { get; } = new Command();
        public Command RemoveCommand { get; } = new Command();

        public BonePropertiesViewModel(Bone bone, Action<int> removeAction)
        {
            Id.Get = $"{bone.Id}";
            BoneId.Get = $"{bone.BoneId}";
            ParentId.Get = $"{bone.ParentBone}";
            From.Get = $"{bone.Positions[0].X} / {bone.Positions[0].Y} / {bone.Positions[0].Z}";
            To.Get = $"{bone.Positions[1].X} / {bone.Positions[1].Y} / {bone.Positions[1].Z}";

            SaveCommand.ExecuteFunc = x =>
            {
                if (ParentId.Get != null && int.TryParse(ParentId.Get, out int id))
                    bone.ParentBone = id;
            };

            CancelCommand.ExecuteFunc = x =>
            {
                Close();
            };

            RemoveCommand.ExecuteFunc = x =>
            {
                removeAction(bone.Id);
                Close();
            };

            SetPropertyChangeForAll();
        }
    }
}
