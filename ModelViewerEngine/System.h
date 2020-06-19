#pragma once
#define WIN32_LEAN_AND_MEAN


#include <windows.h>
#include <chrono>
#include <thread>
#include "Graphics.h"

using namespace std::chrono;

class SystemClass
{
public:
	SystemClass();
	SystemClass(const SystemClass&);
	~SystemClass();

	bool Initialize();
	bool Initialize(HWND);
	void Shutdown();
	void Run();
	void Stop();
	void PauseRendering();
	void ContinueRendering();
	void RotateCam(float x, float y, float z);
	void MoveCam(float x);

	LRESULT CALLBACK MessageHandler(HWND, UINT, WPARAM, LPARAM);

private:
	bool Frame();
	void InitializeWindows(int& screenWidth, int& screenHeight);
	void ShutdownWindows();
	void run();

private:
	LPCWSTR m_applicationName;
	HINSTANCE m_hinstance;
	HWND m_hwnd;
	milliseconds lastRenderTime = milliseconds::zero();

	GraphicsClass* m_Graphics;
	thread runner;
	bool isRunning = false;
	bool cancel = false;
	bool pause = false;
	bool isPaused = false;
};


static LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

static SystemClass* ApplicationHandle = 0;