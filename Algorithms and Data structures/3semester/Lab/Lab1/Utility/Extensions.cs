using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Utility
{
    static class Extensions
    {
        public static void Swap<T>(
            this IList<T> list,
            int firstIndex,
            int secondIndex
        )
        {
            Contract.Requires(list != null);
            Contract.Requires(firstIndex >= 0 && firstIndex < list.Count);
            Contract.Requires(secondIndex >= 0 && secondIndex < list.Count);
            if (firstIndex == secondIndex)
            {
                return;
            }
            T temp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = temp;
        }

        //public static ulong Sum(this ulong[] ulongs)
        //{
        //    ulong sum=0;
        //    for (int i = 0; i < ulongs.Length; i++)
        //    {
        //        if (ulong.MaxValue - sum < ulongs[i]) throw new Exception();
        //        sum += ulongs[i];
        //    }
        //    return sum;
        //}
    }
}
