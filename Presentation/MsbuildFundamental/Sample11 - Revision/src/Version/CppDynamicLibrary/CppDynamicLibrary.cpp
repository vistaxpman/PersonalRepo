#include "CppDynamicLibrary.h"

using namespace System;
using namespace System::Runtime::InteropServices;

__declspec(dllexport) char* GetText()
{
    char* pText = (char*)Marshal::StringToHGlobalAnsi(CSharpAssembly::CSharpClass::GetString()).ToPointer();
    return pText;
}

__declspec(dllexport) void FreeText(char* pText)
{
    Marshal::FreeHGlobal(IntPtr(pText));
}