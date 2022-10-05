using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Lab1;

public class Generator
{
    public ulong numberAmount;
    private string fileType;

    public const int numberSizeInBytes = sizeof(long);
    public const int numberSizeInBits = numberSizeInBytes * 8;

    public const string fileName = "Input";

    public Random random = new Random();

    public Generator(string fileType)
    {

        this.fileType = fileType;
    }

    public void GenerateFile(ulong fileSizeInBytes)
    {
        if (fileType == ".bin")
            GenerateBinaryFile(fileSizeInBytes);
        else if (fileType == ".txt")
            GenerateTextFile(fileSizeInBytes);
    }

    public void GenerateBinaryFile(ulong fileSizeInBytes)
    {
        numberAmount = (ulong)Math.Ceiling((double)(fileSizeInBytes / numberSizeInBytes));
        BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create));

        for (ulong i = 0; i < numberAmount; i++)
        {
            bw.Write(LongRandom(0, ulong.MaxValue));
        }
    }

    public void GenerateTextFile(ulong fileSizeInBytes)
    {
        numberAmount = (ulong)Math.Ceiling((double)(fileSizeInBytes / numberSizeInBytes));
        StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create));
        //optimize

        for (ulong i = 0; i < numberAmount; i++)
        {
            sw.Write(LongRandom(0, ulong.MaxValue));//
        }
    }

    public ulong LongRandom(ulong min, ulong max)
    {
        byte[] buf = new byte[numberSizeInBytes];
        random.NextBytes(buf);
        ulong longRand = BitConverter.ToUInt64(buf, 0);
        longRand >>= random.Next(0, numberSizeInBits);

        return ((longRand % (max - min)) + min);
    }

    //public static void ExpandFile(ulong sizeToAdd)
    //{
    //    if (sizeToAdd < 0) throw new ArgumentOutOfRangeException();

    //    for (int i = 0; i < length; i++)
    //    {

    //    }
    //}
}
