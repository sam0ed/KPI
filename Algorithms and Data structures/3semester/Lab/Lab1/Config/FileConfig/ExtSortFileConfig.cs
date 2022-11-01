using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Manager;

namespace Lab1.Config.FileConfig
{
    internal class ExtSortFileConfig : FileConfig
    {

        public string fileType;
        public ulong dataSizeInBytes;
        public long runsAmount;// may cause error, idk
        public FileManager fileManager;

        public ExtSortFileConfig(string fileName)
        {
            this.fileName = fileName;
            fileType = fileName.Substring(fileName.LastIndexOf('.'));
            dataSizeInBytes = 0;

            if (fileType == ".bin")
                fileManager = new BinFileManager(this,
                    (FileConfig x, ulong size) =>
                    {
                        (x as ExtSortFileConfig)!.dataSizeInBytes += size;
                    },
                    (FileConfig x, ulong size) =>
                    {
                        (x as ExtSortFileConfig)!.dataSizeInBytes -= size;
                    }
                    );

            else if (fileType == ".txt")
                fileManager = new TxtFileManager(this,
                    (FileConfig x, ulong size) =>
                    {
                        (x as ExtSortFileConfig)!.dataSizeInBytes += size;
                    },
                    (FileConfig x, ulong size) =>
                    {
                        (x as ExtSortFileConfig)!.dataSizeInBytes -= size;
                    }
                    );
            else throw new ArgumentException("Can`t create FileManager of the given type");
            //fileManager = SelectFileManager(fileName);
        }

        public void ReassignToEmptyFile()
        {
            dataSizeInBytes = 0;
            runsAmount = 0;
            fileManager.ReassignToEmptyFile(this);
        }
        //public FileManager SelectFileManager(string fileName)
        //{

        //}
    }
}
