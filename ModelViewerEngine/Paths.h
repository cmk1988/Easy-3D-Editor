#pragma once
#include <string>
#include <fstream>
#include <streambuf>
#include <vector>
#include <filesystem>

using namespace std;

class FileSystemClass
{
public:
	static vector<wstring> findFiles(const wchar_t* path, const wchar_t* filename);
	static bool replace(wstring& str, const wstring& from, const wstring& to);
	static bool endsWith(wstring const& fullString, wstring const& ending);
	static bool startsWith(wstring const& fullString, wstring const& start);
	static int removeIfAlternativeExists(const wchar_t* path, const wchar_t* toRemove, const wchar_t* alternative);
	static void replaceFile(const wchar_t* oldFile, const wchar_t* newFile);
	static vector<wstring> getFilesWithDifferentEndings(const wchar_t* path, const wchar_t* filename, const wchar_t* fileEnding, wstring& additionalFileEnding);
	static wstring getFilenameWithoutExtension(const wchar_t* filename, const wchar_t* fileEnding, wstring additionalFileEnding);

};

