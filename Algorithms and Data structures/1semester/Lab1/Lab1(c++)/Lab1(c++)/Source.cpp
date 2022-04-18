#include <iostream>
#include <cmath>


int main( )
{
	//просимо користувача ввести початкові дані та считуємо їх, записуючи у змінну
	std::cout << "Enter time period in minutes: " << std::endl; 
	int minutesInput;
	std::cin >> minutesInput;

	//вираховуємо кількість годин у введеному користувачем числі
	int minutes = minutesInput % 60;
	int hours = (minutesInput-minutes) / 60;

	//виводимо результуюче число записане у годинах і хвилинах на екран
	std::cout << "Your time period is " << hours << " hours, " << minutes << " minutes." << std::endl;

	//повертаємо код успішного виконання програми назад в систему
	return 1;
}