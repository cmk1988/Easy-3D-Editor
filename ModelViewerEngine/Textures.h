#pragma once
#include "textureclass.h"
#include <unordered_map>
#include <vector>

using namespace std;

class Textures
{
public:
	Textures(ID3D11Device* device);
	~Textures();

	TextureClass* GetOrSetTexture(std::wstring);

	static Textures* GetInstance(ID3D11Device* m_device);

private:
	ID3D11Device* m_device;
	std::unordered_map<std::wstring, TextureClass*> m_textures;
	static Textures* instance;
};