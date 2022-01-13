#include "mergeS.h"
void merge(int* arr, int lb, int mid, int ub)
{
	int i = lb;
	int k = 0;
	int j = mid + 1;
	int* newArr = new int[ub - lb + 1];
	while (i <= mid && j <= ub)
	{
		if (arr[i] <= arr[j])
		{
			newArr[k] = arr[i];
			++i;
		}
		else
		{
			newArr[k] = arr[j];
			++j;
		}
		++k;
	}
	if (i > mid)
	{
		while (j <= ub)
		{
			newArr[k] = arr[j];
			++j; ++k;
		}
	}
	else
	{
		while (i <= mid)
		{
			newArr[k] = arr[i];
			++i; ++k;
		}
	}
	copyArr(newArr, ub - lb + 1, arr + lb);

}

void mergeSort(int* arr, int lb, int ub)
{
	if (ub - lb > 0)
	{
		int mid = (ub + lb) / 2;
		mergeSort(arr, lb, mid);
		mergeSort(arr, mid + 1, ub);
		merge(arr, lb, mid, ub);
	}
}