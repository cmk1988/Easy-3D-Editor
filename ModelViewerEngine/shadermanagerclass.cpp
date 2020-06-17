////////////////////////////////////////////////////////////////////////////////
// Filename: shadermanagerclass.cpp
////////////////////////////////////////////////////////////////////////////////
#include "shadermanagerclass.h"


ShaderManagerClass::ShaderManagerClass()
{
	m_TextureShader = nullptr;
}


ShaderManagerClass::ShaderManagerClass(const ShaderManagerClass& other)
{
}


ShaderManagerClass::~ShaderManagerClass()
{
}


bool ShaderManagerClass::Initialize(ID3D11Device* device, HWND hwnd)
{
	auto tex = new TextureClass();
	tex->Initialize(device, L"textur/empty_map.dat");
	m_emptyBumpMap = tex->GetTexture();
	//tex->Shutdown();

	bool result;

	m_TextureShader = new TextureShaderClass;
	if(!m_TextureShader)
	{
		return false;
	}
	result = m_TextureShader->Initialize(device, hwnd);
	if(!result)
	{
		return false;
	}

	m_noNormalTextureShader = new NoNormalTextureShaderClass;
	if (!m_noNormalTextureShader)
		return false;
	result = m_noNormalTextureShader->Initialize(device, hwnd);
	if (!result)
		return false;

	m_diffuseColor = new D3DXVECTOR4(0.0f, 0.0f, 0.0f, 1.0f);
	m_lightDirection = new D3DXVECTOR3(0.0f, -0.707f, 0.707f);

	return true;
}


void ShaderManagerClass::Shutdown()
{

	// Release the texture shader object.
	if(m_TextureShader)
	{
		m_TextureShader->Shutdown();
		delete m_TextureShader;
		m_TextureShader = nullptr;
	}

	if (m_noNormalTextureShader)
	{
		m_noNormalTextureShader->Shutdown();
		delete m_noNormalTextureShader;
		m_noNormalTextureShader = nullptr;
	}

	if(m_diffuseColor)
		delete m_diffuseColor;
	if(m_lightDirection)
		delete m_lightDirection;

	return;
}


bool ShaderManagerClass::RenderTextureShader(ID3D11DeviceContext* deviceContext, int indexCount, D3DXMATRIX worldMatrix, D3DXMATRIX viewMatrix,
	D3DXMATRIX projectionMatrix, ID3D11ShaderResourceView* texture1, ID3D11ShaderResourceView* texture2)
{
	if (!texture2)
		texture2 = m_emptyBumpMap;
	return m_TextureShader->Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, texture1);
}
