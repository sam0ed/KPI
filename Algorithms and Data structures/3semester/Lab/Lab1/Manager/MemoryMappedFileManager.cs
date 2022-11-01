using Lab1.Config.FileConfig;
using Lab1.Utility;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Manager
{
    internal class MemoryMappedFileManager : FileManager
    {
        public string fileName;
        private MemoryMappedFile fileManager;
        public MemoryMappedFileManager(FileConfig fileConfig, Action<FileConfig, ulong> doOnWriting, Action<FileConfig, ulong> doOnReading) : base(fileConfig, doOnWriting, doOnReading) { }

        public override void OpenReader(FileMode fileMode)
        {
            throw new NotImplementedException();
        }

        public override void OpenWriter(FileMode fileMode)
        {
            throw new NotImplementedException();
        }

        public override ulong[] ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex = null)
        {
            throw new NotImplementedException();
        }

        public override void WriteRandFromRangeToFile(ulong inputSizeInBytes, ulong minGeneratableValue = 0, ulong maxGeneratableValue = ulong.MaxValue)
        {
            throw new NotImplementedException();
        }

        public override void WriteToFile(ulong[] inputData, FileMode fileMode = FileMode.Append)
        {
            throw new NotImplementedException();
        }
    }
}
