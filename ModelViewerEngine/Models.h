#pragma once
#include "modelclass.h"
#include <unordered_map>


enum class Shaders
{
	SHADER_TEXTURE,
	SHADER_TEXTURE_BUMPMAP
};

class ModelCombination
{
public:
	bool isBlocked;
	vector<int> Models;

	ModelCombination()
	{
	}

	~ModelCombination()
	{
	}
};

class ModelInfos
{
public:
	wstring FileName;
	wstring TextureName;
	wstring BumpmapName;
	Shaders Shader;
};

class Models
{
public:
	Models(ID3D11Device* device);
	~Models();

	void SetModel(std::wstring, std::wstring);
	ModelClass* getModel();

	static Models* GetInstance(ID3D11Device* device);
	static Models* GetInstance();

private:
	ID3D11Device* m_device;
	ModelClass* m_model = nullptr;
	static Models* instance;

};