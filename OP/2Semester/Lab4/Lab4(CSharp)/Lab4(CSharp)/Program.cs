using System;

namespace Lab4_CSharp_
{
    class Program
    {
        static void Main(string[] args)
        {
            Triangle firstTr = new Triangle(new Point(3.0, 4.0), new Point(3.0, 8.0), new Point(6.0, 4.0));
            firstTr.PrintTriangle("firstTr");
            Console.WriteLine("-------------------------------------------");
            Triangle secondTr = new Triangle(new Line(new Point(3.0, 4.0), new Point(3.0, 8.0)),
                new Line(new Point(3.0, 8.0), new Point(6.0, 4.0)),
                new Line(new Point(3.0, 4.0), new Point(6.0, 4.0)));
            secondTr.PrintTriangle("secondTr");
            Console.WriteLine("-------------------------------------------");
            Triangle thirdTr = new Triangle(new Line(new Point(3.0, 4.0), new Point(3.0, 8.0)),
                new Line(new Point(3.0, 8.0), new Point(6.0, 4.0)));
            thirdTr.PrintTriangle("thirdTr");
        }
    }
}
