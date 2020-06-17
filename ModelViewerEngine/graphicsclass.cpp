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

	// Create the Direct3D object.
	m_D3D = new D3DClass;
	if(!m_D3D)
	{
		return false;
	}


	// Initialize the Direct3D object.
	result = m_D3D->Initialize(screenWidth, screenHeight, VSYNC_ENABLED, hwnd, FULL_SCREEN, SCREEN_DEPTH, SCREEN_NEAR);
	if(!result)
	{
		MessageBox(hwnd, L"Could not initialize Direct3D.", L"Error", MB_OK);
		return false;
	}

	// Create the camera object.
	m_Camera = new CameraClass;
	if(!m_Camera)
	{
		return false;
	}

	// Set the initial position of the camera.
	m_Camera->SetPosition(0.0f, 0.1f, 0.0f);
	m_Camera->SetRotation(0.0f, 0.0f, 0.0f);

	//=====================

	m_models = Models::GetInstance(m_D3D->GetDevice());

	// Create the model object.

	m_models->GetOrSetModel(L"modell/skydome.txt.bin", L"textur/vp_sky_v2_002.png", L"textur/empty_map.dat");

	for (size_t i = 0; i < g->kkk->get_x()*g->kkk->get_y(); i++)
	{
		auto id = g->kkk->get_element(i)->get_art();
		auto mc = g->kkk->m_ModelCombinations[id];
		for (auto & element : mc.Models)
		{
			m_models->GetOrSetModel(
				g->kkk->get_modellname(element),
				g->kkk->get_texturname(element),
				g->kkk->get_bumpmapname(element));
			
			if (!result)
			{
				MessageBox(hwnd, L"Could not initialize the model object.", L"Error", MB_OK);
				return false;
			}
			
		}
	}
	for (size_t i = 0; i < 4; i++)
	{
		for (size_t j = 0; j < g->figuren[i].get_modell_anzahl(); j++)
		{
			m_models->GetOrSetModel(
				g->figuren[i].get_modellname(j),
				g->figuren[i].get_texturname(j),
				L"D:\\Develop\\Cpp\\3DEngine\\Engine\\textur\\mauer_NRM.dat");

			if (!result)
			{
				MessageBox(hwnd, L"Could not initialize the model object.", L"Error", MB_OK);
				return false;
			}
		}

	}

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

	InitBlurExtension(screenWidth, screenHeight, hwnd);

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

	// Release the full screen ortho window object.
	if (m_FullScreenWindow)
	{
		m_FullScreenWindow->Shutdown();
		delete m_FullScreenWindow;
		m_FullScreenWindow = 0;
	}

	// Release the small ortho window object.
	if (m_SmallWindow)
	{
		m_SmallWindow->Shutdown();
		delete m_SmallWindow;
		m_SmallWindow = 0;
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


bool GraphicsClass::Frame(game* g, HWND hwnd)
{
	bool result;


	// Render the graphics scene.
	result = Render(g,hwnd);
	if(!result)
	{
		return false;
	}

	return true;
}

bool GraphicsClass::Render(game* g, HWND hwnd)
{
	bool result;


	// First render the scene to a render texture.
	result = RenderSceneToTexture(g, hwnd);
	if (!result)
	{
		return false;
	}

	// Next down sample the render texture to a smaller sized texture.
	result = DownSampleTexture();
	if (!result)
	{
		return false;
	}

	// Perform a horizontal blur on the down sampled render texture.
	result = RenderHorizontalBlurToTexture();
	if (!result)
	{
		return false;
	}

	// Now perform a vertical blur on the horizontal blur render texture.
	result = RenderVerticalBlurToTexture();
	if (!result)
	{
		return false;
	}

	// Up sample the final blurred render texture to screen size again.
	result = UpSampleTexture();
	if (!result)
	{
		return false;
	}

	// Render the blurred up sampled render texture to the screen.
	result = Render2DTextureScene();
	if (!result)
	{
		return false;
	}

	return true;
}

bool GraphicsClass::RenderSceneToTexture(game* g, HWND hwnd)
{
	D3DXMATRIX worldMatrix, viewMatrix, projectionMatrix;
	bool result;


	// Set the render target to be the render to texture.
	m_RenderTexture->SetRenderTarget(m_D3D->GetDeviceContext());

	// Clear the render to texture.
	m_RenderTexture->ClearRenderTarget(m_D3D->GetDeviceContext(), 0.0f, 0.0f, 0.0f, 1.0f);

	// Clear the buffers to begin the scene.
	//m_D3D->BeginScene(0.0f, 0.0f, 0.0f, 1.0f);

	// Generate the view matrix based on the camera's position.
	m_Camera->Render();
	//m_Camera->SetRotation(0.0f, g->f_sehrichtung, 0.0f);

	// Get the world, view, and projection matrices from the camera and d3d objects.
	m_Camera->GetViewMatrix(viewMatrix);
	m_D3D->GetWorldMatrix(worldMatrix);
	m_D3D->GetProjectionMatrix(projectionMatrix);

	_drehe_y(&viewMatrix, g->f_richtung);
	_drehe_x(&viewMatrix, -g->f_sr);
	//_drehe_z(&viewMatrix, g->f_sr - 90.0f);// sin(g->f_sr * 3.14159265 / 180.0f));

	D3DVECTOR v;
	v = m_Camera->GetPosition();
	//m_Camera->SetPosition(g->f_x - 4.5*sin(g->f_geschwindigkeit), v.y, g->f_y - 4.5*cos(g->f_geschwindigkeit));
	//v = m_Camera->GetRotation();
	//m_Camera->SetRotation(v.x, g->f_geschwindigkeit / 0.0174532925f,v.z);

	auto sphere = m_models->GetOrSetModel(L"modell/skydome.txt.bin", L"textur/vp_sky_v2_002.png", L"textur/empty_map.dat");
	sphere->Render(m_D3D->GetDeviceContext());
	result = m_ShaderManager->RenderNoNormalTextureShader(m_D3D->GetDeviceContext(),
		sphere->GetIndexCount(),
		g->world,
		viewMatrix,
		projectionMatrix,
		sphere->GetTexture());
	
	for (int h = g->i_x - 30; h < g->i_x + 30; h++)
	for (int i = g->i_y - 30; i < g->i_y + 30; i++)
	{
		auto element = g->kkk->get_element(h, i);
		auto id = element ? element->get_art() : -1;
		if (id >= 0)
		{
			auto mc = g->kkk->m_ModelCombinations[id];
			for (auto& element : mc.Models)
			{
				auto model = m_models->GetOrSetModel(
					g->kkk->get_modellname(element),
					g->kkk->get_texturname(element),
					g->kkk->get_bumpmapname(element));
				model->Render(m_D3D->GetDeviceContext());

				//g->kkk->get_element(i)->get_modell(j)->Render(m_D3D->GetDeviceContext());

				result = m_ShaderManager->RenderTextureShader(m_D3D->GetDeviceContext(),
					model->GetIndexCount(),
					g->world * *g->kkk->get_element(h, i)->get_matrix(),
					viewMatrix,
					projectionMatrix,
					model->GetTexture(),
					model->GetTexture2());
				if (!result)
				{
					MessageBox(hwnd, L"Could not initialize the model object.", L"Error", MB_OK);
					return false;
				}
			}
		}
	}
	for (size_t i = 0; i < 4; i++)
	{
		for (size_t j = 0; j < g->figuren[i].get_modell_anzahl(); j++)
		{
			auto model = m_models->GetOrSetModel(
				g->figuren[i].get_modellname(j),
				g->figuren[i].get_texturname(j),
				L"D:\\Develop\\Cpp\\3DEngine\\Engine\\textur\\mauer_NRM.dat");

			model->Render(m_D3D->GetDeviceContext());
			result = m_ShaderManager->RenderTextureShader(m_D3D->GetDeviceContext(),
				model->GetIndexCount(),
				g->world*g->figuren[i].get_matrix(0),
				viewMatrix,
				projectionMatrix,
				model->GetTexture(),
				model->GetTexture2());
			if (!result)
			{
				MessageBox(hwnd, L"Could not initialize the model object.", L"Error", MB_OK);
				return false;
			}
		}

	}

	if(!result)
	{
		return false;
	}

	// Present the rendered scene to the screen.
	//m_D3D->EndScene();

	//ID3D11Resource *res;
	//m_RenderTexture->GetShaderResourceView()->GetResource(&res);
	//D3DX11SaveTextureToFileW(m_D3D->GetDeviceContext(), res, D3DX11_IMAGE_FILE_FORMAT::D3DX11_IFF_PNG, L"test.png");

		// Reset the render target back to the original back buffer and not the render to texture anymore.
	m_D3D->SetBackBufferRenderTarget();

	// Reset the viewport back to the original.
	m_D3D->ResetViewport();

	return true;
}

bool GraphicsClass::InitBlurExtension(int screenWidth, int screenHeight, HWND hwnd)
{
	bool result;	
	int downSampleWidth, downSampleHeight;

	// Set the size to sample down to.
	downSampleWidth = screenWidth;// / 1;// 1.1f;
	downSampleHeight = screenHeight;// / 1;//1.1f;

	// Create the render to texture object.
	m_RenderTexture = new RenderTextureClass;
	if (!m_RenderTexture)
	{
		return false;
	}

	// Initialize the render to texture object.
	result = m_RenderTexture->Initialize(m_D3D->GetDevice(), screenWidth, screenHeight, SCREEN_DEPTH, SCREEN_NEAR);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the render to texture object.", L"Error", MB_OK);
		return false;
	}

	// Create the down sample render to texture object.
	m_DownSampleTexure = new RenderTextureClass;
	if (!m_DownSampleTexure)
	{
		return false;
	}

	// Initialize the down sample render to texture object.
	result = m_DownSampleTexure->Initialize(m_D3D->GetDevice(), downSampleWidth, downSampleHeight, SCREEN_DEPTH, SCREEN_NEAR);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the down sample render to texture object.", L"Error", MB_OK);
		return false;
	}

	// Create the horizontal blur render to texture object.
	m_HorizontalBlurTexture = new RenderTextureClass;
	if (!m_HorizontalBlurTexture)
	{
		return false;
	}

	// Initialize the horizontal blur render to texture object.
	result = m_HorizontalBlurTexture->Initialize(m_D3D->GetDevice(), downSampleWidth, downSampleHeight, SCREEN_DEPTH, SCREEN_NEAR);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the horizontal blur render to texture object.", L"Error", MB_OK);
		return false;
	}

	// Create the vertical blur render to texture object.
	m_VerticalBlurTexture = new RenderTextureClass;
	if (!m_VerticalBlurTexture)
	{
		return false;
	}

	// Initialize the vertical blur render to texture object.
	result = m_VerticalBlurTexture->Initialize(m_D3D->GetDevice(), downSampleWidth, downSampleHeight, SCREEN_DEPTH, SCREEN_NEAR);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the vertical blur render to texture object.", L"Error", MB_OK);
		return false;
	}

	// Create the up sample render to texture object.
	m_UpSampleTexure = new RenderTextureClass;
	if (!m_UpSampleTexure)
	{
		return false;
	}

	// Initialize the up sample render to texture object.
	result = m_UpSampleTexure->Initialize(m_D3D->GetDevice(), screenWidth, screenHeight, SCREEN_DEPTH, SCREEN_NEAR);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the up sample render to texture object.", L"Error", MB_OK);
		return false;
	}

	// Create the small ortho window object.
	m_SmallWindow = new OrthoWindowClass;
	if (!m_SmallWindow)
	{
		return false;
	}

	// Initialize the small ortho window object.
	result = m_SmallWindow->Initialize(m_D3D->GetDevice(), downSampleWidth, downSampleHeight);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the small ortho window object.", L"Error", MB_OK);
		return false;
	}

	// Create the full screen ortho window object.
	m_FullScreenWindow = new OrthoWindowClass;
	if (!m_FullScreenWindow)
	{
		return false;
	}

	// Initialize the full screen ortho window object.
	result = m_FullScreenWindow->Initialize(m_D3D->GetDevice(), screenWidth, screenHeight);
	if (!result)
	{
		MessageBox(hwnd, L"Could not initialize the full screen ortho window object.", L"Error", MB_OK);
		return false;
	}
}

bool GraphicsClass::DownSampleTexture()
{
	D3DXMATRIX worldMatrix, viewMatrix, orthoMatrix;
	bool result;


	worldMatrix = CameraClass::GetIdentitymatrix();
	viewMatrix = CameraClass::GetIdentitymatrix();
	orthoMatrix = CameraClass::GetIdentitymatrix();

	// Set the render target to be the render to texture.
	m_DownSampleTexure->SetRenderTarget(m_D3D->GetDeviceContext());

	// Clear the render to texture.
	m_DownSampleTexure->ClearRenderTarget(m_D3D->GetDeviceContext(), 0.0f, 1.0f, 0.0f, 1.0f);

	// Generate the view matrix based on the camera's position.
	m_Camera->Render();

	// Get the world and view matrices from the camera and d3d objects.
	//m_Camera->GetViewMatrix(viewMatrix);
	//m_D3D->GetWorldMatrix(worldMatrix);

	// Get the ortho matrix from the render to texture since texture has different dimensions being that it is smaller.
	//m_DownSampleTexure->GetOrthoMatrix(orthoMatrix);

	// Turn off the Z buffer to begin all 2D rendering.
	m_D3D->TurnZBufferOff();

	// Put the small ortho window vertex and index buffers on the graphics pipeline to prepare them for drawing.
	m_SmallWindow->Render(m_D3D->GetDeviceContext());

	// Render the small ortho window using the texture shader and the render to texture of the scene as the texture resource.
	result = m_ShaderManager->RenderNoNormalTextureShader(m_D3D->GetDeviceContext(), m_SmallWindow->GetIndexCount(), worldMatrix, viewMatrix, orthoMatrix,
		m_RenderTexture->GetShaderResourceView());
	if (!result)
	{
		return false;
	}

	// Turn the Z buffer back on now that all 2D rendering has completed.
	m_D3D->TurnZBufferOn();

	// Reset the render target back to the original back buffer and not the render to texture anymore.
	m_D3D->SetBackBufferRenderTarget();

	// Reset the viewport back to the original.
	m_D3D->ResetViewport();

	return true;
}


bool GraphicsClass::RenderHorizontalBlurToTexture()
{
	D3DXMATRIX worldMatrix, viewMatrix, orthoMatrix;
	float screenSizeX;
	bool result;


	worldMatrix = CameraClass::GetIdentitymatrix();
	viewMatrix = CameraClass::GetIdentitymatrix();
	orthoMatrix = CameraClass::GetIdentitymatrix();

	// Store the screen width in a float that will be used in the horizontal blur shader.
	screenSizeX = (float)m_HorizontalBlurTexture->GetTextureWidth();

	// Set the render target to be the render to texture.
	m_HorizontalBlurTexture->SetRenderTarget(m_D3D->GetDeviceContext());

	// Clear the render to texture.
	m_HorizontalBlurTexture->ClearRenderTarget(m_D3D->GetDeviceContext(), 0.0f, 0.0f, 1.0f, 1.0f);

	// Generate the view matrix based on the camera's position.
	m_Camera->Render();

	// Get the world and view matrices from the camera and d3d objects.
	//m_Camera->GetViewMatrix(viewMatrix);
	//m_D3D->GetWorldMatrix(worldMatrix);

	// Get the ortho matrix from the render to texture since texture has different dimensions.
	//m_HorizontalBlurTexture->GetOrthoMatrix(orthoMatrix);

	// Turn off the Z buffer to begin all 2D rendering.
	m_D3D->TurnZBufferOff();

	// Put the small ortho window vertex and index buffers on the graphics pipeline to prepare them for drawing.
	m_SmallWindow->Render(m_D3D->GetDeviceContext());

	// Render the small ortho window using the horizontal blur shader and the down sampled render to texture resource.
	result = m_ShaderManager->GetHorizontalBlurShader()->Render(m_D3D->GetDeviceContext(), m_SmallWindow->GetIndexCount(), worldMatrix, viewMatrix, orthoMatrix,
		m_DownSampleTexure->GetShaderResourceView(), screenSizeX);
	if (!result)
	{
		return false;
	}

	// Turn the Z buffer back on now that all 2D rendering has completed.
	m_D3D->TurnZBufferOn();

	// Reset the render target back to the original back buffer and not the render to texture anymore.
	m_D3D->SetBackBufferRenderTarget();

	// Reset the viewport back to the original.
	m_D3D->ResetViewport();

	return true;
}


bool GraphicsClass::RenderVerticalBlurToTexture()
{
	D3DXMATRIX worldMatrix, viewMatrix, orthoMatrix;
	float screenSizeY;
	bool result;


	worldMatrix = CameraClass::GetIdentitymatrix();
	viewMatrix = CameraClass::GetIdentitymatrix();
	orthoMatrix = CameraClass::GetIdentitymatrix();

	// Store the screen height in a float that will be used in the vertical blur shader.
	screenSizeY = (float)m_VerticalBlurTexture->GetTextureHeight();

	// Set the render target to be the render to texture.
	m_VerticalBlurTexture->SetRenderTarget(m_D3D->GetDeviceContext());

	// Clear the render to texture.
	m_VerticalBlurTexture->ClearRenderTarget(m_D3D->GetDeviceContext(), 1.0f, 0.0f, 0.0f, 1.0f);

	// Generate the view matrix based on the camera's position.
	m_Camera->Render();

	// Get the world and view matrices from the camera and d3d objects.
	//m_Camera->GetViewMatrix(viewMatrix);
	//m_D3D->GetWorldMatrix(worldMatrix);

	// Get the ortho matrix from the render to texture since texture has different dimensions.
	//m_VerticalBlurTexture->GetOrthoMatrix(orthoMatrix);

	// Turn off the Z buffer to begin all 2D rendering.
	m_D3D->TurnZBufferOff();

	// Put the small ortho window vertex and index buffers on the graphics pipeline to prepare them for drawing.
	m_SmallWindow->Render(m_D3D->GetDeviceContext());

	// Render the small ortho window using the vertical blur shader and the horizontal blurred render to texture resource.
	result = m_ShaderManager->GetVerticalBlurShader()->Render(m_D3D->GetDeviceContext(), m_SmallWindow->GetIndexCount(), worldMatrix, viewMatrix, orthoMatrix,
		m_HorizontalBlurTexture->GetShaderResourceView(), screenSizeY);
	if (!result)
	{
		return false;
	}

	// Turn the Z buffer back on now that all 2D rendering has completed.
	m_D3D->TurnZBufferOn();

	// Reset the render target back to the original back buffer and not the render to texture anymore.
	m_D3D->SetBackBufferRenderTarget();

	// Reset the viewport back to the original.
	m_D3D->ResetViewport();

	return true;
}


bool GraphicsClass::UpSampleTexture()
{
	D3DXMATRIX worldMatrix, viewMatrix, orthoMatrix;
	bool result;

	worldMatrix = CameraClass::GetIdentitymatrix();
	viewMatrix = CameraClass::GetIdentitymatrix();
	orthoMatrix = CameraClass::GetIdentitymatrix();


	// Set the render target to be the render to texture.
	m_UpSampleTexure->SetRenderTarget(m_D3D->GetDeviceContext());

	// Clear the render to texture.
	m_UpSampleTexure->ClearRenderTarget(m_D3D->GetDeviceContext(), 0.0f, 1.0f, 0.0f, 1.0f);

	// Generate the view matrix based on the camera's position.
	m_Camera->Render();

	// Get the world and view matrices from the camera and d3d objects.
	//m_Camera->GetViewMatrix(viewMatrix);
	//m_D3D->GetWorldMatrix(worldMatrix);

	// Get the ortho matrix from the render to texture since texture has different dimensions.
	//m_UpSampleTexure->GetOrthoMatrix(orthoMatrix);

	// Turn off the Z buffer to begin all 2D rendering.
	m_D3D->TurnZBufferOff();

	// Put the full screen ortho window vertex and index buffers on the graphics pipeline to prepare them for drawing.
	m_FullScreenWindow->Render(m_D3D->GetDeviceContext());

	// Render the full screen ortho window using the texture shader and the small sized final blurred render to texture resource.
	result = m_ShaderManager->RenderNoNormalTextureShader(m_D3D->GetDeviceContext(), m_FullScreenWindow->GetIndexCount(), worldMatrix, viewMatrix, orthoMatrix,
		m_VerticalBlurTexture->GetShaderResourceView());
	if (!result)
	{
		return false;
	}

	// Turn the Z buffer back on now that all 2D rendering has completed.
	m_D3D->TurnZBufferOn();

	// Reset the render target back to the original back buffer and not the render to texture anymore.
	m_D3D->SetBackBufferRenderTarget();

	// Reset the viewport back to the original.
	m_D3D->ResetViewport();

	return true;
}


bool GraphicsClass::Render2DTextureScene()
{
	D3DXMATRIX worldMatrix, viewMatrix, orthoMatrix;
	bool result;

	worldMatrix = CameraClass::GetIdentitymatrix();
	viewMatrix = CameraClass::GetIdentitymatrix();
	orthoMatrix = CameraClass::GetIdentitymatrix();

	// Clear the buffers to begin the scene.
	m_D3D->BeginScene(1.0f, 1.0f, 0.0f, 0.0f);

	// Generate the view matrix based on the camera's position.
	//m_Camera->Render();

	// Get the world, view, and ortho matrices from the camera and d3d objects.
	m_Camera->GetViewMatrix(viewMatrix);
	//m_D3D->GetWorldMatrix(worldMatrix);
	//m_D3D->GetOrthoMatrix(orthoMatrix);

	// Turn off the Z buffer to begin all 2D rendering.
	m_D3D->TurnZBufferOff();

	// Put the full screen ortho window vertex and index buffers on the graphics pipeline to prepare them for drawing.
	m_FullScreenWindow->Render(m_D3D->GetDeviceContext());


	//auto sphere = m_models->GetOrSetModel(L"modell/skydome.txt.bin", L"textur/vp_sky_v2_002.png", L"textur/empty_map.dat");


	// Render the full screen ortho window using the texture shader and the full screen sized blurred render to texture resource.
	result = m_ShaderManager->RenderNoNormalTextureShader(m_D3D->GetDeviceContext(), m_FullScreenWindow->GetIndexCount(), worldMatrix, viewMatrix, orthoMatrix,
		//sphere->GetTexture());
		m_UpSampleTexure->GetShaderResourceView());
	if (!result)
	{
		return false;
	}

	// Turn the Z buffer back on now that all 2D rendering has completed.
	m_D3D->TurnZBufferOn();

	// Present the rendered scene to the screen.
	m_D3D->EndScene();

	return true;
}