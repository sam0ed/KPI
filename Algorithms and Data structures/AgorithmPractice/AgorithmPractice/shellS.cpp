#include "shellS.h"
void shellSort(int* arr, int size)
{
	for (int i = size / 2; i > 0; i /= 2)
	{
		for (int j = i; j < size; j++)
		{
			for (int k = j - i; k >= 0; k -= i)
			{
				if (arr[k + i] < arr[k])
				{
					swapEl(arr, k, k + i);
				}
			}
		}
	}
}