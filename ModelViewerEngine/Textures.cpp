#include "Textures.h"

Textures* Textures::instance = nullptr;

Textures::Textures(ID3D11Device* device)
{
	m_device = device;
}

Textures::~Textures()
{
}

TextureClass* Textures::GetOrSetTexture(std::wstring filename)
{
	TextureClass* texture = m_textures[filename];
	if (texture)
		return texture;
	texture = new TextureClass();
	texture->Initialize(m_device, filename.c_str());
	m_textures[filename] = texture;
	return texture;
}

Textures* Textures::GetInstance(ID3D11Device* device)
{
	if(Textures::instance == nullptr)
		Textures::instance = new Textures(device);
	return Textures::instance;
}
