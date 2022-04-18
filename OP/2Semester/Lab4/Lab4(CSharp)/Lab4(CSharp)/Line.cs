using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_CSharp_
{
    class Line
    {
        public (Point A, Point B) Endings { get; set; }
        public double Length { get; set; }
        public Line(Point a, Point b)
        {
            Endings = (a, b);
            Length = Math.Sqrt(Math.Pow(Endings.A.Coordinates.X - Endings.B.Coordinates.X, 2) + Math.Pow(Endings.A.Coordinates.Y - Endings.B.Coordinates.Y, 2));
        }
       
        public static Line operator++(Line line)
        {
            line.ResizeLine(1.0);
            return line;
        }
        public static Line operator--(Line line)
        {
            line.ResizeLine(-1.0);
            return line;
        }
        public static Line operator+(Line line, double value)
        {
            line.ResizeLine(value);
            return line;
        }
        public static Line operator -(Line line, double value)
        {
            line.ResizeLine(-value);
            return line;
        }
        public void ResizeLine(double value, string side = "A")
        {
            //перевіряємо з яким кінцем відрізка проводимо операцію
            if (side == "A")
            {
                double unitVectorY = (this.Endings.A.Coordinates.Y - this.Endings.B.Coordinates.Y) / this.Length; //розраховуємо у частину юніт вектора поточного відрізка
                double unitVectorX = (this.Endings.A.Coordinates.X - this.Endings.B.Coordinates.X) / this.Length;
                if (this.Length + value > 0)// якщо зміна довжини не змінить положення робочого кінця відрізка(А) відносно іншого кінця
                {
                    this.Endings.A.Coordinates = (this.Endings.A.Coordinates.X + unitVectorX * value, this.Endings.A.Coordinates.Y + unitVectorY * value);
                }
            }
            else if(side=="B")
            {
                double unitVectorY = (this.Endings.B.Coordinates.Y - this.Endings.A.Coordinates.Y) / this.Length; //розраховуємо у частину юніт вектора поточного відрізка
                double unitVectorX = (this.Endings.B.Coordinates.X - this.Endings.A.Coordinates.X) / this.Length;
                if (this.Length + value > 0)// якщо зміна довжини не змінить положення робочого кінця відрізка(А) відносно іншого кінця
                {
                    this.Endings.B.Coordinates = (this.Endings.B.Coordinates.X + unitVectorX * value, this.Endings.B.Coordinates.Y + unitVectorY * value);
                }
            }
            this.Length += value;
        }
        public void PrintLine()
        {
            Console.WriteLine($"Coordinates of line vertices are:");
            Endings.A.PrintPoint();
            Console.Write("\t");
            Endings.B.PrintPoint();
            Console.Write("\n");
            Console.WriteLine($"Length : {this.Length} ");
        }
    }
}
