using Lab1;
using System.Globalization;
using System.Runtime.InteropServices;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Enter the size of the file you want to generate(gb/mb/kb--min 4kb): ");
        ulong fileSizeInBytes = SizeInBytes(Console.ReadLine()!);

        Console.Write("Enter type of file you want to use for sorting(.txt-text/.bin-binary): ");
        string fileType = Console.ReadLine()!;

        Console.Write("How many files you want to use for sorting algorithm(up to 8): ");
        int filesAmount = int.Parse(Console.ReadLine()!);

        ulong runSizeInBytes;
        try
        {
            Console.Write("How long do you want a single run to be?\nSkip(Enter) this for default run size:");
            runSizeInBytes = SizeInBytes(Console.ReadLine()!);
        }
        catch (Exception)
        {
            runSizeInBytes = (ulong)Math.Ceiling((double)(fileSizeInBytes / 512));
        }

        Generator generator = new Generator( fileType);
        generator.GenerateFile(fileSizeInBytes);

        /////////////////////////////////////////////////////////////////////
        Splitter splitter = new Splitter(runSizeInBytes, fileSizeInBytes, filesAmount);

        //ulong number = 1523654555;
        //double newNum= Convert.ToDouble(number)%5.5;
        //Console.WriteLine(newNum);
    }

    static ulong SizeInBytes(string unconvertedSize)
    {
        string actualSizeArray = string.Concat(unconvertedSize.ToCharArray().Where(x => x > 47 && x < 58));
        if (string.IsNullOrEmpty(actualSizeArray)) throw new Exception("Input format incorrect");

        int unitSize = int.Parse(actualSizeArray);
        string unitMeasure = string.Concat(unconvertedSize.Except(actualSizeArray));

        ulong sizeInBytes = unitMeasure.ToLower() switch
        {
            "b"=>(ulong)unitSize,
            "kb" => (ulong)(1024 * unitSize),
            "mb" => (ulong)(1024 * 1024 * unitSize),
            "gb" => (ulong)(1024 * 1024 * 1024 * unitSize),
            _ => 0
        };
        return sizeInBytes;
    }
}