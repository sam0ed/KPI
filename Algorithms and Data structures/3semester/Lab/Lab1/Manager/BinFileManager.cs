using Lab1.Config;
using Lab1.Config.FileConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Manager
{
    internal class BinFileManager : FileManager
    {
        private BinaryReader? binReader;
        private BinaryWriter? binWriter;
        public BinFileManager(FileConfig fileConfig, Action<FileConfig, ulong> doOnWriting, Action<FileConfig, ulong> doOnReading) : base(fileConfig, doOnWriting, doOnReading) { }

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
            changeFileConfigAfterReading.Invoke(fileConfig, (ulong)resultArr.Length * ProgramConfig.numberSizeInBytes);
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
            changeFileConfigAfterWriting.Invoke(fileConfig, (ulong)inputData.Length*ProgramConfig.numberSizeInBytes);
        }

        public override void OpenReader(FileMode fileMode)
        {
            if (binReader == null)
            {
                if (binWriter != null)
                {
                    binWriter?.Close();
                    binWriter = null;
                }
                binReader = new BinaryReader(File.Open(fileConfig.fileName, fileMode));
            }

        }

        public override void OpenWriter(FileMode fileMode)
        {
            if (binWriter == null)
            {
                if (binReader != null)
                {
                    binReader?.Close();
                    binReader = null;
                }
                binWriter = new BinaryWriter(File.Open(fileConfig.fileName, fileMode));
            }
        }
    }
}
