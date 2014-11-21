#include <tchar.h>
#include <windows.h>
#include <iostream>

int _tmain(int argc, _TCHAR* argv[])
{
   char dummy;
   HANDLE hHeap;
   void *p;
   BOOL bRet;
   hHeap = GetProcessHeap();
   p = HeapAlloc(hHeap, 0, 20);
   bRet = HeapFree(hHeap, 0, p);
   std::cout << "Free p first time " << bRet << std::endl;
   bRet = HeapFree(hHeap, 0, p);
   std::cout << "Free p second time " << bRet << std::endl;
   bRet = HeapValidate(hHeap, 0, NULL);
   std::cout << "HeapValidate " << bRet << std::endl;
   std::cin >> dummy;
   return 0;
}