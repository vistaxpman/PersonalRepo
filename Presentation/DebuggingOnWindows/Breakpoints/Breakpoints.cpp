// Breakpoints.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <windows.h>
#include <process.h>

using namespace std;

DWORD g_danger;
CRITICAL_SECTION cs;

void CallToAnother()
{
   cout << "Tracepoint" << endl;
}

void Danger(void *)
{
   EnterCriticalSection(&cs);
   g_danger = GetTickCount();
   cout << g_danger << endl;
   LeaveCriticalSection(&cs);
}

int _tmain(int argc, _TCHAR* argv[])
{
	int a = 0;
   for (int i = 0; i < 1000; i++)
   {
      if (i == 100)
      {
         cout << "Conditional breakpoit: Is True" << endl;
         a = 2;
      }

      if (i == 200)
         cout << "Conditional breakpoit: Has changed" << endl;

      if (i == 300)
         cout << "Conditional breakpoit: Hit count" << endl;

      if (i == 400)
         CallToAnother();
   }
   
   InitializeCriticalSection(&cs);
   HANDLE hThreadHandles[2];
   hThreadHandles[0] = (HANDLE)_beginthread(Danger, 0, NULL);
   hThreadHandles[1] = (HANDLE)_beginthread(Danger, 0, NULL);
   WaitForMultipleObjects(2, hThreadHandles, TRUE, INFINITE);
   return 0;
}

