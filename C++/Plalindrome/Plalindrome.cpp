#include <limits>
#include <cassert>
#include <tchar.h>

bool TestPlalindrome(unsigned int testedNumber)
{
    // calculate digits
    // NOTE: UINT_MAX is 65535
start:
    // O must be plalindrome
    if (testedNumber == 0)
    {
        return true;
    }

    unsigned int digit = testedNumber % 10;
    // Number like 10, 20, 100, 100: they are not plalindrome
    if (digit == 0)
    {
        return false;
    }

    // Sigle digit must be plalindrome
    if (digit == testedNumber)
    {
        return true;
    }

    for (unsigned int i = 1; i <= 1000000000; i *= 10)
    {
        unsigned int remainder = testedNumber - (digit * i);
        if (remainder < 0)
        {
            // The first digit is less than the last digit
            return false;
        }

        if (remainder < i)
        {
            // The first digit equals to the last digit, we should compare other digits
            testedNumber = remainder / 10;
            // Recursion may be more readable, but the growth of stack consumes more memory.
            // So use evil goto here
            goto start;
        }
    }
}

int _tmain(int argc, _TCHAR* argv[])
{
    assert(TestPlalindrome(std::numeric_limits<unsigned int>::max()) == false);
    assert(TestPlalindrome(1234554321) == true);
    assert(TestPlalindrome(65535) == false);
    assert(TestPlalindrome(65534) == false);
    assert(TestPlalindrome(55555) == true);
    assert(TestPlalindrome(12321) == true);
    assert(TestPlalindrome(10002) == false);
    assert(TestPlalindrome(10001) == true);
    assert(TestPlalindrome(10000) == false);
    assert(TestPlalindrome(9999) == true);
    assert(TestPlalindrome(5665) == true);
    assert(TestPlalindrome(5656) == false);
    assert(TestPlalindrome(1002) == false);
    assert(TestPlalindrome(1001) == true);
    assert(TestPlalindrome(1000) == false);
    assert(TestPlalindrome(999) == true);
    assert(TestPlalindrome(998) == false);
    assert(TestPlalindrome(122) == false);
    assert(TestPlalindrome(121) == true);
    assert(TestPlalindrome(120) == false);
    assert(TestPlalindrome(111) == true);
    assert(TestPlalindrome(110) == false);
    assert(TestPlalindrome(102) == false);
    assert(TestPlalindrome(101) == true);
    assert(TestPlalindrome(100) == false);
    assert(TestPlalindrome(99) == true);
    assert(TestPlalindrome(98) == false);
    assert(TestPlalindrome(33) == true);
    assert(TestPlalindrome(23) == false);
    assert(TestPlalindrome(22) == true);
    assert(TestPlalindrome(20) == false);
    assert(TestPlalindrome(13) == false);
    assert(TestPlalindrome(12) == false);
    assert(TestPlalindrome(11) == true);
    assert(TestPlalindrome(10) == false);
    assert(TestPlalindrome(2) == true);
    assert(TestPlalindrome(1) == true);
    assert(TestPlalindrome(0) == true);
}
