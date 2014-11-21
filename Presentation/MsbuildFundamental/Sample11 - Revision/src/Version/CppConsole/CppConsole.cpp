#include <iostream>
#include <tchar.h>
#include "CppDynamicLibrary.h"

int _tmain(int argc, _TCHAR* argv[])
{
    char* pText = GetText();
    std::cout << pText << std::endl;
    FreeText(pText);

    return 0;
}