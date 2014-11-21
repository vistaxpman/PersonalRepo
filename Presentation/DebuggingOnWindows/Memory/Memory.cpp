// Memory.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdlib.h>
#include <windows.h>
int _tmain(int argc, _TCHAR* argv[])
{   
	char *buffer = (char*)malloc(12);
   buffer[12] = 'a';
   free(buffer);
   return 0;
}