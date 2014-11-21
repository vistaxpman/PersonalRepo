#include <Windows.h>
extern "C"
{
__declspec( dllexport ) void func()
{
	RaiseException(0xE0030001, EXCEPTION_NONCONTINUABLE, 0, NULL);
}
};
