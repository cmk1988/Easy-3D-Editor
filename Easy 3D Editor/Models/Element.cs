using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Models
{
    public class ListElement
    {
        public int Id { get; }
        public string Text { get; }

        public ListElement(int id, string text)
        {
            Id = id;
            Text = text;
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
    public abstract class Element
    {
        public static int id = 0;

        public int Id { get; set; }
        public string Text { get; set; }
        public Position3D[] Positions { get; set; }
        public bool IsSelected { get; set; }

        public Element()
        {
            Id = ++id;
        }

        public ListElement GetListElement()
        {
            return new ListElement(Id, Text);
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
        public int flatCount { get; }

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
        public Rectangle()
        {
            Text = $"Id: {Id} (Rectangle)";
            Positions = new Position3D[3];
        }
    }

    [Serializable]
    class Plane : Element
    {
        public Plane()
        {
            Text = $"Id: {Id} (Plane)";
            Positions = new Position3D[4];
        }
    }
}
