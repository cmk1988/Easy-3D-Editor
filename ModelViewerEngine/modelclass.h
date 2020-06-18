#ifndef _MODELCLASS_H_
#define _MODELCLASS_H_

#include <d3d11.h>
#include <d3dx10math.h>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
using namespace std;

#include "Textures.h"
#include "StringHelpers.h"

class ObjIndices
{
public:
	int iPosition = 0;
	int iTexture = 0;
	int iNormal = 0;
};

class ModelClass
{
private:
	struct VertexType
	{
		D3DXVECTOR3 position;
		D3DXVECTOR2 texture;
		D3DXVECTOR3 normal;
		D3DXVECTOR3 tangent;
		D3DXVECTOR3 binormal;
	};

	struct ModelType
	{
		float x, y, z;
		float tu, tv;
		float nx, ny, nz;
		float tx, ty, tz;
		float bx, by, bz;
	};	
	
	struct TempVertexType
	{
		float x, y, z;
		float tu, tv;
		float nx, ny, nz;
	};

	struct VectorType
	{
		float x, y, z;
	};

public:
	ModelClass();
	ModelClass(const ModelClass&);
	~ModelClass();

	bool Initialize(ID3D11Device*, std::wstring, std::wstring, std::wstring);
	void Shutdown();
	void Render(ID3D11DeviceContext*);
	void Save(std::wstring filename);
	void Load(std::wstring filename);

	int GetIndexCount();
	ID3D11ShaderResourceView* GetTexture();
	ID3D11ShaderResourceView* GetTexture2();


private:
	bool InitializeBuffers(ID3D11Device*);
	void ShutdownBuffers();
	void RenderBuffers(ID3D11DeviceContext*);

	bool LoadTexture(ID3D11Device*, const wchar_t*, const wchar_t*);
	void ReleaseTexture();

	bool LoadModel(std::wstring);
	void ReleaseModel();

	void CalculateModelVectors();
	bool LoadModelObj(std::wstring filename, float multiplicator = 1.0f);
	void CalculateTangentBinormal(TempVertexType, TempVertexType, TempVertexType, VectorType&, VectorType&);
	void CalculateNormal(VectorType, VectorType, VectorType&);
	ObjIndices getIndices(string str);
	void loadTriangle(int i_f, float** f_v, float** f_vt, const ObjIndices &index1, const ObjIndices& index2, const ObjIndices& index3, float multiplicator);

private:
	bool isNormalLoaded = false;
	ID3D11Buffer *m_vertexBuffer, *m_indexBuffer;
	int m_vertexCount, m_indexCount;
	TextureClass* m_Texture;
	TextureClass* m_Texture2;
	ModelType* m_model;
};

#endif