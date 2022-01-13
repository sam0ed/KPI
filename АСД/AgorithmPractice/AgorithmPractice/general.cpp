#include "general.h"

void swapEl(int* arr, int ind1, int ind2)
{
	int temp = arr[ind1];
	arr[ind1] = arr[ind2];
	arr[ind2] = temp;
}

void copyArr(int* source, int length, int* destination)
{
	for (int i = 0; i < length; i++)
	{
		*(destination + i) = *(source + i);
	}
}

void randArrInit(int* arr, int size)
{
	for (int i = 0; i < size; i++)
	{
		arr[i] = rand( ) % (5 * (size + 1));
	}
}
void printArr(int* arr, int size)
{
	for (int i = 0; i < size; i++)
	{
		cout << arr[i] << ' ';
	}
	cout << endl;
}