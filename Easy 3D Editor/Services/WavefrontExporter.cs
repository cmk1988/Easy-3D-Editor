using Easy_3D_Editor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using Xml2CSharp;

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

            public int BoneId { get; set; } = 0;

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
            static int _id = 0;

            public int Id { get; }
            public int ModelId { get; }
            public List<FlatPoint> Points { get; set; } = new List<FlatPoint>();

            public Flat(int id)
            {
                Id = ++_id;
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

        int addTexture(Position2D posi)
        {
            return addTexture(posi.X, posi.Y);
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

        void addFlat(int id, Position3D a, Position3D b, Position3D c, Position2D ta, Position2D tb, Position2D tc)
        {
            var flat = new Flat(id);

            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(a.X, a.Y, a.Z),
                TextureId = addTexture(ta)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(b.X, b.Y, b.Z),
                TextureId = addTexture(tb)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(c.X, c.Y, c.Z),
                TextureId = addTexture(tc)
            });

            flats.Add(flat);
        }

        void addFlat(int id, Position3D a, Position3D b, Position3D c, Position3D d,
            Position2D ta, Position2D tb, Position2D tc, Position2D td)
        {
            var flat = new Flat(id);

            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(a.X, a.Y, a.Z),
                TextureId = addTexture(ta)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(b.X, b.Y, b.Z),
                TextureId = addTexture(tb)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(c.X, c.Y, c.Z),
                TextureId = addTexture(tc)
            });
            flat.Points.Add(new FlatPoint
            {
                VertexId = addVertex(d.X, d.Y, d.Z),
                TextureId = addTexture(td)
            });

            flats.Add(flat);
        }

        public void CalcRoundNormals(int id)
        {
            var _flats = flats.Where(x => x.ModelId == id).ToList();

            var _vertices = flats.SelectMany(x => x.Points.Select(y => y.VertexId)).Distinct().ToList();


            foreach (var vertex in _vertices)
            {
                var _normals = _flats.SelectMany(o => 
                    o.Points.Where(
                        b => b.VertexId == vertex)
                    .Select(b => b.NormalId))
                    .ToList();

                var x = 0.0f;
                var y = 0.0f;
                var z = 0.0f;


                foreach (var normal in _normals)
                {
                    x += normals[normal - 1].X;
                    y += normals[normal - 1].Y;
                    z += normals[normal - 1].Z;
                }

                _flats.SelectMany(o => o.Points)
                    .Where(o => o.VertexId == vertex)
                    .ToList()
                    .ForEach(o => o.NormalId = addNormal(x, y, z));
            }
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

        float offsetX;
        float offsetY;
        float offsetZ;

        public WavefrontExporter(
            IEnumerable<Element> elements,
            float multiplicator = 0.01f,
            float offsetX = 0.0f,
            float offsetY = 0.0f,
            float offsetZ = 0.0f)
        {
            this.elements = elements;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.offsetZ = offsetZ;

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

        public void SetTextureForElement(int id, TextureForFlat texture)
        {
            var element = elements.First(x => x.Id == id);
            var _flats = flats.Where(x => x.ModelId == id).ToList();
            if (element.GetType() == typeof(Cube))
            {
                foreach (var flat in _flats)
                {
                    SetTextureforFlat(flat.Id, texture);
                }
            }
            else if (element.GetType() == typeof(Sphere))
            {
                //var sphere = (Sphere)element;
                //var curY = 0.0f;
                //foreach (var posi in element.Positions)
                //{
                //    flats.Where(x => x.ModelId == id).ToList().ForEach(x => x.Points.ForEach(y => 
                //    {
                //        var tex = textureCoordinates[y.TextureId - 1];
                //        y.TextureId = addTexture(
                //            texture.Coordinates[0].X + tex.X * (texture.Coordinates[0].X - texture.Coordinates[1].X),

                //            );
                //    }));
                //}
            }
        }

        void calcTexturePosition(TextureForFlat texture, float _x, float _y, out float x, out float y)
        {
            var c1 = texture.Coordinates[0];
            var c2 = texture.Coordinates[1];
            var c3 = texture.Coordinates[2];
            var c4 = texture.Coordinates[3];

            var a1 = (c1.Y - c4.Y) / (c1.X - c4.X);
            var b1 = c4.Y;

            var a2 = (c2.Y - c1.Y) / (c2.X - c1.X);
            var b2 = c1.Y;

            var a3 = (c3.Y - c4.Y) / (c3.X - c4.X);
            var b3 = c4.Y;

            x = c4.X;
            y = 0.0f;
        }

        void setTextureforFlat(Flat flat, TextureForFlat texture)
        {
            if (flat.Points.Count == texture.Coordinates.Count)
            {
                for (int i = 0; i < flat.Points.Count; i++)
                {
                    var textureCoord = texture.Coordinates[i];
                    flat.Points[i].TextureId = addTexture(textureCoord.X, textureCoord.Y);
                }
            }
        }

        public void SetTextureforFlat(int id, TextureForFlat texture)
        {
            var flat = flats[id];
            setTextureforFlat(flat, texture);
        }

        public List<FlatWithPositions> GetFlatBetweenXY(int x, int y)
        {
            var result = new List<FlatWithPositions>();
            var p = new fPoint(x * multiplicator, y * multiplicator * -1.0f);

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
                else if (flat.Points.Count() == 3)
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

        IEnumerable<Element> elements;

        public void LoadData(IEnumerable<Element> elements, float multiplicator = 0.01f)
        {
            this.multiplicator = multiplicator;
            this.elements = elements;

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
            elements.Where(x => x.GetType() == typeof(Sphere) && ((Sphere)x).CalcRoundNormals)
                .ToList()
                .ForEach(x => CalcRoundNormals(x.Id));
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

            float alpha = 1.0f / element.level;
            float beta = 1.0f / element.positionPerLevelCount;

            Position2D texPosi1;
            Position2D texPosi2;
            Position2D texPosi3;
            Position2D texPosi4;

            for (int i = 1; i < element.positionPerLevelCount; ++i)
            {
                texPosi1 = new Position2D
                {
                    X = 0.5f,
                    Y = 0.0f
                };
                texPosi2 = new Position2D
                {
                    X = beta * (i-1),
                    Y = alpha
                };
                texPosi3 = new Position2D
                {
                    X = beta * (i),
                    Y = alpha
                };

                addFlat(
                    element.Id,
                    element.Positions[0],
                    element.Positions[i],
                    element.Positions[i + 1],
                    texPosi1,
                    texPosi2,
                    texPosi3);

                f++;
            }

            texPosi1 = new Position2D
            {
                X = 0.5f,
                Y = 0.0f
            };
            texPosi2 = new Position2D
            {
                X = beta * (element.positionPerLevelCount - 1),
                Y = alpha
            };
            texPosi3 = new Position2D
            {
                X = beta * element.positionPerLevelCount,
                Y = alpha
            };

            addFlat(
                element.Id,
                element.Positions[0],
                element.Positions[element.positionPerLevelCount],
                element.Positions[1],
                texPosi1,
                texPosi2,
                texPosi3);
            f++;

            for (int i = 0; i < element.level - 3; ++i)
            {
                var level = element.positionPerLevelCount * i + 1;
                for (int j = 0; j < element.positionPerLevelCount - 1; ++j)
                {

                    texPosi1 = new Position2D
                    {
                        X = beta * (j),
                        Y = alpha * (i + 1)
                    };
                    texPosi2 = new Position2D
                    {
                        X = beta * (j),
                        Y = alpha * (i + 2)
                    };
                    texPosi3 = new Position2D
                    {
                        X = beta * (j + 1),
                        Y = alpha * (i + 2)
                    };
                    texPosi4 = new Position2D
                    {
                        X = beta * (j + 1),
                        Y = alpha * (i + 1)
                    };

                    addFlat(
                        element.Id,
                        element.Positions[level + j],
                        element.Positions[level + j + element.positionPerLevelCount],
                        element.Positions[level + j + element.positionPerLevelCount + 1],
                        element.Positions[level + j + 1],
                        texPosi1,
                        texPosi2,
                        texPosi3,
                        texPosi4);
                    f++;
                }

                texPosi1 = new Position2D
                {
                    X = beta * element.positionPerLevelCount,
                    Y = alpha * (i + 1)
                };
                texPosi2 = new Position2D
                {
                    X = beta * element.positionPerLevelCount,
                    Y = alpha * (i + 2)
                };
                texPosi3 = new Position2D
                {
                    X = beta * (element.positionPerLevelCount - 1),
                    Y = alpha * (i + 2)
                };
                texPosi4 = new Position2D
                {
                    X = beta * (element.positionPerLevelCount - 1),
                    Y = alpha * (i + 1)
                };

                addFlat(
                    element.Id,
                    element.Positions[level],
                    element.Positions[level + element.positionPerLevelCount],
                    element.Positions[level + element.positionPerLevelCount * 2 - 1],
                    element.Positions[level + element.positionPerLevelCount - 1],
                    texPosi1,
                    texPosi2,
                    texPosi3,
                    texPosi4);
                f++;
            }

            var tx = 0;
            for (int i = element.positionCount - element.positionPerLevelCount - 1;
                i < element.positionCount - 1;
                ++i)
            {
                texPosi1 = new Position2D
                {
                    X = beta * tx,
                    Y = alpha * (i + 1)
                };
                texPosi2 = new Position2D
                {
                    X = beta * (tx + 1),
                    Y = alpha * (i + 1)
                };
                texPosi3 = new Position2D
                {
                    X = 0.5f,
                    Y = alpha * element.level
                };

                addFlat(
                    element.Id,
                    element.Positions[i],
                    element.Positions[i + 1],
                    element.Positions[element.positionCount - 1],
                    texPosi1,
                    texPosi2,
                    texPosi3);
                f++;
                tx++;
            }
            texPosi1 = new Position2D
            {
                X = beta * tx,
                Y = alpha * (element.positionPerLevelCount - 1)
            };
            texPosi2 = new Position2D
            {
                X = beta * (tx + 1),
                Y = alpha * (element.positionPerLevelCount - 1)
            };
            texPosi3 = new Position2D
            {
                X = 0.5f,
                Y = alpha * element.level
            };

            addFlat(
                element.Id,
                element.Positions[element.positionCount - element.positionPerLevelCount - 1],
                element.Positions[element.positionCount - 2],
                element.Positions[element.positionCount - 1],
                texPosi1,
                texPosi2,
                texPosi3);
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

            File.WriteAllText(ConfigLoader.Instance.Config.OutputPath + filename, content);
        }
    }
}
