
//printArr(arr1);
	//printArr(arr2);
	//printArr(arr3);
	/*printArr(arr3);*/
#include <iostream>

void array1Init(char* arr);
void array2Init(char* arr);
void array3Init(char* arr1, char* arr2, char* arr3);
int resultCount(char* arr);
void printArr(char* arr);

int main( )
{
	char arr1[10], arr2[10], arr3[10] = { 0 };

	array1Init(arr1);
	array2Init(arr2);
	array3Init(arr1, arr2, arr3);
	printArr(arr3);
	std::cout <<"The amount of characters in array 3, which have numeric values less then 127 are: " << resultCount(arr3) << std::endl;
}

void array1Init(char* arr)
{
	for (int i = 0; i < 10; ++i)
		arr[i] = 130 - i;
}

void array2Init(char* arr)
{
	for (int i = 0; i < 10; ++i)
		arr[i] = 120 + i;
}

void array3Init(char* arr1, char* arr2, char* arr3)
{
	int indexArr = 0;
	for (int i = 0; i < 10; ++i)
	{
		for (int j = 0; j < 10; ++j)
		{
			if (arr2[j] == arr1[i])
			{
				arr3[indexArr] = arr1[i];
				indexArr++;
			}
		}
	}
}

int resultCount(char* arr)
{
	int n = 0;
	for (int i = 0; arr[i] != 0; ++i)
	{
		if (arr[i] > 0 && arr[i] < 127) n++;
	}
	return n;
}

void printArr(char* arr)
{
	for (int i = 0; i < 10; ++i) std::cout << arr[i] << " ";
	std::cout << '\n';
}