using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Config;
using Lab1.Utility;

namespace Lab1
{
    internal static class PolyPhaseSort
    {
        public static FileConfig Sort(FileConfig[] sortFiles, int filesAmount)
        {
            BinaryHeap heap = new BinaryHeap();
            heap.GenerateHeap(sortFiles.Length - 1);
            int emptyFileIndex = -1;
            FileConfig? emptyFile = null;
            int nonEmptyFileIndex;
            FileConfig[] nonEmptyFiles = new FileConfig[sortFiles.Length - 1];
            while (sortFiles.Select(x => x.runsAmount).Sum() != 1)
            {
                nonEmptyFileIndex = 0;
                for (int i = 0; i < sortFiles.Length; i++)/////////////////////////
                {
                    if (sortFiles[i].runsAmount == 0)
                    {
                        emptyFileIndex = i;
                        emptyFile = sortFiles[i];
                    }
                    else
                    {
                        nonEmptyFiles[nonEmptyFileIndex] = sortFiles[i];
                        nonEmptyFileIndex++;
                    }
                }
                long amountOfRunsToMergeInCurIter = nonEmptyFiles.Select(x => x.runsAmount).Min();
                emptyFile?.fileManager.ReassignToEmptyFile(sortFiles[emptyFileIndex].fileName);
                emptyFile.dataSizeInBytes = 0;
                emptyFile.runsAmount = 0;

                ulong[] amountOfNumbersInRunsRemain;
                for (int i = 0; i < amountOfRunsToMergeInCurIter; i++)
                {
                    heap.FlushHeap();
                    amountOfNumbersInRunsRemain = new ulong[nonEmptyFiles.Length];
                    for (int j = 0; j < nonEmptyFiles.Length; j++)
                    {
                        amountOfNumbersInRunsRemain[j] = nonEmptyFiles[j].dataSizeInBytes / (ulong)nonEmptyFiles[j].runsAmount / ProgramConfig.numberSizeInBytes;
                    }
                    //initiall filling up of leaves
                    for (int j = 0; j < nonEmptyFiles.Length; j++)
                    {
                        ReassignRemovedLeaf(null, ref nonEmptyFiles, ref heap, ref amountOfNumbersInRunsRemain);

                    }
                    ulong readNumberCashed = 0;
                    ulong readNumber;
                    while ( heap.heap[0]!=null)
                    {
                        readNumber = (ulong)heap.heap[0]!;

                        if (readNumberCashed > readNumber) throw new Exception();
                        else readNumberCashed = readNumber;

                        emptyFile.fileManager.WriteToFile(new ulong[] { readNumber });
                        emptyFile.dataSizeInBytes += ProgramConfig.numberSizeInBytes;

                        ReassignRemovedLeaf(readNumber, ref nonEmptyFiles, ref heap, ref amountOfNumbersInRunsRemain);
                    }
                    emptyFile.runsAmount++;
                    for (int j = 0; j < nonEmptyFiles.Length; j++)
                    {
                        nonEmptyFiles[j].runsAmount--;
                    }
                }
            }
            sortFiles[emptyFileIndex].fileManager.OpenReader(FileMode.Open);
            return sortFiles[emptyFileIndex];




            //int emptyFileIndex = -1;
            //while (sortFiles.Select(x => x.runsAmount).Sum() != 1)
            //{
            //    FileConfig emptyFile = sortFiles.Where(x => x.runsAmount == 0).First();

            //    for (int i = 0; i < sortFiles.Length; i++)/////////////////////////
            //    {
            //        if (sortFiles[i] == emptyFile) emptyFileIndex = i;
            //    }
            //    long amountOfRunsToMergeInCurIter = sortFiles.Where(x => x != emptyFile).Select(x => x.runsAmount).Min() /*?? throw new Exception()*/;
            //    sortFiles[emptyFileIndex].fileManager.ReassignToEmptyFile(sortFiles[emptyFileIndex].fileName);


            //    for (long i = 0; i < amountOfRunsToMergeInCurIter; i++)
            //    {
            //        ulong[][] nextRunsToBeMerged = new ulong[sortFiles.Length - 1][];
            //        int nextRunsToBeMergedIndex = 0;
            //        for (int j = 0; j < sortFiles.Length; j++)
            //        {
            //            if (j != emptyFileIndex)
            //            {
            //                ulong runSizeInBytes = sortFiles[j].dataSizeInBytes / (ulong)sortFiles[j].runsAmount;
            //                nextRunsToBeMerged[nextRunsToBeMergedIndex++] = sortFiles[j].fileManager.ReadFromFile(runSizeInBytes);
            //                sortFiles[j].runsAmount--;
            //                sortFiles[j].dataSizeInBytes -= runSizeInBytes;
            //            }
            //        }
            //        ulong[] mergedSeria = QuickSortMerge(nextRunsToBeMerged);
            //        sortFiles[emptyFileIndex].fileManager.WriteToFile(mergedSeria);
            //        sortFiles[emptyFileIndex].runsAmount++;
            //        sortFiles[emptyFileIndex].dataSizeInBytes += (ulong)mergedSeria.Length * ProgramConfig.numberSizeInBytes;

            //    }
            //}
            //sortFiles[emptyFileIndex].fileManager.OpenReader(FileMode.Open);
            //return sortFiles[emptyFileIndex];
        }

        public static ulong[] QuickSortMerge(ulong[][] seiesToMerge)
        {
            ulong[] result = seiesToMerge.SelectMany(x => x).ToArray();
            Array.Sort(result);
            return result;
        }

        public static void ReassignRemovedLeaf(ulong? leafValueToReplace, ref FileConfig[] inputFiles, ref BinaryHeap heap, ref ulong[] numbersInRunRemain/*, ref ulong mergedRunLength*/) // the shittiest method ive seen so far
        {
            ulong[]? replacementNumber;
            for (int i = heap.leafsSlotsStartIndex; i < heap.leafsSlotsStartIndex + inputFiles.Length; i++)
            {
                if (heap.heap[i] == leafValueToReplace)
                {
                    if (numbersInRunRemain[i - heap.leafsSlotsStartIndex] > 0)
                    {
                        replacementNumber = inputFiles[i - heap.leafsSlotsStartIndex].fileManager.ReadFromFile(ProgramConfig.numberSizeInBytes);
                        --numbersInRunRemain[i - heap.leafsSlotsStartIndex];
                        inputFiles[i - heap.leafsSlotsStartIndex].dataSizeInBytes -= ProgramConfig.numberSizeInBytes;
                        //--mergedRunLength;
                    }
                    else
                    {
                        replacementNumber=null;
                    }
                    heap.heap[i] = replacementNumber != null ? replacementNumber[0] : null;
                    heap.HeapifyIndex(i);
                    i = heap.leafsSlotsStartIndex + inputFiles.Length;
                }
            }
        }
        //public static ulong[] MultiWayMerge(FileConfig[] seriesToMerge)
        //{
        //    ulong[][] originalTrees=
        //}

        //public static (ulong[][],int[]) GenerateHeaps(ref FileConfig[] seriesToMerge)//parameter should not include empty file
        //{
        //    int totalLeafsAmount = seriesToMerge.Length;
        //    int heapsAmount = 0;
        //    while (totalLeafsAmount > 0)
        //    {
        //        totalLeafsAmount -= (int)Math.Pow(2, Math.Floor(Math.Log2(totalLeafsAmount)));
        //        heapsAmount++;
        //    }

        //    int[] heapsLeafsAmount = new int[heapsAmount];
        //    ulong[][] heaps = new ulong[heapsAmount][];
        //    totalLeafsAmount = seriesToMerge.Length;
        //    ulong numSizeInBytes = (ulong)ProgramConfig.numberSizeInBytes;
        //    for (int i = 0; i < heapsAmount; i++)
        //    {
        //        heapsLeafsAmount[i] = (int)Math.Pow(2, Math.Floor(Math.Log2(totalLeafsAmount)));
        //        heaps[i] = GenerateHeap(heapsLeafsAmount[i]);
        //        int leavesLotsStartAtIndex = heaps[i].Length - heapsLeafsAmount[i];
        //        for (int j = 0; j < heapsLeafsAmount[i]; j++)
        //        {
        //            heaps[i][leavesLotsStartAtIndex + j] = seriesToMerge[seriesToMerge.Length - totalLeafsAmount--].fileManager.ReadFromFile(numSizeInBytes)[0];
        //        }
        //        HeapifyRange(ref heaps[i], leavesLotsStartAtIndex, heapsLeafsAmount[i]);
        //        //totalLeafsAmount -= heapsLeafsAmount[i];
        //    }
        //    return (heaps, heapsLeafsAmount);
        //}

        //private static ulong[] GetLeafs()
        //    private static ulong[] GenerateHeap(int leafsAmount)
        //    {
        //        double heightOfTree = Math.Log2(leafsAmount);
        //        //building heap
        //        double lengthOfHeap = 0;
        //        for (byte i = 0; i <= heightOfTree; i++)
        //        {
        //            lengthOfHeap += Math.Pow(2, i);
        //        }

        //        ulong[] heap = new ulong[(ulong)lengthOfHeap];
        //        //filling heap with empty entries
        //        for (int i = 0; i < lengthOfHeap; i++)
        //        {
        //            heap[i] = ulong.MaxValue;
        //        }
        //        return heap;
        //    }

        //    public static void HeapifyRange(ref ulong[] heap, int startIndex, int amount)//end index is excluded
        //    {
        //        if (heap.Length == 1) return;

        //        if (amount % 2 != 0) throw new ArgumentException();
        //        for (int i = 0; i < amount / 2; i++)
        //        {
        //            HeapifyIndex(ref heap, startIndex + i * 2);
        //        }

        //    }
        //    public static void HeapifyIndex(ref ulong[] heap, int startIndex)
        //    {

        //        int startCompIndReg = startIndex - 1 + startIndex % 2;
        //        if (startCompIndReg > 0 && startCompIndReg < heap.Length)
        //        {
        //            ulong min = heap[startCompIndReg] > heap[startCompIndReg + 1] ? heap[startCompIndReg + 1] : heap[startCompIndReg];
        //            heap[startCompIndReg / 2] = min;
        //            HeapifyIndex(ref heap, startCompIndReg / 2);
        //        }
        //    }

        //}
    }
}

//int runsAmount = seriesToMerge.Length;
//int leafsAmount = runsAmount % 2;
//double heightOfTree = Math.Log2(leafsAmount);
////building heap
//double lengthOfHeap = 0;
//for (byte i = 0; i <= heightOfTree; i++)
//{
//    lengthOfHeap += Math.Pow(2, i);
//}
//int leavesLotsStartAtIndex = (int)lengthOfHeap - leafsAmount;
//ulong[] heap = new ulong[(ulong)lengthOfHeap];
////filling heap with empty entries
//for (int i = 0; i < lengthOfHeap; i++)
//{
//    heap[i] = ulong.MaxValue;
//}
////filling heap with leaves
//ulong numSizeInBytes = (ulong)ProgramConfig.numberSizeInBytes;
//for (int i = 0; i < leafsAmount; i++)
//{
//    heap[leavesLotsStartAtIndex + i] = seriesToMerge[i].fileManager.ReadFromFile(numSizeInBytes)[0];
//}
////heapifying heap
//HeapifyRange(ref heap, leavesLotsStartAtIndex, leafsAmount);














