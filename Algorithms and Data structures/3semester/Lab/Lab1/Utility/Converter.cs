using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab1.Config;

namespace Lab1.Utility
{
    static class Converter
    {
        public static ulong StringToBytes(string unconvertedSize)
        {
            string actualSizeArray = string.Concat(unconvertedSize.ToCharArray().Where(x => x > 47 && x < 58));
            if (string.IsNullOrEmpty(actualSizeArray)) throw new Exception("Input format incorrect");

            int unitSize = int.Parse(actualSizeArray);
            string unitMeasure = string.Concat(unconvertedSize.Except(actualSizeArray));

            ulong sizeInBytes = unitMeasure.ToLower() switch
            {
                "b" => (ulong)unitSize,
                "kb" => (ulong)(1024 * unitSize),
                "mb" => (ulong)(1024 * 1024 * unitSize),
                "gb" => (ulong)(1024 * 1024 * 1024 * unitSize),
                _ => 0
            };
            return sizeInBytes;
        }
        //public static ulong[] BytesToUlongArr(byte[] source)
        //{
        //    ulong[] resultArr = source.Chunk(ProgramConfig.numberSizeInBytes).Select(s => BitConverter.ToUInt64(s.ToArray(), 0)).ToArray();
        //    return resultArr;
        //}
    }
}
