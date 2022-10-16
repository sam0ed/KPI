using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Manager;

namespace Lab1.Config
{
    internal class FileConfig
    {
        public string fileName;
        public string fileType;
        public ulong dataSizeInBytes;
        public long runsAmount;// may cause error, idk
        public FileManager fileManager;

        public FileConfig(string fileName)
        {
            this.fileName = fileName;
            fileType = fileName.Substring(fileName.LastIndexOf('.'));
            fileManager = SelectFileManager(fileName);
            dataSizeInBytes = 0;
            /*runsAmount = null;*///questionable choice of default value here////////////////may cause problems here

            //fileManager.ReassignToEmptyFile(fileName);

        }

        public FileManager SelectFileManager(string fileName)
        {
            if (fileType == ".bin") return new BinFileManager(fileName);
            else if (fileType == ".txt") return new TxtFileManager(fileName);
            else throw new ArgumentException("Can`t create FileManager of the given type");
        }
    }
}
