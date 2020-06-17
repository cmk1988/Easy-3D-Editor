#include "Models.h"


Models* Models::instance = nullptr;

Models::Models(ID3D11Device* device)
{
	m_device = device;
}

Models::~Models()
{
}

ModelClass* Models::GetOrSetModel(std::wstring filename, std::wstring texture, std::wstring bumpmap)
{
	ModelClass* model = m_models[filename];
	if (model)
		return model;
	model = new ModelClass();
	model->Initialize(m_device, filename, texture, bumpmap);
	m_models[filename] = model;
	return model;
}

Models* Models::GetInstance(ID3D11Device* device)
{
	if (!instance)
		instance = new Models(device);
	return instance;
}
