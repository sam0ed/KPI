#include "Func.h"
void printFile(const string& fileName)
{
	cout << "The contents of the file " << fileName.substr(fileName.find_last_of('/', fileName.length( )) + 1) << ":\n";
	std::ifstream fileIn(fileName);
	string temp;
	while (!fileIn.eof( ))
	{
		getline(fileIn, temp);
		cout << temp << endl;
	}
	fileIn.close( );
}

void appendFile(const string& fileName)
{
	cout << "You are editing: " << fileName.substr(fileName.find_last_of('/', fileName.length( )) + 1) << "\n";
	cout << "Press ctrl+E to stop the edit. \nPlace your input here:\n";

	string temp;
	ofstream file(fileName, ios::out | ios::app);
	if (file.is_open( ))
	{
			getline(cin, temp, char(5));
			if(temp.back()!='\n') temp += '\n';
			file << temp;
	}
	else
	{
		cout << "Error occured while trying to open file" << fileName.substr(fileName.find_last_of('/', fileName.length( )) + 1);
	}
	cout << endl;
	file.close( );
}

void rewriteFile(const string& fileName)
{
	ofstream file(fileName);
	file.close( );
	appendFile(fileName);
}

void invertBinaryDigits(const string& fileName, const string& newFileName)
{
	ifstream file(fileName);
	ofstream newFile(newFileName);
	string temp;
	while (!file.eof( ))
	{
		getline(file, temp);
		temp += '\n';
		for (int i = 0; i < temp.length( ); i++)
		{
			if (temp[i] == '0') temp.replace(i, 1, "1");
			else if (temp[i] == '1') temp.replace(i, 1, "0");
		}


		newFile << temp;
	}
	file.close( );
	newFile.close( );
}

