// WindowsMemoryManagement.cpp : Defines the entry point for the console application.
//

#include <windows.h>
#include <tchar.h>
#include <iostream>

__int64 FileTimeToQuadWord(PFILETIME pft)
{
	return (Int64ShllMod32(pft->dwHighDateTime, 32) | pft->dwLowDateTime);
}

int _tmain(int argc, _TCHAR* argv[])
{
	HANDLE hFile;
	TCHAR szBuffer[] = TEXT("Windows Memory Management ");
	DWORD dwNumBytes;
	FILETIME ftDummy, ftKernelTimeStart, ftUserTimeStart, ftKernelTimeEnd, ftUserTimeEnd;
	__int64 qwTotalTime = 0;
	const int iCycleNumber = 500000;
	HANDLE hThread = GetCurrentThread();

	GetThreadTimes(hThread, &ftDummy, &ftDummy, &ftKernelTimeStart, &ftUserTimeStart);
	{
		hFile = CreateFile(TEXT("TestWriteFile.txt"), GENERIC_WRITE, FILE_SHARE_WRITE, NULL,
			CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
		for (int i = 0; i < iCycleNumber; i++)
			WriteFile(hFile, szBuffer, sizeof(szBuffer), &dwNumBytes, NULL);
		CloseHandle(hFile);
	}
	GetThreadTimes(hThread, &ftDummy, &ftDummy, &ftKernelTimeEnd, &ftUserTimeEnd);
	qwTotalTime = (FileTimeToQuadWord(&ftUserTimeEnd) - FileTimeToQuadWord(&ftUserTimeStart)) +
		(FileTimeToQuadWord(&ftKernelTimeEnd) - FileTimeToQuadWord(&ftKernelTimeStart));
	std::cout << "WriteFile: " << qwTotalTime / (float)10000000 << std::endl;

	GetThreadTimes(hThread, &ftDummy, &ftDummy, &ftKernelTimeStart, &ftUserTimeStart);
	{
		hFile = CreateFile(TEXT("TestMemoryMappedFile.txt"), GENERIC_READ | GENERIC_WRITE,
			FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
		HANDLE hFileMapping = CreateFileMapping(hFile, NULL, PAGE_READWRITE, 0, 
			sizeof(szBuffer) * iCycleNumber, NULL);
		PBYTE pbFile = (PBYTE)MapViewOfFile(hFileMapping, FILE_MAP_WRITE, 0, 0, 0);
		PTCH szFile = (PTCH)pbFile;
		szFile[0] = 0;
		for (int i = 0; i < iCycleNumber; i++)
		{
			_tcscpy_s(szFile, _countof(szBuffer), szBuffer);
			szFile += _countof(szBuffer) - 1;
		}
		FlushViewOfFile(pbFile, 0);
		UnmapViewOfFile(pbFile);
		CloseHandle(hFileMapping);
		CloseHandle(hFile);
	}
	GetThreadTimes(hThread, &ftDummy, &ftDummy, &ftKernelTimeEnd, &ftUserTimeEnd);
	qwTotalTime = (FileTimeToQuadWord(&ftUserTimeEnd) - FileTimeToQuadWord(&ftUserTimeStart)) +
		(FileTimeToQuadWord(&ftKernelTimeEnd) - FileTimeToQuadWord(&ftKernelTimeStart));
	std::cout << "MemoryMappedFile: " << qwTotalTime / (float)10000000 << std::endl;

	char dummy;
	std::cin >> dummy;
	return 0;
}

