using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Config
{
    internal static class ProgramConfig
    {
        public static ulong numberSizeInBytes = sizeof(ulong);
        public static int numberSizeInBits = (int)numberSizeInBytes * 8;
        public static string inputFileNamePattern = "Input";
        public static string filesNamePattern = "SortingFile";

    }
}
