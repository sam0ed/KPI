﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab1.Config;
using Lab1.Utility;

namespace Lab1.Manager
{
    internal abstract class FileManager
    {
        public string fileName;
        public Random random = new Random();

        public abstract void ReassignToEmptyFile(string fileName);
        public abstract void WriteRandFromRangeToFile(ulong inputSizeInBytes, ulong minGeneratableValue = 0, ulong maxGeneratableValue = ulong.MaxValue);
        public abstract void WriteToFile(ulong[] inputData, FileMode fileMode = FileMode.Append);
        public abstract ulong[] ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex = null);
        public abstract void OpenReader(FileMode fileMode);
        public abstract void OpenWriter(FileMode fileMode);

        public ulong UlongRandom(ulong min, ulong max)
        {
            byte[] buf = new byte[ProgramConfig.numberSizeInBytes];
            random.NextBytes(buf);
            ulong longRand = BitConverter.ToUInt64(buf, 0);
            longRand >>= random.Next(0, (int)(ProgramConfig.numberSizeInBits*0.9));


            return longRand % (max - min) + min;
        }


        public void PrintPart(ulong requestedSizeInBytes)
        {
            ulong chunkInBytes = Converter.StringToBytes("5gb");
            ulong evenPart = requestedSizeInBytes / chunkInBytes;
            ulong remainder = requestedSizeInBytes % chunkInBytes;
            for (ulong i = 0; i < evenPart; i++)
            {
                ulong[] evenChunk = ReadFromFile(chunkInBytes);
                for (int j = 0; j < evenChunk.Length; j++)
                {
                    Console.Write(evenChunk[j]+" ");
                }
            }
            ulong[] remainderChunk = ReadFromFile(remainder);
            for (int i = 0; i < remainderChunk.Length; i++)
            {
                Console.Write(remainderChunk[i]+" ");
            }
        }
        //public static ulong[] readSeriaFromFile(string fileName, string fileTyp);
    }
}