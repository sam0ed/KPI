using System;

namespace Lab1_sharp_
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:/Users/User/source/repos/KPI/OP/2Semester/Lab1/Files/";
            string firstName = "first_file.txt";
            string secondName = "second_file.txt";
            FileManager.RewriteFile(path + firstName);
            FileManager.InvertBinaryDigits(path + firstName, path + secondName);
            FileManager.PrintContents(path + firstName);
            FileManager.PrintContents(path + secondName);
        }
    }
}
