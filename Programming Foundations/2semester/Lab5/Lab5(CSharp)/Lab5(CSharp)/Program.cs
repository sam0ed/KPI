using System;
using System.Collections.Generic;

namespace Lab5_CSharp_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TIntNumber[] binary = ConsoleReadFabricMethod(typeof(TIntNumber2));
            TIntNumber[] hexodecimal = ConsoleReadFabricMethod(typeof(TIntNumber16));

            binary.ToList().ForEach(x => x++);
            hexodecimal.ToList().ForEach(x => x--);
            Console.WriteLine("Number arrays after element modification: ");
            Print(binary);
            Print(hexodecimal);

            var allNumbers = binary.Select(x => x.ConvertToDecimal()).ToList();
            allNumbers.AddRange(hexodecimal.Select(x => x.ConvertToDecimal()).ToList());
            int indMax = allNumbers.IndexOf(allNumbers.Max());

            if (indMax < binary.Length)
            {
                Console.Write($"The largest element was found in binary array with index {indMax} : ");
                binary[indMax].Print();
            }
            else
            {
                Console.Write($"The largest element was found in hexodecimal array with index {indMax} : ");
                hexodecimal[indMax-binary.Length].Print();
            }

        }

        static void Print(TIntNumber[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Print();
            }
            Console.Write("\n");
        }
        static TIntNumber[] ConsoleReadFabricMethod(Type readableType)
        {
            Console.WriteLine($"Enter {readableType.ToString()} : ");
            string[] m = Console.ReadLine().Split(' ');
            TIntNumber[] result = new TIntNumber[m.Length];
            if (readableType == typeof(TIntNumber2))
            {
                for (int i = 0; i < m.Length; i++)
                {
                    result[i] = new TIntNumber2(m[i]);
                }
            }
            else if (readableType == typeof(TIntNumber16))
            {
                for (int i = 0; i < m.Length; i++)
                {
                    result[i] = new TIntNumber16(m[i]);
                }
            }
            else
            {
                throw new Exception("Wrong type was passed for reading");
            }
            return result;
        }
    }
}