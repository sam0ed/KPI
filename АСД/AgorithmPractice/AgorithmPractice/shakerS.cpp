#include "shakerS.h"
void shakerSort(int* arr, int size)
{
	bool moveRight = 1;
	int rightSortedEl = 0, leftSortedEl = 0;
	for (int i = 0; i < size; i++)
	{
		if (moveRight)
		{
			for (int i = leftSortedEl; i < size - rightSortedEl - 1; i++)
			{
				if (arr[i] > arr[i + 1])
				{
					swapEl(arr, i, i + 1);

				}
			}
			rightSortedEl++;
			moveRight = 0;
		}
		else
		{
			for (int i = size - rightSortedEl - 1; i > leftSortedEl - 1; i--)
			{
				if (arr[i] > arr[i + 1]) swapEl(arr, i, i + 1);
			}
			leftSortedEl++;
			moveRight = 1;
		}
	}
}