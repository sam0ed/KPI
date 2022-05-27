#include "quickS.h"

int partition(int* arr, int lb, int ub, PivotType pivotType = PivotType::MIDDLE)
{
	int start = lb;
	int end = ub;
	int pivotIndex;
	switch (pivotType)
	{
	case PivotType::FIRST:
		pivotIndex = lb;
		break;
	case PivotType::MIDDLE:
		pivotIndex = (floor)(lb + ub) / 2;
		break;
	case PivotType::LAST:
		pivotIndex = ub;
		break;
	default:
		pivotIndex = (floor)(lb + ub) / 2;
		break;
	}
	int part = arr[pivotIndex];
	swapEl(arr, pivotIndex, ub);
	while (start < end)
	{
		while (arr[start] < part && start < ub)
			start++;
		while (arr[end] >= part && end > lb)
			end--;
		if (start < end)
		{
			swapEl(arr, start, end);
		}
	}
	swapEl(arr, start, ub);
	return start;

}

void quickSort(int* arr, int lb, int ub, PivotType pivotType)
{

	if (ub - lb >= 1)
	{
		int mid = partition(arr, lb, ub, pivotType);
		quickSort(arr, lb, mid - 1, pivotType);
		quickSort(arr, mid + 1, ub, pivotType);
	}
}