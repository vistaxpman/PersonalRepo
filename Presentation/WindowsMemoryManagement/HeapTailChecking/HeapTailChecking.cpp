#include <tchar.h>
#include <windows.h>
#include <iostream>

int _tmain(int argc, _TCHAR* argv[])
{
   char dummy;
   HANDLE hHeap;
   char *p;
   BOOL bRet;
   hHeap = GetProcessHeap();
   p = (char*)HeapAlloc(hHeap, 0, 9);
   for (int i = 0; i < 50; ++i)
      *(p+i) = i;
   
   bRet = HeapFree(hHeap, 0, p);
   std::cout << "Free p " << bRet << std::endl;
   std::cin >> dummy;
   return 0;
}