#pragma once
#include "general.h"
enum class PivotType
{
    FIRST,
    MIDDLE,
    LAST
};

int partition(int* arr, int lb, int ub, PivotType pivotType);
void quickSort(int* arr, int lb, int ub, PivotType pivotType);
