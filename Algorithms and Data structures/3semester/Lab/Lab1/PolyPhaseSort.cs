using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal static class PolyPhaseSort
    {
        public static FileConfig Sort(FileConfig sourceFile, int filesAmount, ulong runSizeInBytesRequested)
        {
            Splitter splitter = new Splitter(sourceFile);
            List<FileConfig> sortFiles = splitter.Split(filesAmount, runSizeInBytesRequested);
            int emptyFileIndex=-1;
            while (sortFiles.Select(x => x.runsAmount).Sum() != 1)
            {
                FileConfig emptyFile = sortFiles.Where(x => x.runsAmount == 0).First();
                emptyFileIndex = sortFiles.IndexOf(emptyFile);
                long amountOfRunsToMergeInCurIter = sortFiles.Where(x => x != emptyFile).Select(x => x.runsAmount).Min() ?? throw new Exception();
                sortFiles[emptyFileIndex].fileManager.ReassignToEmptyFile(sortFiles[emptyFileIndex].fileName);


                for (long i = 0; i < amountOfRunsToMergeInCurIter; i++)
                {
                    ulong[][] nextRunsToBeMerged = new ulong[sortFiles.Count-1][];
                    int nextRunsToBeMergedIndex=0;
                    for (int j = 0; j < sortFiles.Count; j++)
                    {
                        if (j != emptyFileIndex)
                        {
                            ulong runSizeInBytes = sortFiles[j].dataSizeInBytes / (ulong)sortFiles[j].runsAmount;
                            nextRunsToBeMerged[nextRunsToBeMergedIndex++] = sortFiles[j].fileManager.ReadFromFile(runSizeInBytes);
                            sortFiles[j].runsAmount--;
                            sortFiles[j].dataSizeInBytes-=runSizeInBytes;
                        }
                    }
                    ulong[] mergedSeria=QuickSortMerge(nextRunsToBeMerged);
                    sortFiles[emptyFileIndex].fileManager.WriteToFile(mergedSeria);
                    sortFiles[emptyFileIndex].runsAmount++;
                    sortFiles[emptyFileIndex].dataSizeInBytes += (ulong)(mergedSeria.Length * ProgramConfig.numberSizeInBytes);

                }
            }
            sortFiles[emptyFileIndex].fileManager.OpenReader(FileMode.Open);
            return sortFiles[emptyFileIndex];

        }

        public static ulong[] QuickSortMerge(ulong[][] seiesToMerge)
        {
            ulong[] result = seiesToMerge.SelectMany(x => x).ToArray();
            Array.Sort(result);
            return result;
        }
    }
}
