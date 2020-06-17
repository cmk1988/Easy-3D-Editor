#pragma once
#ifndef _GRAPHICSCLASS_H_
#define _GRAPHICSCLASS_H_

#include "d3dclass.h"
#include "cameraclass.h"

#include "shadermanagerclass.h"
#include "rendertextureclass.h"

#include "Models.h"

const bool FULL_SCREEN = false;
const bool VSYNC_ENABLED = true;
const float SCREEN_DEPTH = 1000.0f;
const float SCREEN_NEAR = 0.1f;

class GraphicsClass
{
public:
	GraphicsClass();
	GraphicsClass(const GraphicsClass&);
	~GraphicsClass();

	CameraClass* m_Camera;

	bool Initialize(int, int, HWND);
	void Shutdown();
	bool Frame(HWND);



private:
	bool Render(HWND);
	bool RenderScene(HWND hwnd);

private:
	D3DClass* m_D3D;
	ModelClass* m_Model;
	ShaderManagerClass* m_ShaderManager;
	Models* m_models;

	RenderTextureClass
		* m_RenderTexture,
		* m_DownSampleTexure,
		* m_HorizontalBlurTexture,
		* m_VerticalBlurTexture,
		* m_UpSampleTexure;

};

#endif