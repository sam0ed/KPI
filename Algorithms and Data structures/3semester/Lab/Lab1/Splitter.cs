using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Lab1.Config;
using Lab1.Config.FileConfig;
using Lab1.Utility;

namespace Lab1
{

    internal class Splitter
    {

        public ExtSortFileConfig sourceFile;

        private List<long> runsDistribution;
        private const int fileContainsResultIndex = 1;
        private int mergeFileIndex = 0;
        private const string filesNamePattern = "SortingFile";

        public Splitter(ExtSortFileConfig sourceFile)
        {
            ChangeSourceFile(sourceFile);
        }

        public void ChangeSourceFile(ExtSortFileConfig sourceFile)
        {
            this.sourceFile = sourceFile;
        }

        //instead of adding dummy entries to file, therefore increasing its size we adjust runAmount to distribution sum that is less or equal then currenct runAmount.
        public long[] FibonacciRunsDistribution(int filesAmount, long runsAmount)
        {
            List<long> distribution = new List<long>(filesAmount);
            for (int i = 0; i < filesAmount; i++)
            {
                distribution.Add(0);
            }
            distribution[fileContainsResultIndex] = 1;

            int maxRunFileIndex;
            long maxRunFileAmount;
            long[] result = new long[filesAmount];
            while (distribution.Sum() < runsAmount)
            {
                maxRunFileAmount = distribution.Max();
                maxRunFileIndex = distribution.IndexOf(maxRunFileAmount);
                for (int i = 0; i < distribution.Count; i++)
                {
                    if (i != maxRunFileIndex && i != mergeFileIndex)
                        distribution[i] += maxRunFileAmount;
                }
                distribution.Swap(maxRunFileIndex, mergeFileIndex);
                mergeFileIndex = maxRunFileIndex;
                if (Math.Abs(runsAmount - distribution.Sum()) < Math.Abs(runsAmount - result.Sum()))
                    distribution.CopyTo(result);
            }

            return result;
        }

        //the most fucked up function in whole project, signature needs refactoring desperately
        public ulong FindAdditionalSize(long runsAmount) //finds file size according to the runs distribution provided
        {
            long newRunsAmount = runsDistribution.Sum();
            double newRunSize = Convert.ToDouble(sourceFile.dataSizeInBytes) / Convert.ToDouble(newRunsAmount);
            ulong dummySizeInBytes;

            if (newRunsAmount < runsAmount)
            {
                dummySizeInBytes = (ulong)Math.Round((ProgramConfig.numberSizeInBytes - newRunSize % ProgramConfig.numberSizeInBytes) * newRunsAmount);
            }
            else if (newRunsAmount > runsAmount)
            {
                double add = runsAmount - (double)sourceFile.dataSizeInBytes % runsAmount;
                double fileSize= sourceFile.dataSizeInBytes+add;
                fileSize = fileSize + (ProgramConfig.numberSizeInBytes * (ulong)newRunsAmount - fileSize % (ProgramConfig.numberSizeInBytes * (ulong)newRunsAmount));
                while (fileSize / newRunsAmount < (double)sourceFile.dataSizeInBytes / runsAmount)
                {
                    fileSize += ProgramConfig.numberSizeInBytes * (ulong)newRunsAmount;
                }
                dummySizeInBytes = (ulong)fileSize - sourceFile.dataSizeInBytes;
                //dummySizeInBytes = (ulong)Math.Round(((double)sourceFile.dataSizeInBytes / runsAmount - newRunSize) * newRunsAmount);
            }
            else dummySizeInBytes = 0;

            if (dummySizeInBytes < 0) throw new ArgumentOutOfRangeException();
            return dummySizeInBytes;

        }

        public /*List<FileConfig>*/ ExtSortFileConfig[] Split(int filesAmount, ulong runSizeInBytesRequested)//dont forget that runs distribution occurs inside split
        {
            //sourceInfo.Length returns actual size of the file on disk. bin works fine.
            long runsAmountRequested = (long)Math.Ceiling((double)sourceFile.dataSizeInBytes / runSizeInBytesRequested);
            runsDistribution = new List<long>(FibonacciRunsDistribution(filesAmount, runsAmountRequested));
            ulong additionalSize = FindAdditionalSize(runsAmountRequested);
            sourceFile.fileManager.WriteRandFromRangeToFile(additionalSize, 0, 1);
            //sourceFile.dataSizeInBytes += additionalSize;
            sourceFile.runsAmount = runsDistribution.Sum();
            ulong newRunSizeInBytes = sourceFile.dataSizeInBytes / (ulong)sourceFile.runsAmount;

            //List<FileConfig> sortFiles = new List<FileConfig>(filesAmount);
            ExtSortFileConfig[] sortFiles = new ExtSortFileConfig[filesAmount];
            for (int i = 0; i < filesAmount; i++)
            {
                //sortFiles.Add( new FileConfig(ProgramConfig.filesNamePattern+i.ToString()+sourceFile.fileType));
                sortFiles[i] = new ExtSortFileConfig(ProgramConfig.filesNamePattern + i.ToString() + sourceFile.fileType);
                for (int j = 0; j < runsDistribution[i]; j++)
                {
                    ulong[] readArray = sourceFile.fileManager.ReadFromFile(newRunSizeInBytes);
                    Array.Sort(readArray);
                    sortFiles[i].fileManager.WriteToFile(readArray/*sourceFile.fileManager.ReadFromFile(newRunSizeInBytes)*/);
                }
                sortFiles[i].runsAmount = runsDistribution[i];
                //sortFiles[i].dataSizeInBytes = (ulong)sortFiles[i].runsAmount! * newRunSizeInBytes;
            }

            return sortFiles;
        }

    }
}
