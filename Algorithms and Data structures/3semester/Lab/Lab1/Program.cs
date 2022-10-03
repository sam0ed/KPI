using Lab1;
using System.Globalization;
using System.Runtime.InteropServices;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Enter the size of the file you want to generate(gb/mb/kb--min 4kb): ");
        string size = Console.ReadLine()!;

        Console.Write("Enter type of file you want to use for sorting(.txt-text/.bin-binary): ");
        string fileType = Console.ReadLine()!;

        Console.Write("How many files you want to use for sorting algorithm(up to 8): ");
        int filesAmount = int.Parse(Console.ReadLine()!);

        Generator generator = new Generator(SizeInBytes(size), fileType);
        generator.GenerateFile();

    }

    static int SizeInBytes(string unconvertedSize)
    {
        string actualSizeArray = string.Concat(unconvertedSize.ToCharArray().Where(x => x > 47 && x < 58));
        if (string.IsNullOrEmpty(actualSizeArray)) throw new Exception("wrong formatting");

        int unitSize = int.Parse(actualSizeArray);
        string unitMeasure = string.Concat(unconvertedSize.Except(actualSizeArray));

        int sizeInBytes = unitMeasure.ToLower() switch
        {
            "kb" => 1024 * unitSize,
            "mb" => 1024 * 1024 * unitSize,
            "gb" => 1024 * 1024 * 1024 * unitSize,
            _ => 0
        };
        return sizeInBytes;
    }
}