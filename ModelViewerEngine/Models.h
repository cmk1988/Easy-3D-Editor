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

	ModelClass* GetOrSetModel(std::wstring, std::wstring, std::wstring);

	static Models* GetInstance(ID3D11Device* device);

private:
	ID3D11Device* m_device;
	std::unordered_map<std::wstring, ModelClass*> m_models;
	vector<ModelInfos> m_modelInfos;
	vector<ModelCombination> m_modelCombinations;
	static Models* instance;

};