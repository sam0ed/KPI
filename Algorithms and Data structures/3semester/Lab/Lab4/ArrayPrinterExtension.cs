namespace Lab4
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
                else Console.WriteLine(arr[i] );
            }
        }

        public static void Print<T>(this T[,] values)
        {
            for (int i = 0; i < values.GetUpperBound(0); i++)
            {
                for (int j = 0; j < values.GetUpperBound(1); j++)
                {
                    Console.Write(Math.Round(Convert.ToDouble(values[i, j]), 3).ToString().PadLeft(5) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
