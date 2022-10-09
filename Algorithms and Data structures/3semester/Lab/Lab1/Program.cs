using Lab1;
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
        FileConfig sourceFile = new FileConfig(sourceFileNameFull);
        sourceFile.dataSizeInBytes += fileSizeInBytes;
        sourceFile.fileManager.WriteRandFromRangeToFile(fileSizeInBytes);//tight coupling, but how else can we get data from source file

        FileConfig result = PolyPhaseSort.Sort(sourceFile, filesAmount, runSizeInBytes);
        //if (sourceFile.fileType == "bin")
        result.fileManager.PrintPart(result.dataSizeInBytes);

        //Splitter splitter = new Splitter(sourceFile);
        //List<FileConfig> sortFiles= splitter.Split(filesAmount, runSizeInBytes);



        //Splitter splitter = new Splitter(runSizeInBytes, fileSizeInBytes, filesAmount);
        //ulong additionalSize = splitter.FindAdditionalSize();
        //fileManager.WriteRandToFile(additionalSize, FileMode.Append, 1);
        //splitter.fileSizeInBytes += additionalSize;

        //ulong[] firstSeria = sourceFile.fileManager.ReadFromFile(sourceFile.dataSizeInBytes / (ulong)sourceFile.runsAmount!);
        //sourceFile.fileManager.WriteRandFromRangeToFile(fileSizeInBytes);
        //ulong[] secondSeria = sourceFile.fileManager.ReadFromFile(sourceFile.dataSizeInBytes / (ulong)sourceFile.runsAmount!);

        //FileStream file = File.Open("Test.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //byte[] input = new byte[] { (byte)132, (byte)21, (byte)154, (byte)243, (byte)82 };
        //file.Write(input);



    }


}