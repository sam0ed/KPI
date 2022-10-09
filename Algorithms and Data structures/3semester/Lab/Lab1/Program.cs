using Lab1;
using System.Globalization;
using System.Runtime.InteropServices;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Enter the size of data you want to generate(gb/mb/kb--min 4kb): ");
        ulong fileSizeInBytes = /*Converter.StringToBytes(Console.ReadLine()!)*/200*1024;

        Console.Write("Enter type of file you want to use for sorting(.txt-text/.bin-binary): ");
        string fileType = Console.ReadLine()!;

        Console.Write("How many files you want to use for sorting algorithm(up to 8): ");
        int filesAmount = /*int.Parse(Console.ReadLine()!)*/3;

        ulong runSizeInBytes;
        try
        {
            Console.Write("How long do you want a single run to be?\nSkip(Enter) this for default run size: ");
            runSizeInBytes = /*Converter.StringToBytes(Console.ReadLine()!)*/10*1024;
        }
        catch (Exception)
        {
            runSizeInBytes = (ulong)Math.Ceiling((double)(fileSizeInBytes / 512));
        }

        string sourceFileNameFull = ProgramConfig.inputFileNamePattern + fileType;
        FileConfig sourceFile=new FileConfig(sourceFileNameFull);
        sourceFile.dataSizeInBytes += fileSizeInBytes;
        sourceFile.fileManager.WriteRandFromRangeToFile(fileSizeInBytes);//tight coupling, but how else can we get data from source file

        Splitter splitter = new Splitter(sourceFile);
        splitter.Split(filesAmount, runSizeInBytes);


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

        //byte[] buffer=new byte[input.Length];
        //file.Read(buffer, 0, input.Length);
        //int[] result = buffer.Select(x => Convert.ToInt32(x)).ToArray();


        //splitter.Split(fileType);
        ///////////////////////////////////////
        ///

        //StreamWriter streamWriter1 = new StreamWriter(File.Open("Test", FileMode.Create));
        ////StreamWriter streamWriter2 = new StreamWriter(File.Open("Test1", FileMode.Create));
        //ulong num = 6516165151;
        //streamWriter1.Write(num);
        //streamWriter1.Close();
        //StreamReader streamReader1 = new StreamReader(File.OpenRead("Test"));
        //char[] buffer = new char[8];
        //streamReader1.Read(buffer);
        //ulong readNum =ulong.Parse(buffer);
        //Console.WriteLine(readNum);


    }


}