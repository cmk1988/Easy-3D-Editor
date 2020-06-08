using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
