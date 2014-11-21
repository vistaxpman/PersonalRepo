// Compile: CL.EXE .\BinarySearch.cpp

#include <tchar.h>
#include <crtdbg.h>

template <typename T>
T *BinarySearch1(T *pArray, size_t length, T value)
{
	ptrdiff_t lo = 0, hi = length - 1;
	while (lo <= hi)
	{
		size_t mid = (lo + hi) / 2;
		int temp = pArray[mid];
		if (temp == value)
		{
			return pArray + mid;
		}

		if (temp < value)
		{
			lo = mid + 1;
		}
		else
		{
			hi = mid - 1;
		}
	}
	return NULL;
}

template <typename T>
T *BinarySearch(T *pArray, size_t length, T value)
{
	T *pLower = pArray, *pUpper = pArray + length;
	while (pLower < pUpper)
	{
		T* pMiddle = pLower + ((pUpper - pLower) >> 1);
		T temp = *pMiddle;

		if (temp < value)
		{
			pLower = pMiddle + 1;
		}
		else if (temp > value)
		{
			pUpper = pMiddle;
		}
		else
		{
			return pMiddle;
		}
	}
	return NULL;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int array[] = {0, 1, 2, 3, 5, 6, 7, 8, 9};
	size_t length = sizeof(array) / sizeof(int);

	typedef int * (*SearchFunPtr) (int *pArray, size_t length, int value);
	SearchFunPtr funPtr = BinarySearch1;

	int *pResult = funPtr(array, length, 0);
	_ASSERT(pResult == array);

	pResult = funPtr(array, length, 3);
	_ASSERT(pResult == array + 3);

	pResult = funPtr(array, length, 9);
	_ASSERT(pResult == array + length - 1);

	pResult = funPtr(array, length, 4);
	_ASSERT(pResult == NULL);

	pResult = funPtr(array, length, 10);
	_ASSERT(pResult == NULL);

	pResult = funPtr(array, length, -1);
	_ASSERT(pResult == NULL);

	funPtr = BinarySearch;
	pResult = funPtr(array, length, 0);
	_ASSERT(pResult == array);

	pResult = funPtr(array, length, 3);
	_ASSERT(pResult == array + 3);

	pResult = funPtr(array, length, 9);
	_ASSERT(pResult == array + length - 1);

	pResult = funPtr(array, length, 4);
	_ASSERT(pResult == NULL);

	pResult = funPtr(array, length, 10);
	_ASSERT(pResult == NULL);

	pResult = funPtr(array, length, -1);
	_ASSERT(pResult == NULL);

	return 0;
}
