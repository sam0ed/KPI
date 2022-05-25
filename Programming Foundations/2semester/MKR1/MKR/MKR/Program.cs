using System;
using System.Collections.Generic;

namespace MKR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your first number: ");
            Number myNumber1 = new Number(Console.ReadLine());
            myNumber1.printNum();
            Console.Write("Enter your second number: ");
            Number myNumber2 = new Number(Console.ReadLine());
            myNumber2.printNum();

            if(myNumber1>myNumber2)
            {
                myNumber1++;
            }
            else
            {
                myNumber2++;
            }

            Console.WriteLine("Number1: ");
            myNumber1.printNum();
            Console.WriteLine("Number2: ");
            myNumber2.printNum();
        }
        
    }
}