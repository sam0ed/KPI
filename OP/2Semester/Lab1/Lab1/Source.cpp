#include "Func.h"

/*Створити текстовий файл. Переписати до нового файлу всі компоненти вихідного файлу
замінивши в них символ 0 на 1 і навпаки. Вивести вміст вихідного і створеного файлів*/
int main( )
{
	string path = "C:/Users/User/source/repos/KPI/OP/2Semester/Lab1/Files/";
	string firstName = "first_file.txt";
	string secondName = "second_file.txt";
	rewriteFile(path+firstName);
	invertBinaryDigits(path + firstName, path + secondName);
	printFile(path+firstName);
	printFile(path + secondName);

	return 0;
}
