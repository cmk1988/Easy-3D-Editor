using System.Collections.Generic;

namespace Easy_3D_Editor.Models
{
    class AnimationMovement
    {
        public int BoneId { get; set; }
        public float MovementX { get; set; }
        public float MovementY { get; set; }
    }

    class AnimationStep
    {
        public int Time { get; set; }
        public List<AnimationMovement> Movements { get; set; }
    }

    class Animation
    {
        public List<AnimationStep> Steps { get; set; }
    }
}
