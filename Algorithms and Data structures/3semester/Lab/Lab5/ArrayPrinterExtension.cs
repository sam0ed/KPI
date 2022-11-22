namespace Lab5
{
    internal static class ArrayPrinterExtension
    {
        public static void Print<T>(this List<T> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (i != arr.Count - 1)
                {
                    Console.Write(arr[i] + " -> ");
                    if (Console.CursorLeft > Console.WindowWidth - 10) Console.WriteLine();
                }
                else Console.WriteLine(arr[i]);
            }
        }

        public static void Print<T>(this T[,] values)
        {
            int padding = 5;
            Console.Write(new string(' ',padding));
            for (int i = 0; i < values.GetLength(1); i++)
            {
                Console.Write(i.ToString().PadLeft(padding));
            }
            Console.WriteLine();
            for (int i = 0; i < values.GetLength(1)+1; i++)
            {
                Console.Write("---".PadLeft(padding));
            }
            Console.WriteLine();
            for (int i = 0; i < values.GetLength(0); i++)
            {
                Console.Write((i.ToString()+"|").PadLeft(padding));
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.Write(Math.Round(Convert.ToDouble(values[i, j]), 3).ToString().PadLeft(padding));
                    if (i == j) Console.ResetColor();
                }

                Console.WriteLine();
            }
        }
    }
}