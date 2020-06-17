////////////////////////////////////////////////////////////////////////////////
// Filename: modelclass.cpp
////////////////////////////////////////////////////////////////////////////////
#include "modelclass.h"

ModelClass::ModelClass()
{
	m_vertexBuffer = nullptr;
	m_indexBuffer = nullptr;
	m_Texture = nullptr;
	m_Texture2 = nullptr;
	m_model = nullptr;

	m_indexCount = 0;
	m_vertexCount = 0;
}


ModelClass::ModelClass(const ModelClass& other)
{
}


ModelClass::~ModelClass()
{
	if (m_Texture)
	{
		delete m_Texture;
		m_Texture = nullptr;
	}
	if (m_Texture2)
	{
		delete m_Texture2;
		m_Texture2 = nullptr;
	}
	if (m_model)
	{
		delete[] m_model;
		m_model = nullptr;
	}
}

bool endsWith(std::wstring const& fullString, std::wstring const& ending) {
	if (fullString.length() >= ending.length()) {
		return (0 == fullString.compare(fullString.length() - ending.length(), ending.length(), ending));
	}
	else {
		return false;
	}
}

bool exists(const std::wstring& name) {
	ifstream f(name.c_str());
	return f.good();
}

bool ModelClass::Initialize(ID3D11Device* device, std::wstring modelFilename, std::wstring textureFilename, std::wstring textureFilename2)
{
	bool result;


	// Load in the model data,
	bool isLoaded = false;
	if (endsWith(modelFilename, L".bin"))
	{
		Load(modelFilename);
		isLoaded = true;
	}
	else if (endsWith(modelFilename, L".obj"))
	{
		result = LoadModelObj(modelFilename, 0.02f);
		if (!result)
		{
			return false;
		}
	}
	else
	{
		result = LoadModel(modelFilename);
		if (!result)
		{
			return false;
		}
	}

	// Calculate the normal, tangent, and binormal vectors for the model.
	if(!isLoaded)
		CalculateModelVectors();

	if (!isLoaded && !exists(modelFilename + L".bin"))
		Save(modelFilename + L".bin");

	// Initialize the vertex and index buffers.
	result = InitializeBuffers(device);
	if(!result)
	{
		return false;
	}

	// Load the texture for this model.
	result = LoadTexture(device, textureFilename.c_str(), textureFilename2.c_str());
	if(!result)
	{
		return false;
	}

	return true;
}


void ModelClass::Shutdown()
{
	// Release the model texture.
	ReleaseTexture();

	// Shutdown the vertex and index buffers.
	ShutdownBuffers();

	// Release the model data.
	ReleaseModel();

	return;
}


void ModelClass::Render(ID3D11DeviceContext* deviceContext)
{
	// Put the vertex and index buffers on the graphics pipeline to prepare them for drawing.
	RenderBuffers(deviceContext);

	return;
}

void ModelClass::Save(std::wstring filename)
{
	ofstream of(filename, std::ios::binary);
	of.write((const char*)&m_vertexCount, sizeof(m_vertexCount));
	of.write((const char*)m_model, sizeof(ModelType) * m_vertexCount);
}

void ModelClass::Load(std::wstring filename)
{
	ifstream ifs(filename, std::ios::binary);
	ifs.read((char*)&m_vertexCount, sizeof(m_vertexCount));
	m_model = new ModelType[m_vertexCount];
	ifs.read((char*)m_model, sizeof(ModelType) * m_vertexCount);
	m_indexCount = m_vertexCount;
}


int ModelClass::GetIndexCount()
{
	return m_indexCount;
}


ID3D11ShaderResourceView* ModelClass::GetTexture()
{
	return m_Texture->GetTexture();
}


ID3D11ShaderResourceView* ModelClass::GetTexture2()
{
	return m_Texture2->GetTexture();
}


bool ModelClass::InitializeBuffers(ID3D11Device* device)
{
	VertexType* vertices;
	unsigned long* indices;
	D3D11_BUFFER_DESC vertexBufferDesc, indexBufferDesc;
	D3D11_SUBRESOURCE_DATA vertexData, indexData;
	HRESULT result;
	int i;


	// Create the vertex array.
	vertices = new VertexType[m_vertexCount];
	if (!vertices)
	{
		return false;
	}

	// Create the index array.
	indices = new unsigned long[m_indexCount];
	if (!indices)
	{
		return false;
	}

	// Load the vertex array and index array with data.
	for (i = 0; i < m_vertexCount; i++)
	{
		vertices[i].position = D3DXVECTOR3(m_model[i].x, m_model[i].y, m_model[i].z);
		vertices[i].texture = D3DXVECTOR2(m_model[i].tu, m_model[i].tv);
		vertices[i].normal = D3DXVECTOR3(m_model[i].nx, m_model[i].ny, m_model[i].nz);
		vertices[i].tangent = D3DXVECTOR3(m_model[i].tx, m_model[i].ty, m_model[i].tz);
		vertices[i].binormal = D3DXVECTOR3(m_model[i].bx, m_model[i].by, m_model[i].bz);

		indices[i] = i;
	}

	// Set up the description of the static vertex buffer.
	vertexBufferDesc.Usage = D3D11_USAGE_DEFAULT;
	vertexBufferDesc.ByteWidth = sizeof(VertexType) * m_vertexCount;
	vertexBufferDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	vertexBufferDesc.CPUAccessFlags = 0;
	vertexBufferDesc.MiscFlags = 0;
	vertexBufferDesc.StructureByteStride = 0;

	// Give the subresource structure a pointer to the vertex data.
	vertexData.pSysMem = vertices;
	vertexData.SysMemPitch = 0;
	vertexData.SysMemSlicePitch = 0;

	// Now create the vertex buffer.
	result = device->CreateBuffer(&vertexBufferDesc, &vertexData, &m_vertexBuffer);
	if (FAILED(result))
	{
		return false;
	}

	// Set up the description of the static index buffer.
	indexBufferDesc.Usage = D3D11_USAGE_DEFAULT;
	indexBufferDesc.ByteWidth = sizeof(unsigned long) * m_indexCount;
	indexBufferDesc.BindFlags = D3D11_BIND_INDEX_BUFFER;
	indexBufferDesc.CPUAccessFlags = 0;
	indexBufferDesc.MiscFlags = 0;
	indexBufferDesc.StructureByteStride = 0;

	// Give the subresource structure a pointer to the index data.
	indexData.pSysMem = indices;
	indexData.SysMemPitch = 0;
	indexData.SysMemSlicePitch = 0;

	// Create the index buffer.
	result = device->CreateBuffer(&indexBufferDesc, &indexData, &m_indexBuffer);
	if (FAILED(result))
	{
		return false;
	}

	// Release the arrays now that the vertex and index buffers have been created and loaded.
	delete[] vertices;
	vertices = 0;

	delete[] indices;
	indices = 0;

	return true;
}


void ModelClass::ShutdownBuffers()
{
	// Release the index buffer.
	if(m_indexBuffer)
	{
		m_indexBuffer->Release();
		m_indexBuffer = 0;
	}

	// Release the vertex buffer.
	if(m_vertexBuffer)
	{
		m_vertexBuffer->Release();
		m_vertexBuffer = 0;
	}

	return;
}


void ModelClass::RenderBuffers(ID3D11DeviceContext* deviceContext)
{
	unsigned int stride;
	unsigned int offset;


	// Set vertex buffer stride and offset.
	stride = sizeof(VertexType); 
	offset = 0;
    
	// Set the vertex buffer to active in the input assembler so it can be rendered.
	deviceContext->IASetVertexBuffers(0, 1, &m_vertexBuffer, &stride, &offset);

    // Set the index buffer to active in the input assembler so it can be rendered.
	deviceContext->IASetIndexBuffer(m_indexBuffer, DXGI_FORMAT_R32_UINT, 0);

    // Set the type of primitive that should be rendered from this vertex buffer, in this case triangles.
	deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
	//deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_LINELIST);

	return;
}


bool ModelClass::LoadTexture(ID3D11Device* device, const wchar_t* filename, const wchar_t* filename2)
{
	bool result;


	// Create the texture object.
	m_Texture = Textures::GetInstance(device)->GetOrSetTexture(filename);// new TextureClass;
	//if (!m_Texture)
	//{
	//	return false;
	//}

	//// Initialize the texture object.
	//result = m_Texture->Initialize(device, filename);
	//if (!result)
	//{
	//	return false;
	//}


	// Create the texture object.
	m_Texture2 = Textures::GetInstance(device)->GetOrSetTexture(filename2);// new TextureClass;
	//m_Texture2 = new TextureClass;
	//if(!m_Texture2)
	//{
	//	return false;
	//}

	//// Initialize the texture object.
	//result = m_Texture2->Initialize(device, filename2);
	//if(!result)
	//{
	//	return false;
	//}

	return true;
}


void ModelClass::ReleaseTexture()
{
	// Release the texture object.
	if(m_Texture)
	{
		m_Texture->Shutdown();
		delete m_Texture;
		m_Texture = 0;
	}

	if (m_Texture2)
	{
		m_Texture2->Shutdown();
		delete m_Texture2;
		m_Texture2 = 0;
	}

	return;
}


bool ModelClass::LoadModel(std::wstring filename)
{
	ifstream fin;
	char input;
	int i;


	// Open the model file.
	fin.open(filename);
	
	// If it could not open the file then exit.
	if(fin.fail())
	{
		return false;
	}

	// Read up to the value of vertex count.
	fin.get(input);
	while(input != ':')
	{
		fin.get(input);
	}

	// Read in the vertex count.
	fin >> m_vertexCount;

	// Set the number of indices to be the same as the vertex count.
	m_indexCount = m_vertexCount;

	// Create the model using the vertex count that was read in.
	m_model = new ModelType[m_vertexCount];
	if(!m_model)
	{
		return false;
	}

	// Read up to the beginning of the data.
	fin.get(input);
	while(input != ':')
	{
		fin.get(input);
	}
	fin.get(input);
	fin.get(input);

	// Read in the vertex data.
	for(i=0; i<m_vertexCount; i++)
	{
		fin >> m_model[i].x >> m_model[i].y >> m_model[i].z;
		fin >> m_model[i].tu >> m_model[i].tv;
		fin >> m_model[i].nx >> m_model[i].ny >> m_model[i].nz;
	}

	// Close the model file.
	fin.close();

	return true;
}


void ModelClass::ReleaseModel()
{
	if(m_model)
	{
		delete [] m_model;
		m_model = 0;
	}

	return;
}

void ModelClass::CalculateTangentBinormal(TempVertexType vertex1, TempVertexType vertex2, TempVertexType vertex3,
	VectorType& tangent, VectorType& binormal)
{
	float vector1[3], vector2[3];
	float tuVector[2], tvVector[2];
	float den;
	float length;


	// Calculate the two vectors for this face.
	vector1[0] = vertex2.x - vertex1.x;
	vector1[1] = vertex2.y - vertex1.y;
	vector1[2] = vertex2.z - vertex1.z;

	vector2[0] = vertex3.x - vertex1.x;
	vector2[1] = vertex3.y - vertex1.y;
	vector2[2] = vertex3.z - vertex1.z;

	// Calculate the tu and tv texture space vectors.
	tuVector[0] = vertex2.tu - vertex1.tu;
	tvVector[0] = vertex2.tv - vertex1.tv;

	tuVector[1] = vertex3.tu - vertex1.tu;
	tvVector[1] = vertex3.tv - vertex1.tv;

	// Calculate the denominator of the tangent/binormal equation.
	den = 1.0f / (tuVector[0] * tvVector[1] - tuVector[1] * tvVector[0]);

	// Calculate the cross products and multiply by the coefficient to get the tangent and binormal.
	tangent.x = (tvVector[1] * vector1[0] - tvVector[0] * vector2[0]) * den;
	tangent.y = (tvVector[1] * vector1[1] - tvVector[0] * vector2[1]) * den;
	tangent.z = (tvVector[1] * vector1[2] - tvVector[0] * vector2[2]) * den;

	binormal.x = (tuVector[0] * vector2[0] - tuVector[1] * vector1[0]) * den;
	binormal.y = (tuVector[0] * vector2[1] - tuVector[1] * vector1[1]) * den;
	binormal.z = (tuVector[0] * vector2[2] - tuVector[1] * vector1[2]) * den;

	// Calculate the length of this normal.
	length = sqrt((tangent.x * tangent.x) + (tangent.y * tangent.y) + (tangent.z * tangent.z));

	// Normalize the normal and then store it
	tangent.x = tangent.x / length;
	tangent.y = tangent.y / length;
	tangent.z = tangent.z / length;

	// Calculate the length of this normal.
	length = sqrt((binormal.x * binormal.x) + (binormal.y * binormal.y) + (binormal.z * binormal.z));

	// Normalize the normal and then store it
	binormal.x = binormal.x / length;
	binormal.y = binormal.y / length;
	binormal.z = binormal.z / length;

	return;
}

void ModelClass::CalculateNormal(VectorType tangent, VectorType binormal, VectorType& normal)
{
	float length;


	// Calculate the cross product of the tangent and binormal which will give the normal vector.
	normal.x = (tangent.y * binormal.z) - (tangent.z * binormal.y);
	normal.y = (tangent.z * binormal.x) - (tangent.x * binormal.z);
	normal.z = (tangent.x * binormal.y) - (tangent.y * binormal.x);

	// Calculate the length of the normal.
	length = sqrt((normal.x * normal.x) + (normal.y * normal.y) + (normal.z * normal.z));

	// Normalize the normal.
	normal.x = normal.x / length;
	normal.y = normal.y / length;
	normal.z = normal.z / length;

	return;
}

void ModelClass::CalculateModelVectors()
{
	int faceCount, i, index;
	TempVertexType vertex1, vertex2, vertex3;
	VectorType tangent, binormal, normal;


	// Calculate the number of faces in the model.
	faceCount = m_vertexCount / 3;

	// Initialize the index to the model data.
	index = 0;

	// Go through all the faces and calculate the the tangent, binormal, and normal vectors.
	for (i = 0; i < faceCount; i++)
	{
		// Get the three vertices for this face from the model.
		vertex1.x = m_model[index].x;
		vertex1.y = m_model[index].y;
		vertex1.z = m_model[index].z;
		vertex1.tu = m_model[index].tu;
		vertex1.tv = m_model[index].tv;
		vertex1.nx = m_model[index].nx;
		vertex1.ny = m_model[index].ny;
		vertex1.nz = m_model[index].nz;
		index++;

		vertex2.x = m_model[index].x;
		vertex2.y = m_model[index].y;
		vertex2.z = m_model[index].z;
		vertex2.tu = m_model[index].tu;
		vertex2.tv = m_model[index].tv;
		vertex2.nx = m_model[index].nx;
		vertex2.ny = m_model[index].ny;
		vertex2.nz = m_model[index].nz;
		index++;

		vertex3.x = m_model[index].x;
		vertex3.y = m_model[index].y;
		vertex3.z = m_model[index].z;
		vertex3.tu = m_model[index].tu;
		vertex3.tv = m_model[index].tv;
		vertex3.nx = m_model[index].nx;
		vertex3.ny = m_model[index].ny;
		vertex3.nz = m_model[index].nz;
		index++;

		// Calculate the tangent and binormal of that face.
		CalculateTangentBinormal(vertex1, vertex2, vertex3, tangent, binormal);

		// Calculate the new normal using the tangent and binormal.
		CalculateNormal(tangent, binormal, normal);

		// Store the normal, tangent, and binormal for this face back in the model structure.
		m_model[index - 1].nx = normal.x;
		m_model[index - 1].ny = normal.y;
		m_model[index - 1].nz = normal.z;
		m_model[index - 1].tx = tangent.x;
		m_model[index - 1].ty = tangent.y;
		m_model[index - 1].tz = tangent.z;
		m_model[index - 1].bx = binormal.x;
		m_model[index - 1].by = binormal.y;
		m_model[index - 1].bz = binormal.z;

		m_model[index - 2].nx = normal.x;
		m_model[index - 2].ny = normal.y;
		m_model[index - 2].nz = normal.z;
		m_model[index - 2].tx = tangent.x;
		m_model[index - 2].ty = tangent.y;
		m_model[index - 2].tz = tangent.z;
		m_model[index - 2].bx = binormal.x;
		m_model[index - 2].by = binormal.y;
		m_model[index - 2].bz = binormal.z;

		m_model[index - 3].nx = normal.x;
		m_model[index - 3].ny = normal.y;
		m_model[index - 3].nz = normal.z;
		m_model[index - 3].tx = tangent.x;
		m_model[index - 3].ty = tangent.y;
		m_model[index - 3].tz = tangent.z;
		m_model[index - 3].bx = binormal.x;
		m_model[index - 3].by = binormal.y;
		m_model[index - 3].bz = binormal.z;
	}

	return;
}

//std::vector<std::string> split(std::string s, std::string delimiter = " ")
//{
//	std::vector<std::string> v;
//
//	size_t pos = 0;
//	std::string token;
//	while ((pos = s.find(delimiter)) != std::string::npos) {
//		token = s.substr(0, pos);
//		v.push_back(token);
//		s.erase(0, pos + delimiter.length());
//	}
//	v.push_back(s);
//	return v;
//}

bool ModelClass::LoadModelObj(std::wstring filename, float multiplicator)
{
	string w1, w2, w3, w4;

	ifstream fin;
	char input;
	int i_v = 0;
	int i_vt = 0;
	int i_vn = 0;
	int i_f = 0;

	float** f_v;
	float** f_vt;
	float** f_vn;

	// Open the model file.
	fin.open(filename);

	// If it could not open the file then exit.
	if (fin.fail())
	{
		return false;
	}

	string word = "";

	while (std::getline(fin, word))
	{
		if (word[0] == 'v' && word[1] == ' ')
		{
			i_v++;
		}
		if (word[0] == 'v' && word[1] == 't')
		{
			i_vt++;
		}
		if (word[0] == 'v' && word[1] == 'n')
		{
			i_vn++;
		}
		if (word[0] == 'f' && word[1] == ' ')
		{
			auto sts = split(word, " ");
			i_f++;
			if (sts.size() > 4 && sts[4] != "")
			{
				i_f++;
				int iii = 0;
			}
		}
	}

	i_f *= 2;

	fin.clear();
	fin.seekg(0, ios::beg);
	fin.close();
	// Open the model file.
	fin.open(filename);

	// If it could not open the file then exit.
	if (fin.fail())
	{
		return false;
	}

	m_vertexCount = i_f * 3;

	f_v = new float* [i_v];
	for (size_t i = 0; i < i_v; i++)
	{
		f_v[i] = new float[3];
	}

	f_vt = new float* [i_vt];
	for (size_t i = 0; i < i_vt; i++)
	{
		f_vt[i] = new float[2];
	}

	f_vn = new float* [i_vn];
	for (size_t i = 0; i < i_vn; i++)
	{
		f_vn[i] = new float[3];
	}

	m_indexCount = m_vertexCount;
	m_model = new ModelType[m_vertexCount];
	if (!m_model)
	{
		return false;
	}

	i_v = 0;
	i_vt = 0;
	i_vn = 0;
	i_f = 0;

	float f_max = 0.0f;
	float f_min = 0.0f;

	string mtl;
	bool skip = false;
	while (fin >> word)
	{
		skip = true;
		if (word == "v" && word.length() == 1)
		{
			try
			{
				fin >> f_v[i_v][0] >> f_v[i_v][1] >> f_v[i_v][2];
				if (f_v[i_v][0] > f_max)
				{
					f_max = f_v[i_v][0];
				}
				if (f_v[i_v][0] < f_min)
				{
					f_min = f_v[i_v][0];
				}
				if (f_v[i_v][1] > f_max)
				{
					f_max = f_v[i_v][1];
				}
				if (f_v[i_v][1] < f_min)
				{
					f_min = f_v[i_v][1];
				}
				if (f_v[i_v][2] > f_max)
				{
					f_max = f_v[i_v][2];
				}
				if (f_v[i_v][2] < f_min)
				{
					f_min = f_v[i_v][2];
				}
				i_v++;
			}
			catch (...)
			{
				int test = 0;
			}

		}
		if (word == "vt")
		{
			fin >> f_vt[i_vt][0] >> f_vt[i_vt][1];
			i_vt++;
		}
		if (word == "vn")
		{
			fin >> f_vn[i_vn][0] >> f_vn[i_vn][1] >> f_vn[i_vn][2];
			i_vn++;
		}
		if (word == "usemtl")
		{
			fin >> mtl;
		}
		if (word == "f")
		{
			string wz1 = "", wz2 = "", wz3 = "";

			std::string line;
			std::getline(fin, line);
			auto words = split(line, " ");

			w1 = words[1];
			w2 = words[2];
			w3 = words[3];
			w4 = words.size() > 4 ? words[4] : "";

			auto index1 = getIndices(w1);
			auto index2 = getIndices(w2);
			auto index3 = getIndices(w3);

			loadTriangle(i_f, f_v, f_vt, index1, index2, index3, multiplicator);
			i_f++;

			loadTriangle(i_f, f_v, f_vt, index3, index2, index1, multiplicator);
			i_f++;

			if (w4 != "")
			{
				auto index4 = getIndices(w4);

				loadTriangle(i_f, f_v, f_vt, index3, index4, index1, multiplicator);
				i_f++;

				loadTriangle(i_f, f_v, f_vt, index1, index4, index3, multiplicator);
				i_f++;
			}
		}
	}
	fin.close();
	delete[] f_v;
	delete[] f_vt;
	delete[] f_vn;

	return true;
}

ObjIndices ModelClass::getIndices(string str)
{
	ObjIndices i;

	string wz1 = "";
	string wz2 = "";
	string wz3 = "";

	auto sp = split(str, "/");
	wz1 = sp[0];
	wz2 = sp[1];
	if (sp.size() > 2)
		wz3 = sp[2];

	i.iPosition = atoi(wz1.c_str());
	i.iTexture = atoi(wz2.c_str());
	i.iNormal = atoi(wz3.c_str());

	if (i.iTexture == 0)
	{
		int iii = 0;
	}

	return i;
}

void ModelClass::loadTriangle(int i_f, float** f_v, float** f_vt, const ObjIndices& index1, const ObjIndices& index2, const ObjIndices& index3, float multiplicator)
{
	u_int i = i_f * 3;

	m_model[i].x = f_v[index1.iPosition - 1][0] * multiplicator;
	m_model[i].y = f_v[index1.iPosition - 1][1] * multiplicator;
	m_model[i].z = f_v[index1.iPosition - 1][2] * multiplicator;
	m_model[i].tu = f_vt[index1.iTexture - 1][0];
	m_model[i].tv = f_vt[index1.iTexture - 1][1];

	++i;
	m_model[i].x = f_v[index2.iPosition - 1][0] * multiplicator;
	m_model[i].y = f_v[index2.iPosition - 1][1] * multiplicator;
	m_model[i].z = f_v[index2.iPosition - 1][2] * multiplicator;
	m_model[i].tu = f_vt[index2.iTexture - 1][0];
	m_model[i].tv = f_vt[index2.iTexture - 1][1];

	++i;
	m_model[i].x = f_v[index3.iPosition - 1][0] * multiplicator;
	m_model[i].y = f_v[index3.iPosition - 1][1] * multiplicator;
	m_model[i].z = f_v[index3.iPosition - 1][2] * multiplicator;
	m_model[i].tu = f_vt[index3.iTexture - 1][0];
	m_model[i].tv = f_vt[index3.iTexture - 1][1];
}
