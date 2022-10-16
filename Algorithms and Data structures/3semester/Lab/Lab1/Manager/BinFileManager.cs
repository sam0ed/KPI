using Lab1.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Manager
{
    internal class BinFileManager : FileManager
    {
        public string fileName;
        private BinaryReader? binReader;
        private BinaryWriter? binWriter;
        public BinFileManager(string fileName)
        {
            ReassignToEmptyFile(fileName);
        }
        public override void ReassignToEmptyFile(string fileName)
        {
            this.fileName = fileName;
            OpenWriter(FileMode.Create);
        }

        public override ulong[]? ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex = null)
        {

            OpenReader(FileMode.Open);

            ulong arrayLength = requestedSizeInBytes / (ulong)ProgramConfig.numberSizeInBytes;
            ulong[] resultArr = new ulong[arrayLength];
            for (ulong i = 0; i < arrayLength; i++)
            {
                try
                {
                    resultArr[i] = binReader.ReadUInt64();
                }
                catch (Exception)
                {
                    i = arrayLength;
                }
            }

            if (resultArr.Length == 0)
                return null;
            else
                return resultArr;
        }

        public override void WriteRandFromRangeToFile(ulong inputSizeInBytes, ulong minGeneratableValue = 0, ulong maxGeneratableValue = ulong.MaxValue)
        {
            if (inputSizeInBytes < 0) throw new ArgumentOutOfRangeException();
            ulong numberAmount = (ulong)Math.Ceiling((double)(inputSizeInBytes / (double)ProgramConfig.numberSizeInBytes));

            ulong[] randomUlongArr = new ulong[numberAmount];
            for (ulong i = 0; i < numberAmount; i++)
            {
                randomUlongArr[i] = UlongRandom(minGeneratableValue, maxGeneratableValue);
            }
            WriteToFile(randomUlongArr);

        }

        public override void WriteToFile(ulong[] inputData, FileMode fileMode = FileMode.Append)
        {
            OpenWriter(fileMode);

            for (int i = 0; i < inputData.Length; i++)
            {
                binWriter.Write(inputData[i]);

            }
            //binWriter.Write(inputData.SelectMany(i => BitConverter.GetBytes(i)).ToArray());
        }

        public override void OpenReader(FileMode fileMode)
        {
            try
            {
                if (binReader == null)
                    binReader = new BinaryReader(File.Open(fileName, fileMode));
            }
            catch (IOException)
            {
                binWriter?.Close();
                binWriter = null;
                binReader = new BinaryReader(File.Open(fileName, fileMode));
            }
        }

        public override void OpenWriter(FileMode fileMode)
        {
            try
            {
                if (binWriter == null)
                    binWriter = new BinaryWriter(File.Open(fileName, fileMode));
            }
            catch (IOException)
            {
                binReader?.Close();
                binReader = null;
                binWriter = new BinaryWriter(File.Open(fileName, fileMode));
            }
        }
    }
}
