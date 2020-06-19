#include "ModelViewer.h"



ModelViewer::ModelViewer()
{
	m_System = new SystemClass();
	m_System->Initialize();
	m_System->Run();
}

ModelViewer::ModelViewer(int hwnd)
{
	m_System = new SystemClass();
	m_System->Initialize((HWND)hwnd);
	m_System->Run();
}

ModelViewer::~ModelViewer()
{
	m_System->Stop();
	m_System->Shutdown();
	delete m_System;
}

void ModelViewer::Load(System::String^ modelFilePath, System::String^ textureFilePath)
{
	m_System->PauseRendering();
	Models::GetInstance()->SetModel(
		getWCharFromString(modelFilePath),
		getWCharFromString(textureFilePath)
		);
	m_System->ContinueRendering();
}

void ModelViewer::Rotate(float x, float y, float z)
{
	m_System->RotateCam(x, y, z);
}

const wchar_t* ModelViewer::getWCharFromString(System::String^ str)
{
	pin_ptr<const wchar_t> convertedValue = PtrToStringChars(str);
	return convertedValue;
}
