#include <iostream>
#include <cmath>
#include <sstream>

using namespace std;
int main( )
{
	// отримуємо ввід користувача
	cout << "Enter the number you want to get the digit root from: ";
	int number;
	cin >> number;

	// якщо ввід відємний то перетворюємо число на додатнє для правильної роботи програми
	if (number < 0) number *= (-1);

	// ініціалізуємо змінні що будуть використовуватись для обчислення кореня
	int copy, counter, sum;
	
	// для кожного проміжного числа що складаєтьсяя більше ніж з одного розряду
	while (number >= 10)
	{
		// скидаємо попередні значення  циклу або просто ініціалізуємо змінні початковим значенням
		copy = number;
		counter = 0;
		sum = 0;

		// обчислюємо кількість цифр в поточному number
		while (abs(copy) >= 1)
		{
			copy /= 10;
			++counter;
		}

		// знаходимо суму всіх чисел в поточному number
		for (int i = pow(10, counter - 1); i >= 1; i /= 10)
		{
			int add = static_cast<int>(number / i);
			sum += add;
			number -= (add * i);
		}

		// записуємо знайдену суму в number
		number = sum;
	}

	// вивід результату
	cout << "The digit root of the entered value is: "<< number<< '\n';

	return 1;
}