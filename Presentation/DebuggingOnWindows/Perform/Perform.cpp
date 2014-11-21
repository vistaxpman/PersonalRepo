// Perform.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <cmath>
#include <iostream>
#include <fstream>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	for (int i = 0; i < 100000; i++)
   {
      cout << sinf(i);
   }
   
   ofstream outfile("a.txt");
   for (int i = 0; i < 1000000; i++)
   {
      
      outfile << "slow my disk "<< i << endl;
      outfile.flush();
   }
   return 0;
}

