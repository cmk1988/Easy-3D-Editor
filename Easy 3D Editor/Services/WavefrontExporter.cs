using Easy_3D_Editor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Services
{
    class WavefrontExporter
    {
        class Vertex
        {
            public int Id { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public virtual string GetLine()
            {
                return $"v {X} {Y} {Z}\r\n".Replace(",", ".");
            }
        }

        class TextureCoordinate
        {
            public int Id { get; set; }
            public float X { get; set; }
            public float Y { get; set; }

            public string GetLine()
            {
                return $"vt {X} {Y}\r\n".Replace(",", ".");
            }
        }

        class Normal : Vertex
        {
            public override string GetLine()
            {
                return $"vn {X} {Y} {Z}\r\n".Replace(",", ".");
            }
        }

        class FlatPoint
        {
            public int VertexId { get; set; }
            public int TextureId { get; set; } = 0;
            public int NormalId { get; set; } = 0;

            public string GetString()
            {
                if (TextureId == 0 && NormalId == 0)
                    return $" {VertexId}";
                return $" {VertexId}/{(TextureId != 0 ? TextureId.ToString() : "")}/{(NormalId != 0 ? NormalId.ToString() : "")}";
            }
        }

        class Flat
        {
            public List<FlatPoint> Points { get; set; } = new List<FlatPoint>();

            public string GetLine()
            {
                if (Points.Count == 0)
                    return "";
                var str = "";
                foreach (var point in Points)
                    str += point.GetString();
                return $"f{str}\r\n";
            }
        }


        string filename;
        float multiplicator;

        List<Vertex> vertices = new List<Vertex>();
        List<TextureCoordinate> textureCoordinates = new List<TextureCoordinate>();
        List<Normal> normals = new List<Normal>();
        List<Flat> flats = new List<Flat>();

        int addVertex(int x, int y, int z)
        {
            float fX = x * multiplicator;
            float fY = y * multiplicator;
            float fZ = z * multiplicator;

            var vertex = vertices.FirstOrDefault(v => v.X == fX && v.Y == fY && v.Z == fZ);
            if (vertex != null)
            {
                return vertex.Id;
            }
            int i = vertices.Count + 1;
            vertices.Add(new Vertex 
            {
                X = fX,
                Y = fY,
                Z = fZ,
                Id = i
            });
            return i;
        }

        int addTexture(int x, int y)
        {
            float fX = x;
            float fY = y;

            var texture = textureCoordinates.FirstOrDefault(v => v.X == fX && v.Y == fY);
            if (texture != null)
            {
                return texture.Id;
            }
            int i = textureCoordinates.Count + 1;
            textureCoordinates.Add(new TextureCoordinate
            {
                X = fX,
                Y = fY,
                Id = i
            });
            return i;
        }

        int addNormal(float x, float y, float z)
        {
            var normal = normals.FirstOrDefault(v => v.X == x && v.Y == y && v.Z == z);
            if (normal != null)
            {
                return normal.Id;
            }
            int i = normals.Count + 1;
            normals.Add(new Normal
            {
                X = x,
                Y = y,
                Z = z,
                Id = i
            });
            return i;
        }

        void addFlat(Position3D a, Position3D b, Position3D c, Position3D d = null)
        {
            var flat = new Flat();

            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(a.X, a.Y, a.Z)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(b.X, b.Y, b.Z)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(c.X, c.Y, c.Z)
            });
            if(d != null)
                flat.Points.Add(new FlatPoint
                {
                    VertexId = addVertex(d.X, d.Y, d.Z)
                });

            flats.Add(flat);
        }

        void calcFlatNormals()
        {
            foreach (var flat in flats)
            {
                float x;
                float y;
                float z;

                var f1 = vertices[flat.Points[0].VertexId - 1];
                var f2 = vertices[flat.Points[1].VertexId - 1];
                var f3 = vertices[flat.Points[2].VertexId - 1];

                var A = new Vertex
                {
                    X = f2.X - f1.X,
                    Y = f2.Y - f1.Y,
                    Z = f2.Z - f1.Z
                };
                var B = new Vertex
                {
                    X = f3.X - f1.X,
                    Y = f3.Y - f1.Y,
                    Z = f3.Z - f1.Z
                };

                x = A.Y * B.Z - A.Z * B.Y;
                y = A.Z * B.X - A.X * B.Z;
                z = A.X * B.Y - A.Y * B.X;

                foreach (var point in flat.Points)
                {
                    point.NormalId = addNormal(x, y, z);
                }
            }
        }

        public WavefrontExporter(string filename, IEnumerable<Element> elements, float multiplicator = 0.01f)
        {
            this.filename = filename;
            this.multiplicator = multiplicator;

            foreach (var element in elements)
            {
                if (element.Positions.Count() == 8)
                {
                    addFlat(element.Positions[0], element.Positions[1], element.Positions[2], element.Positions[3]);
                    addFlat(element.Positions[4], element.Positions[5], element.Positions[6], element.Positions[7]);
                    addFlat(element.Positions[0], element.Positions[4], element.Positions[5], element.Positions[1]);
                    addFlat(element.Positions[3], element.Positions[7], element.Positions[6], element.Positions[2]);
                    addFlat(element.Positions[0], element.Positions[4], element.Positions[7], element.Positions[3]);
                    addFlat(element.Positions[1], element.Positions[5], element.Positions[6], element.Positions[2]);
                }
            }
            calcFlatNormals();
        }

        public void Export()
        {
            var content = $"# created by CMK Easy 3D Editor\r\n\r\no {filename}\r\n\r\n";

            foreach (var vertex in vertices)
                content += vertex.GetLine();
            content += "\r\n";
            foreach (var texture in textureCoordinates)
                content += texture.GetLine();
            content += "\r\n";
            foreach (var normal in normals)
                content += normal.GetLine();
            content += "\r\n";
            foreach (var flat in flats)
                content += flat.GetLine();

            File.WriteAllText(filename, content);
        }
    }
}
