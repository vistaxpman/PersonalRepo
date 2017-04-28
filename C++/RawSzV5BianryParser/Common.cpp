#include "stdafx.h"

using System::IntPtr;
using System::Runtime::InteropServices::Marshal;

string GetStdStringFromClrString(String ^clr)
{
    IntPtr ptr = Marshal::StringToHGlobalAnsi(clr);
    string std(static_cast<char*>(ptr.ToPointer()));
    Marshal::FreeHGlobal(ptr);

    return std;
}