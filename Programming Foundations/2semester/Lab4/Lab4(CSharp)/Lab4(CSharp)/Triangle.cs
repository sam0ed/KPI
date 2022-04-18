using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_CSharp_
{
    class Triangle
    {
        public (Point A, Point B, Point C) Vertices { get; set; }
        public Line[] Sides { get; set; }
        public double Area { get; set; }
        #region constructorOverloads
        public Triangle(Point a, Point b, Point c)
        {
            Vertices = (a, b, c);
            Sides = new[] { new Line(a, b), new Line(b, c), new Line(a, c) };
        }
        public Triangle(Line a, Line b, Line c)
        {
            Point last = a.Endings.A == b.Endings.A ? b.Endings.B : b.Endings.A;
            Vertices = (a.Endings.A, a.Endings.B, last);
            Sides = new[] { a, b, c };
        }
        public Triangle(Line a, Line b) : this(a.Endings.A, a.Endings.B, (b.Endings.A == a.Endings.A||b.Endings.A==a.Endings.B) ? b.Endings.B : b.Endings.A) { }
        #endregion constructorOverloads

        #region operatorOverloads
        public static Triangle operator ++(Triangle tr)
        {
            tr.ResizeTriangle(1);
            return tr;
        }
        public static Triangle operator --(Triangle tr)
        {
            tr.ResizeTriangle(-1);
            return tr;
        }
        public static Triangle operator +(Triangle tr, double value)
        {
            tr.ResizeTriangle(value);
            return tr;
        }
        public static Triangle operator -(Triangle tr, double value)
        {
            tr.ResizeTriangle(-value);
            return tr;
        }
        #endregion operatorOverloads

        #region methods
        public void ResizeTriangle(double value, string vertix = "A")
        {

            if (vertix == "A")
            {
                List<Point> newThirdSideCoord = new List<Point>(); //contains coordinates of third side vertices after first two sides are resized
                List<Line> thirdSide = new List<Line>(Sides); //after foreach cycle resized sides will be excluded from list, so it will contain only one unresized side

                //in the cycle we go through all sides of triangle and if one of its ends is the vertix which is used as initial point for scaling, we resize the side
                //on its opposite vertix
                foreach (var side in Sides)
                {
                    if (side.Endings.A == Vertices.A)
                    {
                        side.ResizeLine(value, "B");
                        newThirdSideCoord.Add(side.Endings.B);
                        thirdSide.Remove(side);
                    }
                    else if (side.Endings.B == Vertices.A)
                    {
                        side.ResizeLine(value, "A");
                        newThirdSideCoord.Add(side.Endings.A);
                        thirdSide.Remove(side);
                    }
                }
                thirdSide[0].Endings = (newThirdSideCoord[0], newThirdSideCoord[1]);
            }
        }

        public double CalculateArea()
        {
            //Gerons formula
            double p = (Sides[0].Length + Sides[1].Length + Sides[2].Length) / 2;
            return Math.Sqrt(p * (p - Sides[0].Length) * (p - Sides[1].Length) * (p - Sides[2].Length));
        }
        public void PrintTriangle(string triangleIdentifyer)
        {
            Console.WriteLine($"Triangle {triangleIdentifyer} has elements:");
            foreach (var item in Sides)
            {
                item.PrintLine();
            }
            Console.WriteLine($"Area : {CalculateArea()}");

        }
        #endregion methods
    }
}
