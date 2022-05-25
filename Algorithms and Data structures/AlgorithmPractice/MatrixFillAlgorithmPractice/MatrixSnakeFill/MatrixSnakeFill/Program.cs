using System;
using System.Collections.Generic;

namespace MatrixSnakeFill
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new Matrix(10, 5);
            matrix.DiagonalSnakeFill(Matrix.Modes.StartRight);
            Console.WriteLine(matrix.ToString());
        }
    }
}