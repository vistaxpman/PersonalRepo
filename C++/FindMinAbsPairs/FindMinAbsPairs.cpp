// Compile: CL.EXE .\FindMinAbsPairs.cpp

#include <vector>
#include <limits>
#include <tchar.h>

using namespace std;

typedef vector<pair<int, int>> ResultVector;

// Assume the array is sorted ASC.
// If not, sort the arary.
ResultVector FindMinAbsPairs(int array[], size_t length)
{
	ResultVector result;

	if (length < 2)
	{
		// The array length is less than 2;
		return result;
	}

	if (length == 2)
	{
		// The array only contains 2 elements
		result.push_back(make_pair(array[0], array[1]));
		return result;
	}

	if (array[0] >= 0)
	{
		// All positive elements.
		// Pick up the miminial 2 elements.
		for (size_t i = 1; i < length && (array[i] == array[1]); i++)
		{
			result.push_back(make_pair(array[0], array[1]));
		}
		return result;
	}

	if (array[length - 1] <= 0)
	{
		// All negative elements.
		// Pick up the miximal 2 elements.
		for (size_t i = length - 2; i >= 0 && (array[i] == array[length - 2]); i--)
		{
			result.push_back(make_pair(array[length -2], array[length -1]));
		}
		return result;
	}

	size_t pos = 0;
	for (; ((array[pos] < 0) && (array[pos + 1] < 0)); pos++)
	{}
	size_t neg = pos;

	for (; array[pos + 1] == 0; pos++)
	{}
	pos++;

	unsigned int minimalSum = numeric_limits<unsigned int>::max();

	if (pos -neg > 2)
	{
		for (size_t i = 0; i < pos - neg - 2; i++)
		{
			// There are mulitple '0'.
			minimalSum = 0;
			result.push_back(make_pair(0, 0));
		}
	}
	else if (pos -neg == 2)
	{
		size_t i = 0;
		if (-array[neg] > array[pos])
		{
			minimalSum = -array[neg];
			for (; (neg - i >= 0) && (array[neg - i] == array[neg]); i++)
			{
				result.push_back(make_pair(array[neg], 0));
			}
			neg -= i;
		}
		else if (-array[neg] == array[pos])
		{
			minimalSum = 0;
			size_t j = 1;
			for (; (neg - i >= 0) && (array[neg - i] == array[neg]); i++)
			{
				result.push_back(make_pair(array[neg], array[pos]));
			}

			for (j = 0; (pos + j < length) && (array[pos + j] == array[pos]); j++)
			{
				
			}
			neg -= i;
			pos += j;
		}
		else
		{
			// -array[neg] < array[pos]
			minimalSum = array[pos];
			for (; (pos + i >= 0) && (array[pos + i] == array[pos]); i++)
			{
				result.push_back(make_pair(0, array[pos]));
			}

			pos += i;
		}
	}

	while ((neg >= 0) && (pos < length))
	{
		unsigned int newSum = abs(array[neg] + array[pos]);
		if (newSum < minimalSum)
		{
			minimalSum = newSum;
			result.clear();
		}

		if (newSum <= minimalSum)
		{
			size_t i = 0, j = 1;
			for (; (neg - i >= 0) && (array[neg - i] == array[neg]); i++)
			{
				result.push_back(make_pair(array[neg], array[pos]));
			}
			
			for (; (pos + j < length) && (array[pos + j] == array[pos]); j++)
			{
				result.push_back(make_pair(array[neg], array[pos]));
			}
			neg -= (i - 1);
			pos += (j - 1);
		}

		if ((neg == 0) || (pos == length -1))
		{
			break;
		}

		if (-array[neg] < array[pos])
		{
			do
			{
				neg--;
			} while ((neg >=0) && (-array[neg] < array[pos]));
		}
		else
		{
			do
			{
				pos++;
			} while ((pos < length) && (-array[neg] > array[pos]));
		}
	}

	return result;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int a1[] = {0};
	ResultVector result = FindMinAbsPairs(a1, 1);

	int a2[] = {0, 1};
	result = FindMinAbsPairs(a2, 2);

	int a3[] = {0, 1, 2};
	result = FindMinAbsPairs(a3, 3);

	int a4[] = {0, 1, 1};
	result = FindMinAbsPairs(a4, 3);

	int a5[] = {-2, -1, 0};
	result = FindMinAbsPairs(a5, 3);

	int a6[] = {-1, -1, 0};
	result = FindMinAbsPairs(a6, 3);

	int a7[] = {-1, 0, 1};
	result = FindMinAbsPairs(a7, 3);

	int a8[] = {-1, 0, 0, 1};
	result = FindMinAbsPairs(a8, 4);

	int a9[] = {-1, 0, 0, 0, 1};
	result = FindMinAbsPairs(a9, 5);

	int a10[] = {-3, -2, -1, 0, 1, 2, 3, 4};
	result = FindMinAbsPairs(a10, 8);

	int a11[] = {-3, -2, -1, 1, 2, 3, 4};
	result = FindMinAbsPairs(a11, 7);

	int a12[] = {-3, -2, -1, -1, -1, 1, 2, 3, 4};
	result = FindMinAbsPairs(a12, 9);
}
