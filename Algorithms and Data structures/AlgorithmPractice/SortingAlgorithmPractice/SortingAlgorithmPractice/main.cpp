#include "general.h"
#include "mergeS.h"
#include "quickS.h"
#include "shakerS.h"
#include "shellS.h"
#include "heapS.h"

int main( )
{
	srand(time(NULL));
	rand( );


	int size = 100000;
	int* arr = new int[size];
	randArrInit(arr, size);
	/*int arr[] = { 5, 8, 9, -3, 0, -5, 30, 18, -4, -9, 6, 12, 11, 15, 25, 18, 24, 16, 13, 19 };
	int size = sizeof(arr) / sizeof(int);*/


	auto start = std::chrono::high_resolution_clock::now( );
	/*mergeSort(arr, 0, size - 1);*/
	//heapSort(arr, size);
	quickSort(arr, 0, size - 1, PivotType::MIDDLE);
	//shellSort(arr, size);
	//shakerSort(arr, size);
	auto end = std::chrono::high_resolution_clock::now( );
	auto duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
	cout << duration.count( ) << endl;

	printArr(arr, size);

	return 0;
}