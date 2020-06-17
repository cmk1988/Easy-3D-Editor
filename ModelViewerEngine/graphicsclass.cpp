////////////////////////////////////////////////////////////////////////////////
// Filename: graphicsclass.cpp
////////////////////////////////////////////////////////////////////////////////
#include "Graphics.h"

void _drehe_x(D3DXMATRIX* matrix, float f)
{
	D3DXMATRIX test;

	test._11 = 1.0;
	test._21 = 0;
	test._31 = 0;
	test._41 = 0;

	test._12 = 0;
	test._22 = cos(f);
	test._32 = -sin(f);
	test._42 = 0;

	test._13 = 0;
	test._23 = sin(f);
	test._33 = cos(f);
	test._43 = 0;

	test._14 = 0;
	test._24 = 0;
	test._34 = 0;
	test._44 = 1.0;

	*matrix = *matrix * test;
}

void _drehe_y(D3DXMATRIX *matrix, float f)
{
	D3DXMATRIX test = *matrix;

	test._11 = cos(f);
	test._21 = 0;
	test._31 = sin(f);
	test._41 = 0;

	test._12 = 0;
	test._22 = 1.0;
	test._32 = 0;
	test._42 = 0;

	test._13 = -sin(f);
	test._23 = 0;
	test._33 = cos(f);
	test._43 = 0;

	test._14 = 0;
	test._24 = 0;
	test._34 = 0;
	test._44 = 1.0;

	*matrix = *matrix*test;
}

void _drehe_z(D3DXMATRIX* matrix, float f)
{
	D3DXMATRIX test = *matrix;

	test._11 = cos(f);
	test._21 = -sin(f);
	test._31 = 0;
	test._41 = 0;

	test._12 = sin(f);
	test._22 = cos(f);
	test._32 = 0;
	test._42 = 0;

	test._13 = 0;
	test._23 = 0;
	test._33 = 1.0;
	test._43 = 0;

	test._14 = 0;
	test._24 = 0;
	test._34 = 0;
	test._44 = 1.0;

	*matrix = *matrix * test;
}

GraphicsClass::GraphicsClass()
{
	m_D3D = 0;
	m_Camera = 0;
	m_Model = 0;
	m_ShaderManager = 0;
}


GraphicsClass::GraphicsClass(const GraphicsClass& other)
{
}


GraphicsClass::~GraphicsClass()
{
}


bool GraphicsClass::Initialize(int screenWidth, int screenHeight, HWND hwnd)
{
	bool result;

	m_D3D = new D3DClass;
	if(!m_D3D)
	{
		return false;
	}

	result = m_D3D->Initialize(screenWidth, screenHeight, VSYNC_ENABLED, hwnd, FULL_SCREEN, SCREEN_DEPTH, SCREEN_NEAR);
	if(!result)
	{
		MessageBox(hwnd, L"Could not initialize Direct3D.", L"Error", MB_OK);
		return false;
	}

	m_Camera = new CameraClass;
	if(!m_Camera)
	{
		return false;
	}

	m_Camera->SetPosition(0.0f, 0.1f, 0.0f);
	m_Camera->SetRotation(0.0f, 0.0f, 0.0f);

	m_models = Models::GetInstance(m_D3D->GetDevice());

	// Create the texture shader object.
	m_ShaderManager = new ShaderManagerClass;
	if(!m_ShaderManager)
	{
		return false;
	}

	// Initialize the texture shader object.
	result = m_ShaderManager->Initialize(m_D3D->GetDevice(), hwnd);
	if(!result)
	{
		MessageBox(hwnd, L"Could not initialize the texture shader object.", L"Error", MB_OK);
		return false;
	}

	return true;
}


void GraphicsClass::Shutdown()
{
	// Release the texture shader object.
	if(m_ShaderManager)
	{
		m_ShaderManager->Shutdown();
		delete m_ShaderManager;
		m_ShaderManager = 0;
	}

	// Release the camera object.
	if(m_Camera)
	{
		delete m_Camera;
		m_Camera = 0;
	}

	// Release the D3D object.
	if(m_D3D)
	{
		m_D3D->Shutdown();
		delete m_D3D;
		m_D3D = 0;
	}

	// Release the up sample render to texture object.
	if (m_UpSampleTexure)
	{
		m_UpSampleTexure->Shutdown();
		delete m_UpSampleTexure;
		m_UpSampleTexure = 0;
	}

	// Release the vertical blur render to texture object.
	if (m_VerticalBlurTexture)
	{
		m_VerticalBlurTexture->Shutdown();
		delete m_VerticalBlurTexture;
		m_VerticalBlurTexture = 0;
	}

	// Release the horizontal blur render to texture object.
	if (m_HorizontalBlurTexture)
	{
		m_HorizontalBlurTexture->Shutdown();
		delete m_HorizontalBlurTexture;
		m_HorizontalBlurTexture = 0;
	}

	// Release the down sample render to texture object.
	if (m_DownSampleTexure)
	{
		m_DownSampleTexure->Shutdown();
		delete m_DownSampleTexure;
		m_DownSampleTexure = 0;
	}

	// Release the render to texture object.
	if (m_RenderTexture)
	{
		m_RenderTexture->Shutdown();
		delete m_RenderTexture;
		m_RenderTexture = 0;
	}

	return;
}


bool GraphicsClass::Frame(HWND hwnd)
{
	bool result;

	result = Render(hwnd);
	if(!result)
	{
		return false;
	}

	return true;
}

bool GraphicsClass::Render(HWND hwnd)
{
	return RenderScene(hwnd);
}

bool GraphicsClass::RenderScene(HWND hwnd)
{
	D3DXMATRIX worldMatrix, viewMatrix, projectionMatrix;
	bool result;

	m_D3D->BeginScene(0.0f, 0.0f, 0.0f, 1.0f);

	m_Camera->Render();
	m_Camera->GetViewMatrix(viewMatrix);
	m_D3D->GetWorldMatrix(worldMatrix);
	m_D3D->GetProjectionMatrix(projectionMatrix);

	D3DVECTOR v;
	v = m_Camera->GetPosition();


	auto model = m_models->getModel();

	if (!model || !model->GetTexture())
		return true;

	model->Render(m_D3D->GetDeviceContext());


	result = m_ShaderManager->RenderTextureShader(m_D3D->GetDeviceContext(),
		model->GetIndexCount(),
		worldMatrix,
		viewMatrix,
		projectionMatrix,
		model->GetTexture());

	if(!result)
	{
		return false;
	}

	m_D3D->EndScene();

	return true;
}