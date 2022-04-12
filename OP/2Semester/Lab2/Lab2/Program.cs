using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static Lab2.ProgramMethods;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath1 = "firstFile";
            string filePath2 = "secondFile";
            //Getting user input and writing it into the file
            WriteIEnumerableToFile<ProgramTV>(filePath1, GetInputPrograms());
            //displaying the content of the first file
            List<ProgramTV> fileContent = GetFilePrograms(filePath1);
            displayFilePrograms(fileContent);

            //selecting the programs that satisfy given condition 
            List<ProgramTV> condSatisf=fileContent.FindAll(x => x.StartTime.Hour >= 9 && x.EndTime.Hour <= 18 );
            //writing selected programs to the second file
            WriteIEnumerableToFile<ProgramTV>(filePath2, condSatisf);
            //displaying the selected programs
            displayFilePrograms(GetFilePrograms(filePath2));

        }
    }
}
