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


	m_HorizontalBlurShader = new HorizontalBlurShaderClass;
	if (!m_HorizontalBlurShader)
	{
		return false;
	}
	result = m_HorizontalBlurShader->Initialize(device, hwnd);
	if (!result)
	{
		return false;
	}


	m_VerticalBlurShader = new VerticalBlurShaderClass;
	if (!m_VerticalBlurShader)
	{
		return false;
	}
	result = m_VerticalBlurShader->Initialize(device, hwnd);
	if (!result)
	{
		return false;
	}


	m_noNormalTextureShader = new NoNormalTextureShaderClass;
	if (!m_noNormalTextureShader)
		return false;
	result = m_noNormalTextureShader->Initialize(device, hwnd);
	if (!result)
		return false;


	m_BumpMapShader = new BumpMapShaderClass;
	if (!m_BumpMapShader)
		return false;
	if (!m_BumpMapShader->Initialize(device, hwnd))
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

	if (m_HorizontalBlurShader)
	{
		m_HorizontalBlurShader->Shutdown();
		delete m_HorizontalBlurShader;
		m_HorizontalBlurShader = nullptr;
	}

	if (m_VerticalBlurShader)
	{
		m_VerticalBlurShader->Shutdown();
		delete m_VerticalBlurShader;
		m_VerticalBlurShader = nullptr;
	}

	if (m_BumpMapShader)
	{
		m_BumpMapShader->Shutdown();
		delete m_BumpMapShader;
		m_BumpMapShader = nullptr;
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
	return m_TextureShader->Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, texture1, texture2);
}

bool ShaderManagerClass::RenderNoNormalTextureShader(ID3D11DeviceContext* deviceContext, int indexCount, D3DXMATRIX worldMatrix, D3DXMATRIX viewMatrix,
	D3DXMATRIX projectionMatrix, ID3D11ShaderResourceView* texture)
{
	return m_noNormalTextureShader->Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, texture);
}

bool ShaderManagerClass::RenderBumpmapShader(ID3D11DeviceContext* deviceContext, int indexCount, D3DXMATRIX worldMatrix, D3DXMATRIX viewMatrix,
	D3DXMATRIX projectionMatrix, ID3D11ShaderResourceView* texture1, ID3D11ShaderResourceView* texture2)
{
	if (!texture2)
		texture2 = m_emptyBumpMap;
	ID3D11ShaderResourceView* textures[2] = { texture1 , texture2 };
	return m_BumpMapShader->Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, textures, *m_lightDirection, *m_diffuseColor);

}

HorizontalBlurShaderClass* ShaderManagerClass::GetHorizontalBlurShader()
{
	return m_HorizontalBlurShader;
}

VerticalBlurShaderClass* ShaderManagerClass::GetVerticalBlurShader()
{
	return m_VerticalBlurShader;
}
