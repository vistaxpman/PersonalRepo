// Test.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include <malloc.h>
#include <stdlib.h>
#include <crtdbg.h>

int _tmain(int argc, _TCHAR* argv[])
{
   _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
   malloc(10000);
   new char[1000];
   _CrtDumpMemoryLeaks();
   return 0;
}