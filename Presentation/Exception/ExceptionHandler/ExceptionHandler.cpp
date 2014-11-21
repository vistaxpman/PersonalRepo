#include <tchar.h>
#include <Windows.h>
#include <stdio.h>

DWORD Funcmeister1();
DWORD Funcmeister2();
void FunclinRoosevelt1();
LONG OilFilter1(TCHAR **ppchBuffer);
void FunclinRoosevelt2();
LONG OilFilter2(DWORD dwExceptionCode);
void FuncAtude3(TCHAR *sz);
TCHAR g_szBuffer[100];

int _tmain(int argc, _TCHAR* argv[])
{
   DWORD dwRet;
   // dwRet = Funcmeister1();
   // dwRet = Funcmeister2();
   // FunclinRoosevelt1();
   __try {
      // FuncAtude3(NULL);
   }
   __except (EXCEPTION_EXECUTE_HANDLER) {
      // Handle an exception; this never executes.
      MessageBox(NULL, TEXT("I am an exception handler inside Main!"), NULL, MB_OK);
   }
   FunclinRoosevelt2();
}

DWORD Funcmeister1()
{
   DWORD dwTemp;

   // 1. Do any processing here.
   __try {
      // 2. Perform some operation.
      dwTemp = 0;
   }
   __except (EXCEPTION_EXECUTE_HANDLER) {
      // Handle an exception; this never executes.
      MessageBox(NULL, TEXT("I am an exception handler!"), NULL, MB_OK);
   }

   // 3. Continue processing.
   return(dwTemp);
}

DWORD Funcmeister2()
{
   DWORD dwTemp = 0;

   // 1. Do any processing here.
   __try {
      // 2. Perform some operation(s).
      dwTemp = 5 / dwTemp;     // Generates an exception
      dwTemp += 10;            // Never executes
   }
   __except (/* 3. Evaluate filter. */ EXCEPTION_EXECUTE_HANDLER) {
      // 4. Handle an exception.
      MessageBox(NULL, TEXT("I am an exception handler!"), NULL, MB_OK);
   }

   // 5. Continue processing.
   return(dwTemp);
}

void FunclinRoosevelt1()
{
   int x = 0;
   TCHAR *pchBuffer = NULL;

   __try {
      *pchBuffer = TEXT('J');
      x = 5 / x;
   }
   __except (OilFilter1(&pchBuffer)) {
      MessageBox(NULL, TEXT("An exception occurred"), NULL, MB_OK);
   }
   MessageBox(NULL, TEXT("Function completed"), NULL, MB_OK);
}

LONG OilFilter1(TCHAR **ppchBuffer)
{
   if (*ppchBuffer == NULL) {
      *ppchBuffer = g_szBuffer;
      return(EXCEPTION_CONTINUE_EXECUTION);
   }
   return(EXCEPTION_EXECUTE_HANDLER);
}

void FunclinRoosevelt2()
{
   int x = 0;
   TCHAR *pchBuffer = NULL;

   __try {
      RaiseException(0xE0030001, EXCEPTION_NONCONTINUABLE, 0, NULL);
   }
   __except (OilFilter2(GetExceptionCode())) {
      MessageBox(NULL, TEXT("I can handle my own exception."), NULL, MB_OK);
   }
   MessageBox(NULL, TEXT("Function completed"), NULL, MB_OK);
}

LONG OilFilter2(DWORD dwExceptionCode)
{
   if (dwExceptionCode != 0xE0030001) {
      return(EXCEPTION_CONTINUE_SEARCH);
   }
   return(EXCEPTION_EXECUTE_HANDLER);
}

void FuncAtude3(TCHAR *sz)
{
   __try {
      *sz = TEXT('\0');
   }
   __except (EXCEPTION_CONTINUE_SEARCH) {
      // This never executes.
      MessageBox(NULL, TEXT("I am an exception handler inside FuncAtude3!"), NULL, MB_OK);
   }
}

void FuncExceptionCode()
{
   __try {
      int x=0;
      int y = 4 / x; // y is used later so this statement is not optimized away
   }
   __except ((GetExceptionCode() == EXCEPTION_INT_DIVIDE_BY_ZERO) ?
      EXCEPTION_EXECUTE_HANDLER : EXCEPTION_CONTINUE_SEARCH) {
      // Handle divide by zero exception.
   }
}
