
#ifndef _TEXTURESHADERCLASS_H_
#define _TEXTURESHADERCLASS_H_


#include <d3d11.h>

#include <d3dx10math.h>
#include <d3dx11async.h>
#include <fstream>
using namespace std;

class BinaryShader
{
public:

	BinaryShader()
	{
		m_size = 0;
		m_buffer = nullptr;
	}

	BinaryShader(ID3D10Blob* shader)
	{
		m_size = shader->GetBufferSize();
		m_buffer = new char[m_size];
		memcpy(m_buffer, shader->GetBufferPointer(), m_size);
	}

	BinaryShader(wstring filename)
	{
		Load(filename);
	}

	~BinaryShader()
	{
		m_size = 0;
		if (m_buffer)
		{
			delete[] m_buffer;
			m_buffer = nullptr;
		}
	}

	void static Save(wstring filename, ID3D10Blob* shader)
	{
		SIZE_T size = shader->GetBufferSize();
		ofstream of(filename, std::ios::binary);
		of.write((const char*)&size, sizeof(SIZE_T));
		of.write((const char*)shader->GetBufferPointer(), shader->GetBufferSize());
	}

	void Save(wstring filename)
	{
		ofstream of(filename, std::ios::binary);
		of.write((const char*)&m_size, sizeof(SIZE_T));
		of.write((const char*)m_buffer, m_size);
	}

	void Load(wstring filename)
	{
		ifstream ifs(filename, std::ios::binary);
		ifs.read((char*)&m_size, sizeof(SIZE_T));
		m_buffer = new char[m_size];
		ifs.read((char*)m_buffer, m_size);
	}

	char* GetBufferPointer()
	{
		return m_buffer;
	}

	SIZE_T GetSize()
	{
		return m_size;
	}

private:
	SIZE_T m_size;
	char* m_buffer;

};

class TextureShaderClass
{
private:
	struct MatrixBufferType
	{
		D3DXMATRIX world;
		D3DXMATRIX view;
		D3DXMATRIX projection;
	};

	struct LightBufferType
	{
		D3DXVECTOR4 diffuseColor;
		D3DXVECTOR3 lightDirection;
		float padding;
		D3DXVECTOR4 ambi;
	};

public:
	TextureShaderClass();
	TextureShaderClass(const TextureShaderClass&);
	~TextureShaderClass();

	bool Initialize(ID3D11Device*, HWND);
	bool Initialize(ID3D11Device*, HWND, const wchar_t*, const wchar_t*);
	void Shutdown();
	bool Render(ID3D11DeviceContext*, int, D3DXMATRIX, D3DXMATRIX, D3DXMATRIX, ID3D11ShaderResourceView*, ID3D11ShaderResourceView*);

private:
	bool InitializeShader(ID3D11Device*, HWND, const wchar_t*, const wchar_t*);
	void ShutdownShader();
	void OutputShaderErrorMessage(ID3D10Blob*, HWND, const wchar_t*);

	bool SetShaderParameters(ID3D11DeviceContext*, D3DXMATRIX, D3DXMATRIX, D3DXMATRIX, ID3D11ShaderResourceView*, ID3D11ShaderResourceView*);
	void RenderShader(ID3D11DeviceContext*, int);

	void Save(wstring filename, ID3D10Blob* shader);
	void Load(wstring filename, BinaryShader& shader);

private:
	ID3D11VertexShader* m_vertexShader;
	ID3D11PixelShader* m_pixelShader;
	ID3D11InputLayout* m_layout;
	ID3D11Buffer* m_matrixBuffer;
	ID3D11Buffer* m_lightBuffer;

	ID3D11SamplerState* m_sampleState;
};

#endif