#include <iostream>
#include <random>
#include <algorithm>
#include <iomanip>
#include <ctime>
using namespace std;

enum arrayTypes
{
	ASCENDING,
	DESCENDING,
	RANDOM,
};
void bubbleSort(int* arr, int length, int& swapCounter, int& compCounter);
void combSort(int* arr, int length, int& swapCounter, int& compCounter);
int* generateIntArr(int size, arrayTypes arrayType);
void printArr(int* arr, int length);
void testAlgorithm(string&& name, void (*sortFunc)(int*, int, int&, int&), int& length, arrayTypes type);
int main( )
{
	srand(time(NULL));
	rand( );
	int length = 5000;
	testAlgorithm("bubbleSort", bubbleSort, length,arrayTypes:: ASCENDING );
	testAlgorithm("bubbleSort", bubbleSort, length, arrayTypes::DESCENDING);
	testAlgorithm("bubbleSort", bubbleSort, length, arrayTypes::RANDOM);

	cout << '\n';

	//testAlgorithm("combSort", combSort, length, arrayTypes::ASCENDING);
	//testAlgorithm("combSort", combSort, length, arrayTypes::DESCENDING);
	//testAlgorithm("combSort", combSort, length, arrayTypes::RANDOM);

	return 0;
}


void testAlgorithm(string&& name, void (*sortFunc)(int*, int, int&, int&),  int& length, arrayTypes type)
{
	string arrayType;
	switch (type)
	{
	case ASCENDING: arrayType = "ascending"; break;
	case DESCENDING: arrayType = "descending"; break;
	case RANDOM: arrayType = "random"; break;
	}
	cout << "\nYou are testing " << name << " algorithm on "<<arrayType<<" array with " << length << " elements." << endl;
	int swapCounter, compCounter;
	int* arr = generateIntArr(length, type);
	if (length < 50)
	{
		cout << "Array before sorting: " << endl;
		printArr(arr, length);
	}
	sortFunc(arr, length, swapCounter, compCounter);
	if (length < 50)
	{
		cout << "Array after sorting: " << endl;
		printArr(arr, length);
	}
	cout << "Amount of comparisons: " << compCounter << endl;
	cout << "Amount of swappings: " << swapCounter << endl;
}
int* generateIntArr(int size, arrayTypes arrayType)
{
	int* arr = new int[size];
	switch (arrayType)
	{
	case ASCENDING:
		for (int i = 0; i < size; i++)
		{
			arr[i] = i + 1;
		}
		break;
	case DESCENDING:
		for (int i = 0; i < size; i++)
		{
			arr[i] = size - i;
		}
		break;
	case RANDOM:
		arr = generateIntArr(size, ASCENDING);
		int ind1, ind2, temp;
		for (int i = 0; i < size; i++)
		{
			ind1=rand( ) % size;
			ind2 = rand( ) % size;
			temp = arr[ind1];
			arr[ind1] = arr[ind2];
			arr[ind2] = temp;
		}
		break;
	}
	return arr;
}
void combSort(int* arr, int length, int& swapCounter, int& compCounter)
{
	int gap = length;
	swapCounter = 0;
	compCounter = 0;
	bool swap = true;

	while (swap || gap > 1)
	{
		swap = false;
		if (gap > 1) gap = floor(double(gap) / 1.247);
		else gap = 1;
		for (int i = gap; i < length; i++)
		{
			if (arr[i] < arr[i - gap])
			{
				std::swap(arr[i], arr[i - gap]);
				swapCounter++;
				swap = true;
			}
			compCounter++;
		}
	}
}

void bubbleSort(int* arr, int length, int& swapCounter, int& compCounter)
{
	swapCounter = 0;
	compCounter = 0;
	bool swap = true;
	for (int i = 0; i < length - 1 && swap == true; i++)
	{
		swap = false;
		for (int j = 0; j < length - 1 - i; j++)
		{
			if (arr[j + 1] < arr[j])
			{
				std::swap(arr[j + 1], arr[j]);
				swapCounter++;
				swap = true;
			}
			compCounter++;
		}
	}

}
void printArr(int* arr, int length)
{
	for (int i = 0; i < length; i++)
	{
		cout << setw(4) << arr[i];
	}
	cout << endl;
}