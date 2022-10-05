using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Splitter
    {
        private ulong runSizeInBytes;
        private long runsAmount;
        private ulong fileSizeInBytes;
        private readonly int filesAmount;

        private List<long> runsDistribution;
        private const int fileContainsResultIndex=1;
        private int mergeFileIndex = 0;

        public Splitter(ulong runSizeInBytes, ulong fileSizeInBytes, int filesAmount)
        {
            this.runSizeInBytes = runSizeInBytes;
            this.fileSizeInBytes = fileSizeInBytes;
            this.runsAmount = (long)Math.Ceiling((double)fileSizeInBytes / runSizeInBytes);
            this.filesAmount = filesAmount;
            Split();
        }

        //instead of adding dummy entries to file, therefore increasing its size we adjust runAmount to distribution sum that is less or equal then currenct runAmount.
        public long[] RunsDistribution()
        {
            List<long> distribution = new List<long>(filesAmount);
            for (int i = 0; i < filesAmount; i++)
            {
                distribution.Add( 0);
            }
            distribution[fileContainsResultIndex] = 1;

            
            int maxRunFileIndex;
            long maxRunFileAmount;
            long[] result=new long[filesAmount];
            while (distribution.Sum() < runsAmount)
            {
                maxRunFileAmount = distribution.Max();
                maxRunFileIndex = distribution.IndexOf(maxRunFileAmount);
                for (int i = 0; i < distribution.Count; i++)
                {
                    if(i!=maxRunFileIndex&&i!=mergeFileIndex)
                        distribution[i]+=maxRunFileAmount;
                }
                distribution.Swap(maxRunFileIndex, mergeFileIndex);
                mergeFileIndex = maxRunFileIndex;
                if(Math.Abs(runsAmount-distribution.Sum()) < Math.Abs(runsAmount-result.Sum()))
                    distribution.CopyTo(result);
            }

            return result;
        }

        public void Split()
        {
            runsDistribution = new List<long>(RunsDistribution());


            


        }

        public ulong FindAdditionalSize() //finds file size according to the runs distribution provided
        {
            long newRunsAmount = runsDistribution.Sum();
            double newRunSize = Convert.ToDouble(fileSizeInBytes) / Convert.ToDouble(newRunsAmount);
            ulong dummySizeInBytes;

            if (newRunsAmount < runsAmount)
            {
                dummySizeInBytes = (ulong)Math.Round((Generator.numberSizeInBytes - newRunSize % Generator.numberSizeInBytes) * newRunsAmount);
            }
            else if (newRunsAmount > runsAmount)
            {
                dummySizeInBytes = (ulong)Math.Round((runSizeInBytes - newRunSize) * newRunsAmount);
            }
            else dummySizeInBytes = 0;

            if (dummySizeInBytes < 0) throw new ArgumentOutOfRangeException();

            runsAmount = newRunsAmount;
            runSizeInBytes = (fileSizeInBytes + dummySizeInBytes) / (ulong)runsAmount;

            return dummySizeInBytes;

        }

    }
}
