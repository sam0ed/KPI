#include "heapS.h"

void heapify(int* arr, int length, int indCheck)
{
	for (int i = 1; i < 3; i++)
	{
		if (arr[indCheck] < arr[(indCheck * 2) + i] && (indCheck * 2) + i < length)
		{
			swapEl(arr, indCheck, 2 * indCheck + i);
			heapify(arr, length, 2 * indCheck + i);
		}
	}
}

void heapSort(int* arr, int length)
{
	for (int i = floor(length / 2) - 1; i >= 0; i--)
	{
		heapify(arr, length, i);
	}

	for (int i = length - 1; i >= 0; i--)
	{
		swapEl(arr, 0, i);
		heapify(arr, i, 0);
	}
}