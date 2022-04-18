#include "quickS.h"

int partition(int* arr, int lb, int ub)
{
	int start = lb;
	int end = ub;
	int part = arr[lb];
	while (start < end)
	{
		while (arr[start] <= part&& start<ub)
			start++;
		while (arr[end] >/*=*/ part&& end>lb)
			end--;
		if (start < end)
		{
			swapEl(arr, start, end);
		}
	}
	swapEl(arr, lb, end);
	return end;
}

void quickSort(int* arr, int lb, int ub)
{

	if (ub - lb >= 1)
	{
		int mid = partition(arr, lb, ub);
		quickSort(arr, lb, mid-1 );
		quickSort(arr, mid + 1, ub);
	}
}