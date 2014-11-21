extern "C"
{
    __declspec(dllexport) char* GetText();
    __declspec(dllexport) void FreeText(char* pText);
}