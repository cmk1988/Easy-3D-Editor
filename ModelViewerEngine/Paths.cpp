#include "Paths.h"


vector<wstring> FileSystemClass::findFiles(const wchar_t* path, const wchar_t* filename)
{
	vector<wstring> result;
	wstring fn(filename ? filename : L"");
	wstring start;
	wstring end;
	auto wildTokenPosi = fn.find('*');
	bool wildTokenUsed = false;
	if (wildTokenPosi != string::npos)
	{
		start = fn.substr(0, wildTokenPosi);
		end = fn.substr(wildTokenPosi, fn.size() - wildTokenPosi);
		if (end != L"")
			replace(end, L"*", L"");
		wildTokenUsed = true;
	}
	for (auto& p : filesystem::recursive_directory_iterator(path))
	{
		if (filename == nullptr)
		{
			result.push_back(p.path().wstring());
			continue;
		}
		auto filePath = p.path().wstring();
		auto index = filePath.find_last_of(L"\\");
		auto _filename = filePath.substr(index, filePath.size() - index);
		replace(_filename, L"\\", L"");
		if (wildTokenUsed)
		{
			if ((start == L"" || startsWith(_filename, start.c_str())) && (end == L"" || endsWith(_filename, end.c_str())))
			{
				result.push_back(p.path().wstring());
			}
		}
		else if (_filename == fn)
		{
			result.push_back(p.path().wstring());
		}
	}
	return result;
}

bool FileSystemClass::replace(wstring& str, const wstring& from, const wstring& to)
{
	size_t start_pos = str.find(from);
	if (start_pos == wstring::npos)
	{
		return false;
	}
	str.replace(start_pos, from.length(), to);
	return true;
}

bool FileSystemClass::endsWith(wstring const& fullString, wstring const& ending)
{
	if (fullString.length() >= ending.length())
	{
		return (fullString.compare(fullString.length() - ending.length(), ending.length(), ending) == 0);
	}
	return false;
}

bool FileSystemClass::startsWith(wstring const& fullString, wstring const& start)
{
	if (fullString.length() >= start.length())
	{
		return fullString.rfind(start, 0) == 0;
	}
	return false;
}

int FileSystemClass::removeIfAlternativeExists(const wchar_t* path, const wchar_t* toRemove, const wchar_t* alternative)
{
	auto filesToRemove = findFiles(path, toRemove);
	auto alternativeFiles = findFiles(path, alternative);
	if (alternativeFiles.size() > 0)
		for (auto& filename : filesToRemove)
		{
			filesystem::remove(filename);
		}
	return 0;
}

void FileSystemClass::replaceFile(const wchar_t* oldFile, const wchar_t* newFile)
{
	filesystem::remove(oldFile);
	filesystem::rename(newFile, oldFile);
}

vector<wstring> FileSystemClass::getFilesWithDifferentEndings(const wchar_t* path, const wchar_t* filename, const wchar_t* fileEnding, wstring& additionalFileEnding)
{
	auto files = findFiles(path, (wstring(filename) + additionalFileEnding + fileEnding).c_str());
	if (files.size() == 0)
	{
		additionalFileEnding = L"";
		files = findFiles(path, (wstring(filename) + fileEnding).c_str());
	}
	return files;
}

wstring FileSystemClass::getFilenameWithoutExtension(const wchar_t* filename, const wchar_t* fileEnding, wstring additionalFileEnding)
{
	wstring filenameWithoutExtension(filename);
	replace(filenameWithoutExtension, fileEnding, L"");
	if (additionalFileEnding != L"")
		replace(filenameWithoutExtension, additionalFileEnding, L"");
	return filenameWithoutExtension;
}