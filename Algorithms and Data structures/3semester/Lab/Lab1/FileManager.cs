using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Lab1;

public class FileManager
{
    public ulong numberAmount;
    private string fileType;

    public const int numberSizeInBytes = sizeof(long);
    public const int numberSizeInBits = numberSizeInBytes * 8;

    public const string fileName = "Input";

    public Random random = new Random();

    public FileManager(string fileType)
    {

        this.fileType = fileType;
    }

    public void GenerateFile(ulong fileSizeInBytes)
    {
        if (fileType == ".bin")
            WriteRandomToBinaryFile(fileSizeInBytes, FileMode.Create);
        else if (fileType == ".txt")
            WriteRandomToTextFile(fileSizeInBytes, FileMode.Create);
    }

    public void WriteRandomToBinaryFile(ulong inputSizeInBytes, FileMode fileMode)
    {
        numberAmount = (ulong)Math.Ceiling((double)(inputSizeInBytes / numberSizeInBytes));
        BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create));

        for (ulong i = 0; i < numberAmount; i++)
        {
            bw.Write(LongRandom(0, ulong.MaxValue));
        }
    }

    public void WriteRandomToTextFile(ulong inputSizeInBytes, FileMode fileMode)
    {
        if (inputSizeInBytes < 0) throw new ArgumentOutOfRangeException();

        numberAmount = (ulong)Math.Ceiling((double)(inputSizeInBytes / numberSizeInBytes));
        StreamWriter sw = new StreamWriter(File.Open(fileName, fileMode));

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

}
