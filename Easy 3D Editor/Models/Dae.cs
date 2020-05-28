/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName = "contributor", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Contributor
	{
		[XmlElement(ElementName = "author", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Author { get; set; }
		[XmlElement(ElementName = "authoring_tool", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Authoring_tool { get; set; }
	}

	[XmlRoot(ElementName = "unit", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Unit
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "meter")]
		public string Meter { get; set; }
	}

	[XmlRoot(ElementName = "asset", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Asset
	{
		[XmlElement(ElementName = "contributor", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Contributor Contributor { get; set; }
		[XmlElement(ElementName = "created", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Created { get; set; }
		[XmlElement(ElementName = "modified", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Modified { get; set; }
		[XmlElement(ElementName = "unit", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Unit Unit { get; set; }
		[XmlElement(ElementName = "up_axis", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Up_axis { get; set; }
	}

	[XmlRoot(ElementName = "xfov", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Xfov
	{
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "znear", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Znear
	{
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "zfar", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Zfar
	{
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "perspective", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Perspective
	{
		[XmlElement(ElementName = "xfov", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Xfov Xfov { get; set; }
		[XmlElement(ElementName = "aspect_ratio", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Aspect_ratio { get; set; }
		[XmlElement(ElementName = "znear", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Znear Znear { get; set; }
		[XmlElement(ElementName = "zfar", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Zfar Zfar { get; set; }
	}

	[XmlRoot(ElementName = "technique_common", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Technique_common
	{
		[XmlElement(ElementName = "perspective", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Perspective Perspective { get; set; }
		[XmlElement(ElementName = "accessor", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Accessor Accessor { get; set; }
		[XmlElement(ElementName = "instance_material", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Instance_material> Instance_material { get; set; }
	}

	[XmlRoot(ElementName = "optics", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Optics
	{
		[XmlElement(ElementName = "technique_common", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Technique_common Technique_common { get; set; }
	}

	[XmlRoot(ElementName = "technique", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Technique
	{
		[XmlElement(ElementName = "YF_dofdist", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string YF_dofdist { get; set; }
		[XmlElement(ElementName = "shiftx", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Shiftx { get; set; }
		[XmlElement(ElementName = "shifty", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Shifty { get; set; }
		[XmlAttribute(AttributeName = "profile")]
		public string Profile { get; set; }
		[XmlElement(ElementName = "phong", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Phong Phong { get; set; }
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
	}

	[XmlRoot(ElementName = "extra", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Extra
	{
		[XmlElement(ElementName = "technique", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Technique Technique { get; set; }
	}

	[XmlRoot(ElementName = "camera", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Camera
	{
		[XmlElement(ElementName = "optics", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Optics Optics { get; set; }
		[XmlElement(ElementName = "extra", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Extra Extra { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "library_cameras", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Library_cameras
	{
		[XmlElement(ElementName = "camera", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Camera Camera { get; set; }
	}

	[XmlRoot(ElementName = "color", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Color
	{
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "emission", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Emission
	{
		[XmlElement(ElementName = "color", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Color Color { get; set; }
	}

	[XmlRoot(ElementName = "ambient", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Ambient
	{
		[XmlElement(ElementName = "color", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Color Color { get; set; }
	}

	[XmlRoot(ElementName = "diffuse", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Diffuse
	{
		[XmlElement(ElementName = "color", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Color Color { get; set; }
	}

	[XmlRoot(ElementName = "specular", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Specular
	{
		[XmlElement(ElementName = "color", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Color Color { get; set; }
	}

	[XmlRoot(ElementName = "float", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Float
	{
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "shininess", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Shininess
	{
		[XmlElement(ElementName = "float", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Float Float { get; set; }
	}

	[XmlRoot(ElementName = "index_of_refraction", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Index_of_refraction
	{
		[XmlElement(ElementName = "float", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Float Float { get; set; }
	}

	[XmlRoot(ElementName = "phong", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Phong
	{
		[XmlElement(ElementName = "emission", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Emission Emission { get; set; }
		[XmlElement(ElementName = "ambient", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Ambient Ambient { get; set; }
		[XmlElement(ElementName = "diffuse", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Diffuse Diffuse { get; set; }
		[XmlElement(ElementName = "specular", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Specular Specular { get; set; }
		[XmlElement(ElementName = "shininess", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Shininess Shininess { get; set; }
		[XmlElement(ElementName = "index_of_refraction", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Index_of_refraction Index_of_refraction { get; set; }
	}

	[XmlRoot(ElementName = "profile_COMMON", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Profile_COMMON
	{
		[XmlElement(ElementName = "technique", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Technique Technique { get; set; }
	}

	[XmlRoot(ElementName = "effect", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Effect
	{
		[XmlElement(ElementName = "profile_COMMON", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Profile_COMMON Profile_COMMON { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "library_effects", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Library_effects
	{
		[XmlElement(ElementName = "effect", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Effect> Effect { get; set; }
	}

	[XmlRoot(ElementName = "instance_effect", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Instance_effect
	{
		[XmlAttribute(AttributeName = "url")]
		public string Url { get; set; }
	}

	[XmlRoot(ElementName = "material", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Material
	{
		[XmlElement(ElementName = "instance_effect", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Instance_effect Instance_effect { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "library_materials", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Library_materials
	{
		[XmlElement(ElementName = "material", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Material> Material { get; set; }
	}

	[XmlRoot(ElementName = "float_array", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Float_array
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "count")]
		public string Count { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "param", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Param
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName = "accessor", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Accessor
	{
		[XmlElement(ElementName = "param", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Param> Param { get; set; }
		[XmlAttribute(AttributeName = "source")]
		public string Source { get; set; }
		[XmlAttribute(AttributeName = "count")]
		public string Count { get; set; }
		[XmlAttribute(AttributeName = "stride")]
		public string Stride { get; set; }
	}

	[XmlRoot(ElementName = "source", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Source
	{
		[XmlElement(ElementName = "float_array", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Float_array Float_array { get; set; }
		[XmlElement(ElementName = "technique_common", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Technique_common Technique_common { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "input", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Input
	{
		[XmlAttribute(AttributeName = "semantic")]
		public string Semantic { get; set; }
		[XmlAttribute(AttributeName = "source")]
		public string Source { get; set; }
		[XmlAttribute(AttributeName = "offset")]
		public string Offset { get; set; }
		[XmlAttribute(AttributeName = "set")]
		public string Set { get; set; }
	}

	[XmlRoot(ElementName = "vertices", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Vertices
	{
		[XmlElement(ElementName = "input", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Input Input { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "polylist", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Polylist
	{
		[XmlElement(ElementName = "input", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Input> Input { get; set; }
		[XmlElement(ElementName = "vcount", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Vcount { get; set; }
		[XmlElement(ElementName = "p", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string P { get; set; }
		[XmlAttribute(AttributeName = "material")]
		public string Material { get; set; }
		[XmlAttribute(AttributeName = "count")]
		public string Count { get; set; }
	}

	[XmlRoot(ElementName = "mesh", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Mesh
	{
		[XmlElement(ElementName = "source", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Source> Source { get; set; }
		[XmlElement(ElementName = "vertices", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Vertices Vertices { get; set; }
		[XmlElement(ElementName = "polylist", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Polylist> Polylist { get; set; }
	}

	[XmlRoot(ElementName = "geometry", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Geometry
	{
		[XmlElement(ElementName = "mesh", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Mesh Mesh { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "library_geometries", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Library_geometries
	{
		[XmlElement(ElementName = "geometry", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Geometry> Geometry { get; set; }
	}

	[XmlRoot(ElementName = "matrix", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Matrix
	{
		[XmlAttribute(AttributeName = "sid")]
		public string Sid { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "instance_camera", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Instance_camera
	{
		[XmlAttribute(AttributeName = "url")]
		public string Url { get; set; }
	}

	[XmlRoot(ElementName = "node", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Node
	{
		[XmlElement(ElementName = "matrix", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Matrix Matrix { get; set; }
		[XmlElement(ElementName = "instance_camera", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Instance_camera Instance_camera { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlElement(ElementName = "instance_geometry", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Instance_geometry Instance_geometry { get; set; }
	}

	[XmlRoot(ElementName = "instance_material", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Instance_material
	{
		[XmlAttribute(AttributeName = "symbol")]
		public string Symbol { get; set; }
		[XmlAttribute(AttributeName = "target")]
		public string Target { get; set; }
	}

	[XmlRoot(ElementName = "bind_material", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Bind_material
	{
		[XmlElement(ElementName = "technique_common", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Technique_common Technique_common { get; set; }
	}

	[XmlRoot(ElementName = "instance_geometry", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Instance_geometry
	{
		[XmlElement(ElementName = "bind_material", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Bind_material Bind_material { get; set; }
		[XmlAttribute(AttributeName = "url")]
		public string Url { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "visual_scene", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Visual_scene
	{
		[XmlElement(ElementName = "node", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public List<Node> Node { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "library_visual_scenes", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Library_visual_scenes
	{
		[XmlElement(ElementName = "visual_scene", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Visual_scene Visual_scene { get; set; }
	}

	[XmlRoot(ElementName = "instance_visual_scene", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Instance_visual_scene
	{
		[XmlAttribute(AttributeName = "url")]
		public string Url { get; set; }
	}

	[XmlRoot(ElementName = "scene", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class Scene
	{
		[XmlElement(ElementName = "instance_visual_scene", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Instance_visual_scene Instance_visual_scene { get; set; }
	}

	[XmlRoot(ElementName = "COLLADA", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class COLLADA
	{
		[XmlElement(ElementName = "asset", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Asset Asset { get; set; }
		[XmlElement(ElementName = "library_cameras", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Library_cameras Library_cameras { get; set; }
		[XmlElement(ElementName = "library_images", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Library_images { get; set; }
		[XmlElement(ElementName = "library_effects", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Library_effects Library_effects { get; set; }
		[XmlElement(ElementName = "library_materials", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Library_materials Library_materials { get; set; }
		[XmlElement(ElementName = "library_geometries", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Library_geometries Library_geometries { get; set; }
		[XmlElement(ElementName = "library_controllers", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public string Library_controllers { get; set; }
		[XmlElement(ElementName = "library_visual_scenes", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Library_visual_scenes Library_visual_scenes { get; set; }
		[XmlElement(ElementName = "scene", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
		public Scene Scene { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
	}

}
