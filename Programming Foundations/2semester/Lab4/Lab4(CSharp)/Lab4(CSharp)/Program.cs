using System;
using System.Collections.Generic;

namespace Lab4_CSharp_
{
    class Program
    {
        static void Main(string[] args)
        {
            Triangle T1 = new Triangle(new Point(3.0, 4.0), new Point(4.0, 8.0), new Point(6.0, 4.0));
            T1.PrintTriangle("firstTr");
            T1++;
            T1.PrintTriangle("firstTr after increment");
            Console.WriteLine("-------------------------------------------");

            Triangle T2 = new Triangle(new Line(new Point(9.0, 4.0), new Point(3.0, 6.0)),
                new Line(new Point(3.0, 6.0), new Point(6.0, 4.0)),
                new Line(new Point(9.0, 4.0), new Point(6.0, 4.0)));
            T2.PrintTriangle("secondTr");
            T2--;
            T2.PrintTriangle("secondTr after dencrement");
            Console.WriteLine("-------------------------------------------");

            Console.Write("Enter the value(double) by which you want to increase T3: ");
            double value = double.Parse(Console.ReadLine());
            Triangle T3 = new Triangle(new Line(new Point(3.0, 4.0), new Point(3.0, 8.0)),
                new Line(new Point(3.0, 8.0), new Point(6.0, 4.0)));
            T3.PrintTriangle("thirdTr");
            T3 += value;
            T3.PrintTriangle("thirdTr after being increased by value");
            Console.WriteLine("-------------------------------------------");

            //Calculating the triangle with largest area
            List<Triangle> areasTr = new List<Triangle>() { T1, T2, T3 };
            (int Number, double Area) largestTriangle = (-1, -1);
            for (int i = 0; i < areasTr.Count; i++)
            {
                if (areasTr[i].Area > largestTriangle.Area)
                    largestTriangle = (i, areasTr[i].Area);
            }
            Console.WriteLine($"T{largestTriangle.Number+1} has the largest area of {Math.Round(largestTriangle.Area, 3)}.");

        }
    }
}
