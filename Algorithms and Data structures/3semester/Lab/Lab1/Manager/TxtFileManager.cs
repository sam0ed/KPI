using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Formats.Asn1;
using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Lab1.Config;

namespace Lab1.Manager;

internal class TxtFileManager : FileManager
{
    private StreamWriter? writer;
    private StreamReader? reader;

    public TxtFileManager(string fileName)
    {
        ReassignToEmptyFile(fileName);
    }

    public override void ReassignToEmptyFile(string fileName)
    {
        this.fileName = fileName;
        OpenWriter(FileMode.Create);
        //writer.Dispose();
    }

    public override ulong[] ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex = null)
    {
        OpenReader(FileMode.Open);

        ulong resultSize = requestedSizeInBytes / (ulong)ProgramConfig.numberSizeInBytes;
        ulong[] result = new ulong[resultSize];
        for (ulong i = 0; i < resultSize; i++)
        {
            result[i] = ulong.Parse(reader.ReadLine()!);
        }
        //reader.Close();
        return result;

    }

    public override void WriteRandFromRangeToFile(ulong inputSizeInBytes, ulong minGeneratableValue = 0, ulong maxGeneratableValue = ulong.MaxValue)
    {
        if (inputSizeInBytes < 0) throw new ArgumentOutOfRangeException();
        ulong numberAmount = (ulong)Math.Ceiling((double)(inputSizeInBytes / (double)ProgramConfig.numberSizeInBytes));
        ulong[] randomUlongArr = new ulong[numberAmount];
        for (ulong i = 0; i < numberAmount; i++)
        {
            randomUlongArr[i] = UlongRandom(minGeneratableValue, maxGeneratableValue);//
        }
        WriteToFile(randomUlongArr);

    }

    public override void WriteToFile(ulong[] inputData, FileMode fileMode = FileMode.Append)
    {
        OpenWriter(fileMode);
        for (int i = 0; i < inputData.Length; i++)
        {
            writer.WriteLine(inputData[i]);
        }
    }

    public override void OpenReader(FileMode fileMode)
    {
        if (reader == null)
        {
            if (writer != null)
            {
                writer?.Close();
                writer = null;
            }
            reader = new StreamReader(File.Open(fileName, fileMode));
        }
    }

    public override void OpenWriter(FileMode fileMode)
    {
        if (writer == null)
        {
            if (reader != null)
            {
                reader?.Close();
                reader = null;
            }
            writer = new StreamWriter(File.Open(fileName, fileMode));
        }
    }
}
