#include "ModelViewer.h"



ModelViewer::ModelViewer(HWND hwnd)
{
}

ModelViewer::~ModelViewer()
{
}

void ModelViewer::Load(System::String^ modelFilePath, System::String^ textureFilePath)
{
}

void ModelViewer::Rotate(float x, float y, float z)
{
}

const wchar_t* ModelViewer::getWCharFromString(System::String^ str)
{
	pin_ptr<const wchar_t> convertedValue = PtrToStringChars(str);
	return convertedValue;
}
