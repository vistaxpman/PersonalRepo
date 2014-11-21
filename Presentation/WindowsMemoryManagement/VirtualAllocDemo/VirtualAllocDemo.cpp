// VirtualAllocDemo.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include <windows.h>
#include <tchar.h>

int _tmain(int argc, _TCHAR* argv[])
{
	char dummy;
	std::cout << "Start a process." << std::endl;
	std::cin >> dummy;

	LPVOID lpAdress = VirtualAlloc(NULL, 100 * 1024, MEM_RESERVE, PAGE_READWRITE);
	std::cout << "Memory is reserved." << std::endl;
	std::cin >> dummy;
	
	VirtualAlloc(lpAdress, 100 * 1024, MEM_COMMIT, PAGE_READWRITE);
	std::cout << "Memory is committed." << std::endl;
	std::cin >> dummy;

	*((PBYTE)lpAdress) = 1;
	std::cout << "Write a data to the page." << std::endl;
	std::cin >> dummy;

	*((PBYTE)lpAdress + (4 * 1024)) = 1;
	std::cout << "Write a data to another page." << std::endl;
	std::cin >> dummy;
	return 0;
}

