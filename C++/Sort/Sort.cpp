#include <cstddef>
#include <limits>
#include <algorithm>
#include <tchar.h>

using namespace std;

// InsertionSort
void InsertionSort(int array[], size_t length)
{
	for (size_t i = 1; i < length; i++)
	{
		int key = array[i];
		for (size_t j = i - 1; j >= 0; j--)
		{
			if (array[j] > key)
			{
				swap(array[j], array[j + 1]);
			}
			else
			{
				break;
			}
		}
	}
}

// BubbleSort
void BubbleSort(int array[], size_t length)
{
	for (size_t i = length - 1; i > 0; i--)
	{
		for (size_t j = 0; j < i; j++)
		{
			if (array[j] > array[j + 1])
			{
				swap(array[j], array[j + 1]);
			}
		}
	}
}

// MergeSort
void Merge(int array[], size_t l, size_t m, size_t u)
{
	size_t lLength = m - l + 2;
	size_t rLength = u - m + 1;
	int *pLhs = new int[lLength];
	int *pRhs = new int[rLength];

	memcpy(pLhs, &array[l], (lLength - 1) * sizeof(int));
	memcpy(pRhs, &array[m + 1], (rLength - 1) * sizeof(int));
	pLhs[lLength - 1] = pRhs[rLength - 1] = numeric_limits<int>::max();

	int *pL = pLhs;
	int *pR = pRhs;
	for (int *pArray = array + l; pArray < array + u + 1; pArray++)
	{
		if (*pL > *pR)
		{
			*pArray = *(pR++);
		}
		else
		{
			*pArray = *(pL++);
		}
	}
	delete []pLhs;
	delete []pRhs;
}

void MergeSort(int array[], size_t l, size_t u)
{
	if (l < u)
	{
		size_t m = (l + u) / 2;
		MergeSort(array, l, m);
		MergeSort(array, m + 1, u);
		Merge(array, l, m, u);
	}
}

// HeapSort
size_t left(size_t i)
{
	return i << 1;
}

size_t right(size_t i)
{
	return (i << 1) + 1;
}

void Heapify(int array[], size_t i, size_t length)
{
	size_t l = left(i);
	size_t r = right(i);

	size_t largest = i;
	if ((l < length) && (array[l] > array[i]))
	{
		largest = l;
	}

	if ((r < length) && (array[r] > array[largest]))
	{
		largest = r;
	}

	if (largest != i)
	{
		swap(array[largest], array[i]);
		Heapify(array, largest, length);
	}
}

void BuildHeap(int array[], size_t length)
{
	for (int j = length >> 1; j >= 0; j--)
	{
		Heapify(array, j, length);
	}
}

void HeapSort(int array[], size_t length)
{
	BuildHeap(array, length);
	for (size_t i = length - 1; i > 0; i--)
	{
		swap(array[0], array[i]);
		Heapify(array, 0, i - 1);
	}
}

// QuickSort
int *Partition(int *pLower, int *pUpper)
{
	int pivot = *pUpper;
	int *pI = pLower - 1;
	int *pJ = pLower;

	for (; pJ < pUpper; pJ++)
	{
		if (*pJ < pivot)
		{
			swap(*(++pI), *pJ);
		}
	}
	swap(*(++pI), *pUpper);

	return pI;
}

void QuickSort(int *pLower, int *pUpper)
{
	if (pLower < pUpper)
	{
		int *pPivot = Partition(pLower, pUpper);
		QuickSort(pLower, pPivot - 1);
		QuickSort(pPivot + 1, pUpper);
	}
}

// main
int _tmain(int argc, _TCHAR* argv[])
{
	int a1[] = {5, 4, 5, 2, 9, 3};
	// BubbleSort(a1, 6);
	// InsertionSort(a1, 6);
	// MergeSort(a1, 0, 5);
	// HeapSort(a1, 6);
	// QuickSort(a1, a1 + 5);
	return 0;
}
