// Compile: CL.EXE .\MaxSum.cpp

#include <algorithm>
#include <tchar.h>

using namespace std;

int MaxSum1(int array[], int length)
{
	int maxSum = 0;

	for (int i = 0; i < length; i++)
	{
		for (int  j = i; j < length; j++)
		{
			int sum = 0;
			for (int k = i; k <= j; k++)
			{
				sum += array[k];
			}
			maxSum = max(sum, maxSum);
		}
	}

	return maxSum;
}

int MaxSum2(int array[], int length)
{
	int maxSum = 0;

	for (int i = 0; i < length; i++)
	{
		int sum = 0;
		for (int j = i; j < length; j++)
		{
			sum += array[j];
			maxSum = max(maxSum, sum);
		}
	}

	return maxSum;
}

int MaxSum3(int array[], int length)
{
	int *pCumulate = new int[length];
	memcpy(pCumulate, array, length * sizeof(int));

	for (int *p = pCumulate + 1; p < pCumulate + length; p++)
	{
		*p = *(p -1) + *p;
	}
	
	int maxSum = 0;
	for (int i = 0; i < length; i++)
	{
		for (int j = i; j < length; j++)
		{
			maxSum = max(maxSum, *(pCumulate + j) - *(pCumulate + i));
		}
	}

	delete []pCumulate;
	return maxSum;
}

int MaxSum4(int array[], int l, int u)
{
	if (l > u)
	{
		// 0 element
		return 0;
	}

	if (l == u)
	{
		// 1 element
		return max(0, array[l]);
	}

	int m = (l + u) / 2;

	int lMaxSum = 0;
	int sum = 0;
	for (int i = m; i >= l; i--)
	{
		sum += array[i];
		lMaxSum = max(lMaxSum, sum);
	}

	sum = 0;
	int rMaxSum = 0;
	for (int j = m + 1; j <= u; j++)
	{
		sum += array[j];
		rMaxSum = max(rMaxSum, sum);
	}

	return max(lMaxSum + rMaxSum, max(MaxSum4(array, l, m - 1), MaxSum4(array, m + 1, u)));
}

int MaxSum5(int array[], int length)
{
	int maxSum = 0;
	int maxEnd = 0;

	for (int i = 0; i < length; i++)
	{
		maxEnd = max(maxEnd + array[i], 0);
		maxSum = max(maxSum, maxEnd);
	}

	return maxSum;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int array[] = {31, -41, 59, 26, -53, 58, 97, -93, -23, 84};
	int length = sizeof(array) / sizeof(int);
	int result =  MaxSum1(array, length);
	_ASSERT(result == 187);

	result = MaxSum2(array, length);
	_ASSERT(result == 187);

	result = MaxSum3(array, length);
	_ASSERT(result == 187);

	result = MaxSum4(array, 0, length - 1);
	_ASSERT(result == 187);

	result = MaxSum5(array, length);
	_ASSERT(result == 187);

	return 0;
}