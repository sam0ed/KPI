#include <iostream>
# include <cmath>

using namespace std;

long long int factorial(long int value)
{
	long long fact = 1;
	if (value == 0)
		return 1;
	else if (value > 0)
	{
		for (int i = 1; i <= value; ++i)
		{
			fact *= i;
		}
		return fact;
	}
	else if (value < 0)
	{
		cout << " Can`t get factorial of this number.\n";
		exit(-1);
	}
}

double function(double x)
{
	long double numerator=0, denominator=0;
	
	for (int k = 0; k < 5; ++k)
	{
		numerator += pow(x, (2 * k) + 1)/factorial(2*k+1);
		denominator += pow(x, 3 * k) / factorial(3 * k);
	}
	
	return (numerator / denominator);
}

void main( )
{
	cout << "Enter the y value please: ";
	double y;
	cin >> y;
	double result = (1.7 * function(0.25) + 2 * function(1 + y)) / (6 - function(pow(y, 2) - 1));
	cout << "The result of the equation is " << result << endl;
}