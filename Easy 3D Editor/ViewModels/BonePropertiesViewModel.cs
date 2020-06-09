using Easy_3D_Editor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServices;

namespace Easy_3D_Editor.ViewModels
{
    class BoneListElementBase
    {
        Element element;
        public string Text => element.Text;
        public int Id => element.Id;

        public BoneListElementBase(Element element)
        {
            this.element = element;
        }
    }

    class BoneChild : BoneListElementBase
    {
        public Command AddCommand { get; } = new Command();

        public BoneChild(Element element, Action<int> action) : base(element)
        {
            AddCommand.ExecuteFunc = x => action(Id);
        }
    }

    class UsedBoneChild : BoneListElementBase
    {
        public Command RemoveCommand { get; } = new Command();

        public UsedBoneChild(Element element, Action<int> action) : base(element)
        {
            RemoveCommand.ExecuteFunc = x => action(Id);
        }
    }

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

        readonly Bone bone;
        readonly List<Element> elements;

        public NotifiingProperty<List<UsedBoneChild>> List1 { get; } = new NotifiingProperty<List<UsedBoneChild>>();
        public NotifiingProperty<List<BoneChild>> List2 { get; } = new NotifiingProperty<List<BoneChild>>();

        public BonePropertiesViewModel(Bone bone, IEnumerable<Element> elements, Action<int> removeAction)
        {
            List1.Get = new List<UsedBoneChild>();
            this.bone = bone;
            this.elements = elements.ToList();
            List2.Get = this.elements.Select(x => new BoneChild(x, o =>
            {
                List1.Get.Add(new UsedBoneChild(x, p =>
                {
                    List1.Get = List1.Get.Where(q => q.Id != p).ToList();
                }));
                List1.Get = List1.Get.ToList();
            })).ToList();

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
