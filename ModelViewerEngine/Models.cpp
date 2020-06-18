#include "Models.h"


Models* Models::instance = nullptr;

Models::Models(ID3D11Device* device)
{
	m_device = device;
}

Models::~Models()
{
}

void Models::SetModel(std::wstring filename, std::wstring texture)
{
	//if (m_model)
	//	delete m_model;
	if (!m_model)
		m_model = new ModelClass();
	m_model->Initialize(m_device, filename, texture, L"");
}

ModelClass* Models::getModel()
{
	return m_model;
}

Models* Models::GetInstance(ID3D11Device* device)
{
	if (!instance)
		instance = new Models(device);
	return instance;
}

Models* Models::GetInstance()
{
	return instance;
}
