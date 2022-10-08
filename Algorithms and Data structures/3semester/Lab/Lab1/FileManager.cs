using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal abstract class FileManager
    {
        public Random random = new Random();

        public abstract void ReassignToEmptyFile(string fileName);
        public abstract ulong WriteRandFromRangeToFile(ulong inputSizeInBytes, ulong minGeneratableValue=0, ulong maxGeneratableValue=ulong.MaxValue);
        public abstract ulong[] ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex=null);
        public ulong LongRandom(ulong min, ulong max)
        {
            byte[] buf = new byte[ProgramConfig.numberSizeInBytes];
            random.NextBytes(buf);
            ulong longRand = BitConverter.ToUInt64(buf, 0);
            longRand >>= random.Next(0, ProgramConfig.numberSizeInBits);

            return ((longRand % (max - min)) + min);
        }
        //public static ulong[] readSeriaFromFile(string fileName, string fileTyp);
    }
}
