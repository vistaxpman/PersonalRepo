#include <tchar.h>
#include <Windows.h>
#include <stdio.h>

class C
{
public:
   C() {printf("CLASS C Constructor\n");};
   ~C() {printf("CLASS C Destructor\n");}
};

DWORD Funcenstein1();
DWORD Funcenstein2();
DWORD Funcenstein3();
DWORD Funcenstein4();
DWORD Funcenstein5();
DWORD Funcenstein6();
void Funcenstein7();
void Func();

DWORD Funcfurter1();
DWORD Funcinator(DWORD dwProtectedData);

HANDLE g_hSem;
DWORD  g_dwProtectedData;

int _tmain(int argc, _TCHAR* argv[])
{
   g_hSem = CreateSemaphore(NULL, 1, 5, NULL);
   DWORD dwRet;
   __try {
      // dwRet = Funcenstein1();
      // dwRet = Funcenstein2();
      // dwRet = Funcenstein3();
      // dwRet = Funcenstein4();
      // dwRet = Funcenstein5();
      // dwRet = Funcfurter1();
   }
   __except (EXCEPTION_EXECUTE_HANDLER) {
      // Nothing to do here
   }
   __try {
      dwRet = Funcenstein6();
   }
   __except (EXCEPTION_EXECUTE_HANDLER) {
      // Nothing to do here
   }
   Funcenstein7();
return 0;
}

DWORD Funcenstein1()
{
   DWORD dwTemp;

   // 1. Do any processing here.
   __try {
      // 2. Request permission to access
      //    protected data, and then use it.
      WaitForSingleObject(g_hSem, INFINITE);

      g_dwProtectedData = 5;
      dwTemp = g_dwProtectedData;
   }
   __finally {
      // 3. Allow others to use protected data.
      BOOL bIsAbnormal = AbnormalTermination();
      ReleaseSemaphore(g_hSem, 1, NULL);
   }

   // 4. Continue processing.
   return(dwTemp);
}

DWORD Funcenstein2()
{
   DWORD dwTemp;

   // 1. Do any processing here.
   __try {
      // 2. Request permission to access
      //    protected data, and then use it.
      WaitForSingleObject(g_hSem, INFINITE);

      g_dwProtectedData = 5;
      dwTemp = g_dwProtectedData;

      // Return the new value.
      return(dwTemp);
   }
   __finally {
      // 3. Allow others to use protected data.
      BOOL bIsAbnormal = AbnormalTermination();
      ReleaseSemaphore(g_hSem, 1, NULL);
   }

   // Continue processing--this code
   // will never execute in this version.
   dwTemp = 9;
   return(dwTemp);
}

DWORD Funcenstein3()
{
   DWORD dwTemp;

   // 1. Do any processing here.
   __try {
      // 2. Request permission to access
      //    protected data, and then use it.
      WaitForSingleObject(g_hSem, INFINITE);

      g_dwProtectedData = 5;
      dwTemp = g_dwProtectedData;

      // Try to jump over the finally block.
      goto ReturnValue;
   }
   __finally {
      // 3. Allow others to use protected data.
      ReleaseSemaphore(g_hSem, 1, NULL);
   }

   dwTemp = 9;
   // 4. Continue processing.
   ReturnValue:
   return(dwTemp);
}

DWORD Funcfurter1()
{
   DWORD dwTemp;

   // 1. Do any processing here.
   __try {
      // 2. Request permission to access
      //    protected data, and then use it.
      WaitForSingleObject(g_hSem, INFINITE);
      g_dwProtectedData = 0;
      dwTemp = Funcinator(g_dwProtectedData);
   }
   __finally {
      // 3. Allow others to use protected data.
      MessageBox(NULL, TEXT("I am releasing Semaphore now."), NULL, MB_OK);
      BOOL bIsAbnormal = AbnormalTermination();
      ReleaseSemaphore(g_hSem, 1, NULL);
   }

   // 4. Continue processing.
   return(dwTemp);
}

DWORD Funcinator(DWORD dwProtectedData)
{
   return 5 / dwProtectedData;
}

DWORD Funcenstein4()
{
   DWORD dwTemp;
   // 1. Do any processing here.
   __try {
      // 2. Request permission to access
      //    protected data, and then use it.
      WaitForSingleObject(g_hSem, INFINITE);

      g_dwProtectedData = 5;
      dwTemp = g_dwProtectedData;

      // Return the new value.
      return(dwTemp);
   }
   __finally {
      // 3. Allow others to use protected data.
      ReleaseSemaphore(g_hSem, 1, NULL);
      return(103);
   }

   // Continue processing--this code will never execute.
   dwTemp = 9;
   return(dwTemp);
}

DWORD Funcenstein5()
{
   DWORD dwTemp;

   // 1. Do any processing here.
   __try {
      // 2. Request permission to access
      //    protected data, and then use it.
      WaitForSingleObject(g_hSem, INFINITE);
      __leave;
      g_dwProtectedData = 5;
      dwTemp = g_dwProtectedData;
   }
   __finally {
      // 3. Allow others to use protected data.
      BOOL bIsAbnormal = AbnormalTermination();
      ReleaseSemaphore(g_hSem, 1, NULL);
   }

   // 4. Continue processing.
   return(dwTemp);
}

DWORD Funcenstein6()
{
   C c;
   // int y = 5 / (&c - &c);
   throw 1;
   return 0;
}

void Funcenstein7()
{
   try {
      Func();
   }
   catch(...) {
   }
}

void Func()
{
   C c;
   throw 1;
}

BOOL Funcarama1()
{
   HANDLE hFile = INVALID_HANDLE_VALUE;
   PVOID pvBuf = NULL;
   DWORD dwNumBytesRead;
   BOOL bOk;

   hFile = CreateFile(TEXT("SOMEDATA.DAT"), GENERIC_READ,
      FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
   if (hFile == INVALID_HANDLE_VALUE) {
      return(FALSE);
   }

   pvBuf = VirtualAlloc(NULL, 1024, MEM_COMMIT, PAGE_READWRITE);
   if (pvBuf == NULL) {
      CloseHandle(hFile);
      return(FALSE);
   }

   bOk = ReadFile(hFile, pvBuf, 1024, &dwNumBytesRead, NULL);
   if (!bOk || (dwNumBytesRead == 0)) {
      VirtualFree(pvBuf, 1024, MEM_RELEASE | MEM_DECOMMIT);
      CloseHandle(hFile);
      return(FALSE);
   }

   // Do some calculation on the data.
   // Clean up all the resources.
   VirtualFree(pvBuf, 1024, MEM_RELEASE | MEM_DECOMMIT);;
   CloseHandle(hFile);
   return(TRUE);
}

BOOL Funcarama2()
{
   HANDLE hFile = INVALID_HANDLE_VALUE;
   PVOID pvBuf = NULL;
   DWORD dwNumBytesRead;
   BOOL bOk, bSuccess = FALSE;

   hFile = CreateFile(TEXT("SOMEDATA.DAT"), GENERIC_READ,
      FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);

   if (hFile != INVALID_HANDLE_VALUE) {
      pvBuf = VirtualAlloc(NULL, 1024, MEM_COMMIT, PAGE_READWRITE);
      if (pvBuf != NULL) {
         bOk = ReadFile(hFile, pvBuf, 1024, &dwNumBytesRead, NULL);
         if (bOk && (dwNumBytesRead != 0)) {
            // Do some calculation on the data.
            bSuccess = TRUE;
         }
         VirtualFree(pvBuf, 1024, MEM_RELEASE | MEM_DECOMMIT);
      }
      CloseHandle(hFile);
   }
   return(bSuccess);
}

DWORD Funcarama3()
{
   // IMPORTANT: Initialize all variables to assume failure.
   HANDLE hFile = INVALID_HANDLE_VALUE;
   PVOID pvBuf = NULL;

   __try {
      DWORD dwNumBytesRead;
      BOOL bOk;

      hFile = CreateFile(TEXT("SOMEDATA.DAT"), GENERIC_READ,
         FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
      if (hFile == INVALID_HANDLE_VALUE) {
         return(FALSE);
      }

      pvBuf = VirtualAlloc(NULL, 1024, MEM_COMMIT, PAGE_READWRITE);
      if (pvBuf == NULL) {
         return(FALSE);
      }

      bOk = ReadFile(hFile, pvBuf, 1024, &dwNumBytesRead, NULL);
      if (!bOk || (dwNumBytesRead != 1024)) {
         return(FALSE);
      }

      // Do some calculation on the data.
   }

   __finally {
      // Clean up all the resources.
      if (pvBuf != NULL)
         VirtualFree(pvBuf, 1024, MEM_RELEASE | MEM_DECOMMIT);
      if (hFile != INVALID_HANDLE_VALUE)
         CloseHandle(hFile);
   }
   // Continue processing.
   return(TRUE);
}

DWORD Funcarama4()
{
   // IMPORTANT: Initialize all variables to assume failure.
   HANDLE hFile = INVALID_HANDLE_VALUE;
   PVOID pvBuf = NULL;

   // Assume that the function will not execute successfully.
   BOOL bFunctionOk = FALSE;

   __try {
      DWORD dwNumBytesRead;
      BOOL bOk;

      hFile = CreateFile(TEXT("SOMEDATA.DAT"), GENERIC_READ,
         FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
      if (hFile == INVALID_HANDLE_VALUE) {
         __leave;
      }

      pvBuf = VirtualAlloc(NULL, 1024, MEM_COMMIT, PAGE_READWRITE);

      if (pvBuf == NULL) {
         __leave;
      }

      bOk = ReadFile(hFile, pvBuf, 1024, &dwNumBytesRead, NULL);
      if (!bOk || (dwNumBytesRead == 0)) {
         __leave;
      }

      // Do some calculation on the data.
      // Indicate that the entire function executed successfully.
      bFunctionOk = TRUE;
   }
   __finally {
      // Clean up all the resources.
      if (pvBuf != NULL)
         VirtualFree(pvBuf, 1024, MEM_RELEASE | MEM_DECOMMIT);
      if (hFile != INVALID_HANDLE_VALUE)
         CloseHandle(hFile);
   }
   // Continue processing.
   return(bFunctionOk);
}
