#include <iostream>
#include <cmath>
#include <iomanip>

using namespace std;
int main( )
{
	// вводимо кількість ітерацій циклу і зчитуємо в змінну n
	cout << " Enter the n value(amounnt of iterations of calculating the equation): "; 
	int n;
	cin >> n;

	// встановлюємо обмеження на точність виводу результатів обчислень
	cout <<'\n'<< setprecision(10)<<"While calculating the equation n times we have found positive numbeers: \n"; 
	double a;

	// за допомогою циклу обчислюємо значення а при заданій і
	for (int i = 1;i<=n; ++i)
	{
		a = (i - 1) / (i + 1) + sin(pow(i - 1, 3) /( i + 1));
		if (a > 0) cout << " " << a<<'\n';
		else continue;
	}
	return 0;
}