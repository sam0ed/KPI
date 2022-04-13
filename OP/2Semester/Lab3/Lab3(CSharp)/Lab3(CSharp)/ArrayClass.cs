using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_CSharp_
{
    class IntArr
    {
        private int [] MyArr { get; set; }
        public IntArr(int size=0)
        {
            MyArr = new int[size];
        }
        public void FillArrayRandomly()
        {
            Random rand = new Random();
            if (MyArr.Length>0)
            {
                for (int i = 0; i < MyArr.Length; i++)
                {
                    MyArr[i] = rand.Next(5000);
                } 
            }
        }
        public int this[int index]
        {
            get { return MyArr[index]; }
            set { MyArr[index] = value; }
        }
        public int GetMax() => MyArr.Max();
        public void PrintArr()
        {
            foreach (var item in MyArr)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }

    }
}
