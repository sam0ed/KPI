#include <iostream>
#include <cstdlib> 
#include <ctime>
#include <iomanip>>

using namespace std;

//void arrayInit(int* p, int size, int arrayNumber)
//{
//	int input;
//	for (int i = 0; i < size; ++i)
//	{
//		int* copy = p + i;
//		cout << "Enter the " << i + 1 << " element of the " << arrayNumber << " array: ";
//		cin >> input;
//		*copy = input;
//	}
//}

void arrayInit(int* p, int size);
int findTheSmallestNotMatching(int* m, int* k, int size);
void printArray(int* arr, int size, int arrayNum);

void main( )
{
	srand(time(0));
	int inputSize;
	cout << "Enter the number of elements you want to enter: ";
	cin >> inputSize;
	cout << "\n";
	int* m = new int[inputSize];
	int* k = new int[inputSize];

	//arrayInit(m, inputSize, 1);
	//arrayInit(k, inputSize, 2);

	arrayInit(m, inputSize);
	arrayInit(k, inputSize);

	printArray(m, inputSize, 1);
	printArray(k, inputSize, 2);

	int result = findTheSmallestNotMatching(m, k, inputSize);
	if (result) cout << "\nThe smallest not matching element is: " << result << endl;
	else cout << "\nThe smallest not matching element is not found." << endl;
}

void arrayInit(int* p, int size)
{
	for (int i = 0; i < size; ++i)
	{
		*(p + i) = (rand( ) % size) + 1;
	}
}

int findTheSmallestNotMatching(int* m, int* k, int size)
{
	bool foundMatch;
	int theSmallest = INT_MAX;
	for (int i = 0; i < size; ++i)
	{
		foundMatch = false;
		for (int j = 0; j < size; ++j)
		{
			if (*(m + i) == *(k + j)) foundMatch = true;
		}
		if (!foundMatch && theSmallest > *(m + i))
		{
			theSmallest = *(m + i);
		}

	}
	return theSmallest != INT_MAX ? theSmallest : 0;
}

void printArray(int* arr, int size, int arrayNum)
{
	cout << setw(3) << "The lements of the " << arrayNum << " array are:\n";
	for (int i = 0; i < size; ++i)
		cout << *(arr + i) << " ";
	cout << endl;
}