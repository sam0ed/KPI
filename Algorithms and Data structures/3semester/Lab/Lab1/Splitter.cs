using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    //internal class Splitter
    //{
    //    private ulong runSizeInBytesRequested;
    //    private long runsAmount;
    //    public ulong fileSizeInBytes;
    //    private readonly int filesAmount;

    //    private List<long> runsDistribution;
    //    private const int fileContainsResultIndex = 1;
    //    private int mergeFileIndex = 0;
    //    private const string filesNamePattern = "SortingFile";

    //    public Splitter(ulong runSizeInBytesRequested, ulong fileSizeInBytes, int filesAmount)
    //    {
    //        this.runSizeInBytesRequested = runSizeInBytesRequested;
    //        this.fileSizeInBytes = fileSizeInBytes;
    //        this.runsAmount = (long)Math.Ceiling((double)fileSizeInBytes / runSizeInBytesRequested);
    //        this.filesAmount = filesAmount;
    //        this.runsDistribution = new List<long>(FibonacciRunsDistribution());
    //        //runs distribution is inside split
    //    }

    //    //instead of adding dummy entries to file, therefore increasing its size we adjust runAmount to distribution sum that is less or equal then currenct runAmount.
    //    public long[] FibonacciRunsDistribution()
    //    {
    //        List<long> distribution = new List<long>(filesAmount);
    //        for (int i = 0; i < filesAmount; i++)
    //        {
    //            distribution.Add(0);
    //        }
    //        distribution[fileContainsResultIndex] = 1;


    //        int maxRunFileIndex;
    //        long maxRunFileAmount;
    //        long[] result = new long[filesAmount];
    //        while (distribution.Sum() < runsAmount)
    //        {
    //            maxRunFileAmount = distribution.Max();
    //            maxRunFileIndex = distribution.IndexOf(maxRunFileAmount);
    //            for (int i = 0; i < distribution.Count; i++)
    //            {
    //                if (i != maxRunFileIndex && i != mergeFileIndex)
    //                    distribution[i] += maxRunFileAmount;
    //            }
    //            distribution.Swap(maxRunFileIndex, mergeFileIndex);
    //            mergeFileIndex = maxRunFileIndex;
    //            if (Math.Abs(runsAmount - distribution.Sum()) < Math.Abs(runsAmount - result.Sum()))
    //                distribution.CopyTo(result);
    //        }

    //        return result;
    //    }

    //    public ulong FindAdditionalSize() //finds file size according to the runs distribution provided
    //    {

    //        long newRunsAmount = runsDistribution.Sum();
    //        double newRunSize = Convert.ToDouble(fileSizeInBytes) / Convert.ToDouble(newRunsAmount);
    //        ulong dummySizeInBytes;

    //        if (newRunsAmount < runsAmount)
    //        {
    //            dummySizeInBytes = (ulong)Math.Round((Config.numberSizeInBytes - newRunSize % Config.numberSizeInBytes) * newRunsAmount);
    //        }
    //        else if (newRunsAmount > runsAmount)
    //        {
    //            dummySizeInBytes = (ulong)Math.Round((runSizeInBytesRequested - newRunSize) * newRunsAmount);
    //        }
    //        else dummySizeInBytes = 0;

    //        if (dummySizeInBytes < 0) throw new ArgumentOutOfRangeException();

    //        runsAmount = newRunsAmount;
    //        runSizeInBytesRequested = (fileSizeInBytes + dummySizeInBytes) / (ulong)runsAmount;


    //        return dummySizeInBytes;

    //    }

    //    public void Split(string sourceFileType)//dont forget that runs distribution occurs inside split
    //    {
    //        if (sourceFileType == ".txt")
    //        {
    //            StreamReader source = new StreamReader(File.OpenRead(Config.inputFileName));

    //            List<StreamWriter> sortFiles = new List<StreamWriter>(filesAmount);
    //            for (int i = 0; i < sortFiles.Count; i++)
    //            {
    //                sortFiles[i] = new StreamWriter(File.Open(filesNamePattern + i.ToString(), FileMode.Create));
    //                for (int j = 0; j < runsDistribution[i]; j++)
    //                {
    //                    for (ulong k = 0; k < runSizeInBytesRequested / (ulong)Config.numberSizeInBytes; k++)
    //                    {
    //                        sortFiles[i].WriteLine(source.ReadLine());
    //                    }
    //                }
    //            }
    //        }
    //        else if (sourceFileType == ".bin")
    //        {
    //            BinaryReader source = new BinaryReader(File.OpenRead(Config.inputFileName));
    //        }



    //    }

    //}

    internal class Splitter
    {

        public FileConfig sourceFile;

        private List<long> runsDistribution;
        private const int fileContainsResultIndex = 1;
        private int mergeFileIndex = 0;
        private const string filesNamePattern = "SortingFile";

        public Splitter(FileConfig sourceFile)
        {
            ChangeSourceFile(sourceFile);
        }

        public void ChangeSourceFile(FileConfig sourceFile)
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
                dummySizeInBytes = (ulong)Math.Round(((double)sourceFile.dataSizeInBytes / runsAmount - newRunSize) * newRunsAmount);
            }
            else dummySizeInBytes = 0;

            if (dummySizeInBytes < 0) throw new ArgumentOutOfRangeException();
            return dummySizeInBytes;

        }

        public void Split(int filesAmount, ulong runSizeInBytesRequested)//dont forget that runs distribution occurs inside split
        {
            //sourceInfo.Length returns actual size of the file on disk. bin works fine.
            long runsAmountRequested = (long)Math.Ceiling((double)sourceFile.dataSizeInBytes / runSizeInBytesRequested);
            runsDistribution = new List<long>(FibonacciRunsDistribution(filesAmount, runsAmountRequested));
            ulong additionalSize = FindAdditionalSize(runsAmountRequested);
            sourceFile.fileManager.WriteRandFromRangeToFile(additionalSize, 0, 1);
            sourceFile.dataSizeInBytes += additionalSize;
            sourceFile.runsAmount = runsDistribution.Sum();
            ulong newRunSizeInBytes = sourceFile.dataSizeInBytes / (ulong)sourceFile.runsAmount;

            List<FileConfig> sortFiles = new List<FileConfig>(filesAmount);
            for (int i = 0; i < filesAmount; i++)
            {
                sortFiles.Add( new FileConfig(ProgramConfig.filesNamePattern+i.ToString()+sourceFile.fileType));
                for (int j = 0; j < runsDistribution[i]; j++)
                {
                    sortFiles[i].fileManager.WriteToFile(sourceFile.fileManager.ReadFromFile(newRunSizeInBytes));
                }
                sortFiles[i].runsAmount = runsDistribution[i];
                sortFiles[i].dataSizeInBytes = (ulong)sortFiles[i].runsAmount! * newRunSizeInBytes;
            }

        }

    }
}
