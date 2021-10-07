# include <iostream>
#include <cmath>
#include <iomanip>
#include <algorithm>

using namespace std;

//int main( )
//{
//	long double x, n, sum = 0, add = 0;
//
//	cout << "Enter x value: ";
//	cin >> x;
//	
//
//	int k = 0;
//	int counterFact = 1;
//	int doubleCounterFact = 1;
//	bool finish = false;
//
//	while (!finish)
//	{
//		if (k != 0)
//		{
//			counterFact *= k;
//			doubleCounterFact *= (2 * k * (2*k - 1));
//		}
//		
//		add = pow(-1, k) * pow((x / 2), 3*k) / (counterFact * doubleCounterFact);
//		if (fabs(add) >= 1e-4)
//			sum += add;
//		else
//			finish = true;
//		std::cout << "Iteration " << k << " worked here. The current value of sum is: " << sum << std::endl;
//		++k;
//	}
//	cout << "\nThe sum is: " << sum;
//
//	return 0;
//}