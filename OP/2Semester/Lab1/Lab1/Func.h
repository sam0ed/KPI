#pragma once
#include <iostream>
#include <fstream>
#include <ostream>
#include <istream>
#include <Windows.h>
#include <thread>

#include <string>
using namespace std;
void rewriteFile(const string& fileName);
void printFile(const string & fileName);
void appendFile(const string& fileName);
void invertBinaryDigits(const string& fileName, const string& newFileName);