using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_CSharp_
{
    class Point
    {
        public (double X, double Y) Coordinates { get; set; }

        public Point(double x, double y)
        {
            Coordinates = (x, y);
        }
        public void PrintPoint()
        {
            Console.Write($" ( x: {Coordinates.X} y: {Coordinates.Y} )");
        }
        public static bool operator ==(Point a, Point b) => a.Coordinates.X == b.Coordinates.X && a.Coordinates.Y == b.Coordinates.Y;
        public static bool operator !=(Point a, Point b) => a.Coordinates.X != b.Coordinates.X && a.Coordinates.Y != b.Coordinates.Y;
    }
}
