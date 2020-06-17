#pragma once
#include <windows.h>
#include <vcclr.h>
#include "System.h"

public class ModelViewer
{
public:
	ModelViewer(HWND hwnd);
	~ModelViewer();

	void Load(System::String^ modelFilePath, System::String^ textureFilePath);
	void Rotate(float x, float y, float z);

private:
	static const wchar_t* getWCharFromString(System::String^ str);

	SystemClass m_System;
};