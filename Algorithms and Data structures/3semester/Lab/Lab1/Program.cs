using Lab1;
using Lab1.Config;
using Lab1.Config.FileConfig;
using Lab1.Manager;
using Lab1.Utility;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Enter the size of data you want to generate(gb/mb/kb--min 4kb): ");
        ulong fileSizeInBytes = Converter.StringToBytes(Console.ReadLine()!);

        Console.Write("Enter type of file you want to use for sorting(.txt-text/.bin-binary): ");
        string fileType = Console.ReadLine()!;

        Console.Write("How many files you want to use for sorting algorithm(up to 8): ");
        int filesAmount = int.Parse(Console.ReadLine()!);

        ulong runSizeInBytes;
        try
        {
            Console.Write("How long do you want a single run to be?\nSkip(Enter) this for default run size: ");
            runSizeInBytes = Converter.StringToBytes(Console.ReadLine()!);
        }
        catch (Exception)
        {
            runSizeInBytes = (ulong)Math.Ceiling((double)(fileSizeInBytes / 512));
        }

        string sourceFileNameFull = ProgramConfig.inputFileNamePattern + fileType;
        FileConfig sourceFile = new ExtSortFileConfig(sourceFileNameFull);
        (sourceFile as ExtSortFileConfig).fileManager.WriteRandFromRangeToFile(fileSizeInBytes);//tight coupling, but how else can we get data from source file

        Splitter splitter = new Splitter(sourceFile as ExtSortFileConfig);
        ExtSortFileConfig[] sortFiles = splitter.Split(filesAmount, runSizeInBytes);

        //PolyPhaseSort.GenerateHeaps(ref parameter);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        ExtSortFileConfig result = PolyPhaseSort.MergedRunsInternalSort(sortFiles, filesAmount);
        stopwatch.Stop();
        Console.WriteLine($"Elapsed time of sort execution is: {stopwatch.Elapsed.Seconds}");

        result.fileManager.PrintPart(result.dataSizeInBytes);


        //FileManager file1 = new MemoryMappedFileManager("SortingFile2.bin");
        //FileManager file2 = new MemoryMappedFileManager("secondFile.bin");
    }


}