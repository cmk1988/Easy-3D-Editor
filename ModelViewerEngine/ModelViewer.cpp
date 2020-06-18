#include "ModelViewer.h"



ModelViewer::ModelViewer()
{
	m_System = new SystemClass();
	m_System->Initialize();
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
		//L"D:\\Develop\\Easy-3D-Editor\\Easy 3D Editor\\bin\Debug\\t1.obj", 
		//L"C:\\Users\\User\\Pictures\\sixcuts.png"
		getWCharFromString(modelFilePath),
		getWCharFromString(textureFilePath)
		);
	m_System->ContinueRendering();
}

void ModelViewer::Rotate(float x, float y, float z)
{
}

const wchar_t* ModelViewer::getWCharFromString(System::String^ str)
{
	pin_ptr<const wchar_t> convertedValue = PtrToStringChars(str);
	return convertedValue;
}
