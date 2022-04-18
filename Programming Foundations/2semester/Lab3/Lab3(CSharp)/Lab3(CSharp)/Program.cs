using System;
using System.Linq;
using System.Collections.Generic;

namespace Lab3_CSharp_
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialization of container with randomly filled arrays.
            IntArr[] arrayContainer = GetArrayContainerInit();
            for (int i = 0; i < arrayContainer.Length; i++)
            {
                Console.Write($"arr {i}: ");
                arrayContainer[i].PrintArr();
            }

            // getting the array Number-least maximum as a dictionaty
            Dictionary<int, int> arrayMaxValuePairs = new Dictionary<int, int>();
            for (int i = 0; i < arrayContainer.Length; i++)
            {
                arrayMaxValuePairs.Add(i, arrayContainer[i].GetMax());
            }
            //getting the least max value
            var chosenArr = arrayMaxValuePairs.Where(p => p.Value == arrayMaxValuePairs.Values.Min()).Select(p => p);
            foreach (var item in chosenArr)
            {
                Console.WriteLine($"Minimal value of maximal element in array was found in array {item.Key} and has value of {item.Value}:");
                arrayContainer[item.Key].PrintArr();
            }
        }

        public static IntArr[] GetArrayContainerInit()
        {
            Console.Write("Enter the size of container: ");
            IntArr[] arrayContainer = new IntArr[int.Parse(Console.ReadLine())];
            for (int i = 0; i < arrayContainer.Length; i++)
            {
                Console.Write($"Enter the size of array {i} in container: ");
                arrayContainer[i] = new IntArr(int.Parse(Console.ReadLine()));
                arrayContainer[i].FillArrayRandomly();
            }
            return arrayContainer;
        }
    }
}
