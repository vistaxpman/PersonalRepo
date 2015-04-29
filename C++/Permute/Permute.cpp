#include "stdafx.h"

#include <algorithm>
#include <cctype>
#include <iostream>
#include <string>
#include <utility>
#include <vector>

using std::vector;

using std::cout;
using std::endl;
using std::reverse;
using std::swap;

void print(const vector<int> &v)
{
    for (auto m : v)
    {
        cout << m << ',';
    }
    cout << endl;
}

void permute_recursively(vector<int> &v, vector<int>::size_type begin, vector<int>::size_type end)
{
    if (begin == end)
    {
        print(v);
        return;
    }

    for (auto i = begin; i <= end; ++i)
    {
        swap(v[begin], v[i]);
        permute_recursively(v, begin + 1, end);
        swap(v[begin], v[i]);
    }
}

int nextPermutation(vector<int> &target)
{
    auto size = target.size();
    decltype(size) i, j;
    for (i = 1; i < size && target[i - 1] >= target[i]; ++i)
        ;

    if (i == size)
        // means the permutation is over
        return false;

    for (j = 0; target[j] >= target[i]; ++j)
        ;
    swap(target[i], target[j]);
    reverse(target.begin(), target.begin() + i);

    return true;
}

int _tmain(int argc, _TCHAR* argv[])
{
    vector<int> v{ 0, 1, 2, 3 };
    permute_recursively(v, 0, 3);

    cout << endl;
    v = { 0, 1, 2, 3 };
    while (nextPermutation(v))
    {
        print(v);
    }
}