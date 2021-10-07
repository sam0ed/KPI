#include <iostream>
#include <cmath>

using namespace std;

void arrayInit(int* p, int size, int arrayNumber)
{
	int input;
	for (int i = 0; i < size; ++i)
	{
		int* copy = p + i;
		cout << "Enter the " << i + 1 << " element of the "<<arrayNumber<<" array: ";
		cin >> input;
		*copy = input;
	}
}

int findTheSmallestNotMatching(int* m, int* k, int size)
{
	bool foundMatch;
	int theSmallest=INT_MAX;
	for (int i = 0; i < size; ++i)
	{
		foundMatch = false;
		for (int j = 0; j < size; ++j)
		{
			if (*(m + i) == *(k + j)) foundMatch = true;
		}
		if (!foundMatch && theSmallest > * (m + i))
		{
			theSmallest = *(m + i);
		}

	}
	return theSmallest;
}
void main( )
{
	int m[100], k[100], inputSize;
	do
	{
		cout << "Enter the number of elements you want to enter: ";
		cin >> inputSize;
		cout << "\n";
	}
	while (inputSize > 100);


	arrayInit(m, inputSize, 1);
	arrayInit(k, inputSize, 2);
	cout << "\nThe smallest not matching element is: " << findTheSmallestNotMatching(m, k, inputSize) << endl;
}

