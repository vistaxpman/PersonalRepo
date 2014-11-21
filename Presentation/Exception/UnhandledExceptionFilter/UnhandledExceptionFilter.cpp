#include <tchar.h>
#include <Windows.h>
#include <stdio.h>
#include <Dbghelp.h>

LONG WINAPI TopLevelUnhandledExceptionFilter(PEXCEPTION_POINTERS pExceptionInfo);
void CreateMiniDump(PEXCEPTION_POINTERS pep);

int _tmain(int argc, _TCHAR* argv[])
{
   SetUnhandledExceptionFilter(TopLevelUnhandledExceptionFilter);
   DebugBreak();
   return 0;
}

LONG WINAPI TopLevelUnhandledExceptionFilter(PEXCEPTION_POINTERS pExceptionInfo)
{
   MessageBox(NULL, TEXT("I am in TopLevelUnhandledExceptionFilter."), NULL, MB_OK);
   CreateMiniDump(pExceptionInfo);
   // return EXCEPTION_EXECUTE_HANDLER;
   return EXCEPTION_CONTINUE_SEARCH;
}

void CreateMiniDump(PEXCEPTION_POINTERS pep)
{
  // Open the file 
  HANDLE hFile = CreateFile( _T("MiniDump.dmp"), GENERIC_READ | GENERIC_WRITE, 
    0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL ); 

  if( ( hFile != NULL ) && ( hFile != INVALID_HANDLE_VALUE ) ) 
  {
    // Create the minidump 
    MINIDUMP_EXCEPTION_INFORMATION mdei; 
    mdei.ThreadId           = GetCurrentThreadId(); 
    mdei.ExceptionPointers  = pep; 
    mdei.ClientPointers     = FALSE; 

    MINIDUMP_TYPE mdt       = MiniDumpNormal; 

    BOOL rv = MiniDumpWriteDump( GetCurrentProcess(), GetCurrentProcessId(), 
      hFile, mdt, (pep != 0) ? &mdei : 0, 0, 0 ); 

    if( !rv ) 
      _tprintf( _T("MiniDumpWriteDump failed. Error: %u \n"), GetLastError() ); 
    else 
      _tprintf( _T("Minidump created.\n") ); 

    // Close the file 

    CloseHandle( hFile ); 

  }
  else 
  {
    _tprintf( _T("CreateFile failed. Error: %u \n"), GetLastError() ); 
  }
}
