// Lab1(c++).cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>

int main( )
{
	int b;
	double q;
	std::cout << "Enter the first member of infinite deteriorating geomtryc progression: ";
	std::cin >> b;
	std::cout << "\nEnter the denominator of infinite deteriorating geometric progression: ";
	std::cin >> q;

	double sum=b/(1-q);

	std::cout << "\nThe sum of infinite deterirating geometric progression is " << sum;

	return 1;
}
