using Lab1.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class BinaryHeap
    {
        public ulong?[] heap;
        public double heightOfHeap;
        public int leafsSlotsStartIndex;
        public BinaryHeap() { }

        public BinaryHeap(int inputsAmount)
        {
            GenerateHeap(inputsAmount);
        }

        public ulong?[] GenerateHeap(int inputsAmount)
        {
            heightOfHeap = Math.Ceiling(Math.Log2(inputsAmount));
            //building heap
            double lengthOfHeap = 0;
            for (byte i = 0; i <= heightOfHeap; i++)
            {
                lengthOfHeap += Math.Pow(2, i);
            }
            leafsSlotsStartIndex = (int)(lengthOfHeap - Math.Pow(2, heightOfHeap));

            heap = new ulong?[(ulong)lengthOfHeap];
            //filling heap with empty entries
            FlushHeap();
            return heap;
        }
        public void FlushHeap()
        {
            for (int i = 0; i < heap.Length; i++)
            {
                heap[i] = null;
            }
        }

        public void HeapifyRange(int startIndex, int amount)//end index is excluded
        {
            if (heap.Length == 1) return;

            if (amount % 2 != 0) throw new ArgumentException();
            for (int i = 0; i < amount / 2; i++)
            {
                HeapifyIndex(startIndex + i * 2);
            }

        }

        public void HeapifyIndex(int startIndex)
        {

            int startCompIndReg = startIndex - 1 + startIndex % 2;
            if (startCompIndReg > 0 && startCompIndReg < heap.Length)
            {
                ulong? min;
                if (heap[startCompIndReg] == null && heap[startCompIndReg + 1] == null)
                    min = null;
                else if (heap[startCompIndReg] == null && heap[startCompIndReg + 1] != null)
                    min = heap[startCompIndReg + 1];
                else if (heap[startCompIndReg + 1] == null && heap[startCompIndReg] != null)
                    min = heap[startCompIndReg];
                else
                    min = (heap[startCompIndReg] > heap[startCompIndReg + 1]) ? heap[startCompIndReg + 1] : heap[startCompIndReg];

                heap[startCompIndReg / 2] = min;
                HeapifyIndex(startCompIndReg / 2);
            }
        }

        //public void ReassignRemovedLeaf(ulong? leafValue, ref FileConfig[] inputFiles)
        //{
        //    ulong[] replacementNumber;
        //    for (int i = leafsSlotsStartIndex; i < leafsSlotsStartIndex+inputFiles.Length; i++)
        //    {
        //        if (heap[i] == leafValue)
        //        {
        //            replacementNumber = inputFiles[i - leafsSlotsStartIndex].fileManager.ReadFromFile(ProgramConfig.numberSizeInBytes);
        //            heap[i] = replacementNumber != null ? replacementNumber[0] : null;
        //            i = leafsSlotsStartIndex + inputFiles.Length;
        //        }
        //    }
        //}
    }
}
