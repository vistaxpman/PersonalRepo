// MallocAndNew.cpp : Defines the entry point for the console application.
//

#include <stdlib.h>
#include <iostream>
#include <tchar.h>
#include <windows.h>
class ClassA {};

int MyAllocHook(int allocType, void *userData, size_t size, int blockType, long requestNumber, const unsigned char *filename, int lineNumber);


int _tmain(int argc, _TCHAR* argv[])
{
	char dummy;
   // _CrtSetAllocHook(MyAllocHook);
   char* a = (char*)malloc(1000 * sizeof(char));
   // std::cin >> dummy;
	ClassA* classA = new ClassA;

   _CrtDumpMemoryLeaks();
   std::cin >> dummy;
	return 0;
}

int MyAllocHook(int allocType, void *userData, size_t size, int blockType, long requestNumber, const unsigned char *filename, int lineNumber)
{
   if (blockType == _CRT_BLOCK)
      return TRUE;

   switch (allocType)
   {
   case _HOOK_ALLOC:
      std::cout << "CRT ALLOC: " << requestNumber << std::endl;
      break;
   case _HOOK_FREE:
      std::cout << "CRT FREE: " << requestNumber << std::endl;
      break;
   case _HOOK_REALLOC:
      std::cout << "CRT REALLOC: " << requestNumber << std::endl;
      break;
   }
   return TRUE;
}