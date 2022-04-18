#include <iostream>

using namespace std;

int** matrixInit(int rowLength, int columnLength)
{
	int** matrix = new int* [rowLength];
	cout << "Enter the matrix using spaces to divide elements: " << endl;
	for (int i = 0; i < rowLength; ++i)
	{
		matrix[i] = new int[columnLength];
		for (int j = 0; j < columnLength; ++j)
		{
			cin >> matrix[i][j];
			cin.ignore(1);
		}
	}
	return matrix;
}

double columnArithmeticAverage(int** matrix, int columnNum, int rowLength)
{
	double sum = 0;
	for (int i = 0; i < rowLength; ++i)
	{
		sum += static_cast<double>(matrix[i][columnNum]);
	}
	return (sum / static_cast<double>(rowLength));
}

double mainCycle(int** matrix1, int** matrix2, int rowLength, int columnLength)
{
	double sum = 0;
	for (int i = 0; i < rowLength; ++i)
	{
		sum += abs(columnArithmeticAverage(matrix1, i, rowLength) - columnArithmeticAverage(matrix2, i, rowLength));
	}
	return sum;
}

//additional code for displaying matrices
//void printMatrix(int** matrix, int rowLength, int columnLength)
//{
//	for (int i = 0; i < rowLength; ++i)
//	{
//		for (int j = 0; j < columnLength; ++j)
//		{
//			cout << matrix[i][j];
//			cout << " ";
//		}
//		cout << '\n';
//	}
//}

void deleteMatrix(int** matrix, int rowLength, int columnLength)
{
	for (int i = 0; i < rowLength; ++i)
	{
		delete[] matrix[i];
	}
	delete[] matrix;
}

int main( )
{
	// user enters the wanted 1size of matrix
	int rowLength;
	int columnLength;
	std::cout << "Enter the type of matrixes you want to create(rows columns): ";
	cin >> rowLength >> columnLength;

	// creation of two dynamically allocated matrixes of the given size 
	int** matrix1 = matrixInit(rowLength, columnLength);
	int** matrix2 = matrixInit(rowLength, columnLength);

	//calculating the result according top the given formula and displaying it on the screen
	cout << "The resulting sum is " << mainCycle(matrix1, matrix2, rowLength, columnLength) << endl;

	//deleting dynamically allocated two dimensional arrays
	deleteMatrix(matrix1, rowLength, columnLength);
	deleteMatrix(matrix2, rowLength, columnLength);
	return 1;
}