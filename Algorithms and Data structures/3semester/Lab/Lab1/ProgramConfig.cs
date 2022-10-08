using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal static class ProgramConfig
    {
        public static int numberSizeInBytes = sizeof(ulong);
        public static int numberSizeInBits = numberSizeInBytes * 8;
        public static string inputFileNamePattern= "Input";
        private static string filesNamePattern = "SortingFile";

    }
}
