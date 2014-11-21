#include <tchar.h>
#include <stdlib.h>
#include <malloc.h>
#include <assert.h>
#include <vector>
#include <algorithm>
#include <iostream>

bool BuildPair(unsigned int n)
{
    auto numbers = std::vector<unsigned int>(n);
    unsigned long count = 1;
    std::generate(numbers.begin(), numbers.end(), [&count](){ return count++; });

    do
    {
        auto buffer = std::vector<unsigned int>(2 * n, 0);
        unsigned long position = 0;
        auto iter = numbers.begin();
        for (; iter < numbers.end(); ++iter)
        {
            while (position < 2 * n && buffer[position] != 0)
            {
                ++position;
            }

            unsigned long pairPosition = position + *iter + 1;
            if (pairPosition >= 2 * n)
            {
                break;
            }

            if (buffer[pairPosition] != 0)
            {
                break;
            }

            buffer[position] = buffer[pairPosition] = *iter;
            ++position;
        }

        if (iter == numbers.end())
        {
            for each (unsigned int value in buffer)
            {
                std::cout << value << ' ';
            }
            std::cout << std::endl;
            return true;
        }
    }
    while (std::next_permutation(numbers.begin(), numbers.end()));
    return false;
}

int _tmain(int argc, _TCHAR* argv[])
{
    assert(BuildPair(12) == true);
    assert(BuildPair(11) == true);
    assert(BuildPair(10) == false);
    assert(BuildPair(9) == false);
    assert(BuildPair(8) == true);
    assert(BuildPair(7) == true);
    assert(BuildPair(6) == false);
    assert(BuildPair(5) == false);
    assert(BuildPair(4) == true);
    assert(BuildPair(3) == true);
    assert(BuildPair(2) == false);
    assert(BuildPair(1) == false);

    return 0;
}