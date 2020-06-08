using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Easy_3D_Editor.Models
{
    public class ListElement
    {
        Element element;

        public int Id => element.Id;
        public string Text => element.Text;

        public ListElement(Element element)
        {
            this.element = element;
        }
    }

    [Serializable]
    public class Position3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public bool IsSelected { get; set; }
    }

    [Serializable]
    public class Position2D
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    [Serializable]
    public abstract class Element
    {
        public static int id = 0;

        public int Id { get; set; }
        public virtual string Text { get; set; }
        public Position3D[] Positions { get; set; }
        public bool IsSelected { get; set; }
        public int flatCount { get; protected set; }

        public Element()
        {
            Id = ++id;
        }

        public ListElement GetListElement()
        {
            return new ListElement(this);
        }
    }

    [Serializable]
    public class SerializableElements
    {
        public int Id { get; set; }
        public List<Element> Elements { get; set; }
    }

    [Serializable]
    class Cube : Element
    {
        public Cube() : base()
        {
            Text = $"Id: {Id} (Cube)";
            Positions = new Position3D[8];
            flatCount = 6;
            for (int i = 0; i < 8; i++)
            {
                Positions[i] = new Position3D();
            }
        }
    }

    [Serializable]
    class Sphere : Element
    {
        public int level { get; }
        public int positionCount { get; }
        public int positionPerLevelCount { get; }
        public bool CalcRoundNormals { get; set; }

        public Sphere(int level) : base()
        {
            if (level % 2 == 0)
                level -= 1;
            this.level = level;

            positionPerLevelCount = level + level - 2;
            positionCount = (level - 2) * positionPerLevelCount + 2;
            flatCount = (level-1) * positionPerLevelCount;

            Text = $"Id: {Id} (Sphere) level={level}, points={positionCount}";
            Positions = new Position3D[positionCount];
            for (int i = 0; i < positionCount; i++)
            {
                Positions[i] = new Position3D();
            }
        }
    }

    [Serializable]
    class Rectangle : Element
    {
        public Rectangle() : base()
        {
            Text = $"Id: {Id} (Rectangle)";
            Positions = new Position3D[3];
        }
    }

    [Serializable]
    class Plane : Element
    {
        public Plane() : base()
        {
            Text = $"Id: {Id} (Plane)";
            Positions = new Position3D[4];
        }
    }

    class Bone : Element
    {
        static int bid = 1;
        public int BoneId { get; }
        public List<Element> Elements { get; set; } = new List<Element>();
        public Matrix3D Matrix { get; set; }
        public int ParentBone { get; set; } = 0;

        public override string Text => $"Id: {Id} (Bone) Parent = {ParentBone}";

        public Bone() : base()
        {
            Positions = new Position3D[2];
            Positions[0] = new Position3D();
            Positions[1] = new Position3D();
            flatCount = 0;
            BoneId = ++bid;

            Matrix = new Matrix3D
             (
                1.0, 0.0, 0.0, 0.0,
                0.0, 1.0, 0.0, 0.0,
                0.0, 0.0, 1.0, 0.0,
                0.0, 0.0, 0.0, 1.0
            );
        }
    }
}
