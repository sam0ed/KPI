using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Lab1;

public class Generator
{
    public long numberAmount;
    public int numberSizeInBytes;
    public int numberSizeInBits;

    public string fileName = "Input";
    private string fileType;

    public Random random = new Random();

    public Generator(int fileSizeInBytes, string fileType)
    {
        numberSizeInBytes = sizeof(long);
        numberSizeInBits = numberSizeInBytes * 8;
        numberAmount = (long)Math.Ceiling((double)(fileSizeInBytes / numberSizeInBytes));
        this.fileType = fileType;
    }

    public void GenerateFile()
    {
        if (fileType == ".bin")
            GenerateBinaryFile();
        else if (fileType == ".txt")
            GenerateTextFile();
    }

    public void GenerateBinaryFile()
    {
        BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create));

        for (int i = 0; i < numberAmount; i++)
        {
            bw.Write(LongRandom(0, ulong.MaxValue));
        }
    }

    public void GenerateTextFile()
    {
        StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create));
        //optimize

        for (int i = 0; i < numberAmount; i++)
        {
            sw.Write(LongRandom(0, ulong.MaxValue));//
        }
    }

    public ulong LongRandom(ulong min, ulong max)
    {
        byte[] buf = new byte[numberSizeInBytes];
        random.NextBytes(buf);
        ulong longRand = BitConverter.ToUInt64(buf, 0);
        longRand >>= random.Next(0, numberSizeInBits );

        return ((longRand % (max - min)) + min);
    }
}

//public class TestClass<T>
//{
//    public long numberAmount;
//    public TestClass()
//    {
//        unsafe
//        {

//            numberAmount =  Marshal.SizeOf<T>();
//            Console.WriteLine(numberAmount);
//        }
//    }
//}