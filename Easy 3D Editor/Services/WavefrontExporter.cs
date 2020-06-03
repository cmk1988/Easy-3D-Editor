using Easy_3D_Editor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Easy_3D_Editor.Services
{
    class WavefrontExporter
    {
        public class TextureForFlat
        {
            public int FlatId { get; set; }
            public List<TextureCoordinate> Coordinates { get; set; } = new List<TextureCoordinate>();
        }

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

        public class TextureCoordinate
        {
            public int Id { get; set; }
            public float X { get; set; }
            public float Y { get; set; }

            public string GetLine()
            {
                return $"vt {X} {Y}\r\n".Replace(",", ".");
            }
        }

        class fPoint
        {
            public float x;
            public float y;

            public fPoint(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public fPoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        class Normal : Vertex
        {
            public override string GetLine()
            {
                return $"vn {X} {Y} {Z}\r\n".Replace(",", ".");
            }
        }

        public class FlatPoint
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

        public class Flat
        {
            public int ModelId { get; }
            public List<FlatPoint> Points { get; set; } = new List<FlatPoint>();

            public Flat(int id)
            {
                ModelId = id;
            }

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

        public class FlatWithPositions
        {
            public int FlatId { get; set; }
            public List<Position3D> Positions { get; set; } = new List<Position3D>();
        }

        float multiplicator;

        List<Vertex> vertices = new List<Vertex>();
        List<TextureCoordinate> textureCoordinates = new List<TextureCoordinate>();
        List<Normal> normals = new List<Normal>();
        List<Flat> flats = new List<Flat>();

        int addVertex(float x, float y, float z)
        {
            float fX = x * multiplicator;
            float fY = y * multiplicator * -1.0f;
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

        int addTexture(float x, float y)
        {
            var texture = textureCoordinates.FirstOrDefault(v => v.X == x && v.Y == y);
            if (texture != null)
            {
                return texture.Id;
            }
            int i = textureCoordinates.Count + 1;
            textureCoordinates.Add(new TextureCoordinate
            {
                X = x,
                Y = y,
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

        void addFlat(int id, Position3D a, Position3D b, Position3D c, Position3D d = null)
        {
            var flat = new Flat(id);

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
            if (d != null)
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

        public WavefrontExporter(IEnumerable<Element> elements, float multiplicator = 0.01f)
        {
            LoadData(elements, multiplicator);
        }

        float sign(fPoint p1, fPoint p2, fPoint p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        bool PointInTriangle(fPoint pt, fPoint v1, fPoint v2, fPoint v3)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = sign(pt, v1, v2);
            d2 = sign(pt, v2, v3);
            d3 = sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

        public List<FlatWithPositions> GetFlatBetweenXY(int x, int y)
        {
            var result = new List<FlatWithPositions>();
            var p = new fPoint(x * multiplicator, y * multiplicator * -1.0f);

            foreach (var flat in flats)
            {
                bool b = false;
                if(flat.Points.Count() == 4)
                {
                    var v1 = vertices[flat.Points[0].VertexId - 1];
                    var v2 = vertices[flat.Points[1].VertexId - 1];
                    var v3 = vertices[flat.Points[2].VertexId - 1];
                    var v4 = vertices[flat.Points[3].VertexId - 1];

                    b = PointInTriangle(p,
                            new fPoint(v1.X, v1.Y),
                            new fPoint(v2.X, v2.Y),
                            new fPoint(v3.X, v3.Y)
                            ) || 
                        PointInTriangle(p,
                            new fPoint(v3.X, v3.Y),
                            new fPoint(v4.X, v4.Y),
                            new fPoint(v1.X, v1.Y)
                            );
                }
                else if(flat.Points.Count() == 3)
                {
                    var v1 = vertices[flat.Points[0].VertexId - 1];
                    var v2 = vertices[flat.Points[1].VertexId - 1];
                    var v3 = vertices[flat.Points[2].VertexId - 1];
                    b = PointInTriangle(p,
                            new fPoint(v1.X, v1.Y),
                            new fPoint(v2.X, v2.Y),
                            new fPoint(v3.X, v3.Y)
                            );
                }
                if (b)
                {
                    var f = new FlatWithPositions();
                    f.FlatId = flats.IndexOf(flat);
                    foreach (var posi in flat.Points)
                    {
                        var vert = vertices[posi.VertexId - 1];
                        f.Positions.Add(new Position3D
                        {
                            X = vert.X / multiplicator,
                            Y = vert.Y / multiplicator * -1.0f,
                            Z = vert.Z / multiplicator,
                        });
                    }
                    result.Add(f);
                }
            }

            return result;
        }

        public List<FlatWithPositions> GetFlatBetweenXZ(int x, int z)
        {
            var result = new List<FlatWithPositions>();
            var p = new fPoint(x * multiplicator, z * multiplicator);

            foreach (var flat in flats)
            {
                bool b = false;
                if (flat.Points.Count() == 4)
                {
                    var v1 = vertices[flat.Points[0].VertexId - 1];
                    var v2 = vertices[flat.Points[1].VertexId - 1];
                    var v3 = vertices[flat.Points[2].VertexId - 1];
                    var v4 = vertices[flat.Points[3].VertexId - 1];

                    b = PointInTriangle(p,
                            new fPoint(v1.X, v1.Z),
                            new fPoint(v2.X, v2.Z),
                            new fPoint(v3.X, v3.Z)
                            ) &&
                        PointInTriangle(p,
                            new fPoint(v3.X, v3.Z),
                            new fPoint(v4.X, v4.Z),
                            new fPoint(v1.X, v1.Z)
                            );
                }
                else if (flat.Points.Count() == 3)
                {
                    var v1 = vertices[flat.Points[0].VertexId - 1];
                    var v2 = vertices[flat.Points[1].VertexId - 1];
                    var v3 = vertices[flat.Points[2].VertexId - 1];
                    b = PointInTriangle(p,
                            new fPoint(v1.X, v1.Z),
                            new fPoint(v2.X, v2.Z),
                            new fPoint(v3.X, v3.Z)
                            );
                }
                if (b)
                {
                    var f = new FlatWithPositions();
                    f.FlatId = flats.IndexOf(flat);
                    foreach (var posi in flat.Points)
                    {
                        var vert = vertices[posi.VertexId - 1];
                        f.Positions.Add(new Position3D
                        {
                            X = vert.X / multiplicator,
                            Y = vert.Y / multiplicator * -1.0f,
                            Z = vert.Z / multiplicator,
                        });
                    }
                    result.Add(f);
                }
            }

            return result;
        }

        public List<FlatWithPositions> GetFlatBetweenZY(int z, int y)
        {
            var result = new List<FlatWithPositions>();
            var p = new fPoint(z * multiplicator, y * multiplicator);

            foreach (var flat in flats)
            {
                bool b = false;
                if (flat.Points.Count() == 4)
                {
                    var v1 = vertices[flat.Points[0].VertexId - 1];
                    var v2 = vertices[flat.Points[1].VertexId - 1];
                    var v3 = vertices[flat.Points[2].VertexId - 1];
                    var v4 = vertices[flat.Points[3].VertexId - 1];

                    b = PointInTriangle(p,
                            new fPoint(v1.Z, v1.Y),
                            new fPoint(v2.Z, v2.Y),
                            new fPoint(v3.Z, v3.Y)
                            ) &&
                        PointInTriangle(p,
                            new fPoint(v3.Z, v3.Y),
                            new fPoint(v4.Z, v4.Y),
                            new fPoint(v1.Z, v1.Y)
                            );
                }
                else if (flat.Points.Count() == 3)
                {
                    var v1 = vertices[flat.Points[0].VertexId - 1];
                    var v2 = vertices[flat.Points[1].VertexId - 1];
                    var v3 = vertices[flat.Points[2].VertexId - 1];
                    b = PointInTriangle(p,
                            new fPoint(v1.Z, v1.Y),
                            new fPoint(v2.Z, v2.Y),
                            new fPoint(v3.Z, v3.Y)
                            );
                }
                if (b)
                {
                    var f = new FlatWithPositions();
                    f.FlatId = flats.IndexOf(flat);
                    foreach (var posi in flat.Points)
                    {
                        var vert = vertices[posi.VertexId - 1];
                        f.Positions.Add(new Position3D
                        {
                            X = vert.X / multiplicator,
                            Y = vert.Y / multiplicator * -1.0f,
                            Z = vert.Z / multiplicator,
                        });
                    }
                    result.Add(f);
                }
            }

            return result;
        }

        public void LoadData(IEnumerable<Element> elements, float multiplicator = 0.01f)
        {
            this.multiplicator = multiplicator;

            flats = new List<Flat>();
            textureCoordinates = new List<TextureCoordinate>();
            normals = new List<Normal>();
            vertices = new List<Vertex>();

            foreach (var element in elements)
            {
                if (element.GetType() == typeof(Cube))
                {
                    addFlatsForCube((Cube)element);
                }
                else if (element.GetType() == typeof(Sphere))
                {
                    addFlatsForSphere((Sphere)element);
                }
            }
            calcFlatNormals();
        }

        void addFlatsForCube(Cube element)
        {
            if (element.Positions.Count() != 8)
                throw new Exception("Corrupted cube! element.Positions.Count() != 8");
            addFlat(element.Id, element.Positions[0], element.Positions[1], element.Positions[2], element.Positions[3]);
            addFlat(element.Id, element.Positions[4], element.Positions[5], element.Positions[6], element.Positions[7]);
            addFlat(element.Id, element.Positions[0], element.Positions[4], element.Positions[5], element.Positions[1]);
            addFlat(element.Id, element.Positions[3], element.Positions[7], element.Positions[6], element.Positions[2]);
            addFlat(element.Id, element.Positions[0], element.Positions[4], element.Positions[7], element.Positions[3]);
            addFlat(element.Id, element.Positions[1], element.Positions[5], element.Positions[6], element.Positions[2]);
        }

        void addFlatsForSphere(Sphere element)
        {
            int f = 0;

            for (int i = 1; i < element.positionPerLevelCount; ++i)
            {
                addFlat(
                    element.Id,
                    element.Positions[0], 
                    element.Positions[i], 
                    element.Positions[i + 1]);
                f++;
            }
            addFlat(
                element.Id,
                element.Positions[0], 
                element.Positions[element.positionPerLevelCount], 
                element.Positions[1]);
            f++;

            for (int i = 0; i < element.level - 3; ++i)
            {
                var level = element.positionPerLevelCount * i + 1;
                for (int j = 0; j < element.positionPerLevelCount - 1; ++j)
                {
                    addFlat(
                        element.Id,
                        element.Positions[level + j],
                        element.Positions[level + j + element.positionPerLevelCount],
                        element.Positions[level + j + element.positionPerLevelCount + 1],
                       element.Positions[level + j + 1]);
                    f++;
                }
                addFlat(
                    element.Id,
                    element.Positions[level],
                    element.Positions[level + element.positionPerLevelCount],
                    element.Positions[level + element.positionPerLevelCount * 2 - 1],
                   element.Positions[level + element.positionPerLevelCount - 1]);
                f++;
            }


            for (int i = element.positionCount - element.positionPerLevelCount - 1; 
                i < element.positionCount - 1; 
                ++i)
            {
                addFlat(
                    element.Id,
                    element.Positions[i],
                    element.Positions[i + 1],
                    element.Positions[element.positionCount - 1]);
                f++;
            }
            addFlat(
                element.Id,
                element.Positions[element.positionCount - element.positionPerLevelCount - 1],
                element.Positions[element.positionCount - 2],
                element.Positions[element.positionCount - 1]);
            f++;
        }

        public void Export(string filename)
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
