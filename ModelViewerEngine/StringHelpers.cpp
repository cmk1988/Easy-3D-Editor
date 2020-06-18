#include "StringHelpers.h"

std::vector<std::wstring> wsplit(std::wstring s, std::wstring delimiter)
{
	std::vector<std::wstring> v;

	size_t pos = 0;
	std::wstring token;
	while ((pos = s.find(delimiter)) != std::wstring::npos) {
		token = s.substr(0, pos);
		v.push_back(token);
		s.erase(0, pos + delimiter.length());
	}
	v.push_back(s);
	return v;
}

std::vector<std::string> split(std::string s, std::string delimiter)
{
	std::vector<std::string> v;

	size_t pos = 0;
	std::string token;
	while ((pos = s.find(delimiter)) != std::string::npos) {
		token = s.substr(0, pos);
		v.push_back(token);
		s.erase(0, pos + delimiter.length());
	}
	v.push_back(s);
	return v;
}