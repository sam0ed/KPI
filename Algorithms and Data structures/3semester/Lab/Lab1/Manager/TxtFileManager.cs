using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Formats.Asn1;
using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Lab1.Config;
using Lab1.Config.FileConfig;

namespace Lab1.Manager;

internal class TxtFileManager : FileManager
{
    private StreamWriter? writer;
    private StreamReader? reader;

    public TxtFileManager(FileConfig fileConfig, Action<FileConfig, ulong> doOnWriting, Action<FileConfig, ulong> doOnReading)
        : base(fileConfig, doOnWriting, doOnReading) { }

    public override ulong[] ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex = null)
    {
        OpenReader(FileMode.Open);

        ulong resultSize = requestedSizeInBytes / (ulong)ProgramConfig.numberSizeInBytes;
        ulong[] result = new ulong[resultSize];
        for (ulong i = 0; i < resultSize; i++)
        {
            result[i] = ulong.Parse(reader.ReadLine()!);
        }
        changeFileConfigAfterReading.Invoke(fileConfig, (ulong)result.Length * ProgramConfig.numberSizeInBytes);
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
        changeFileConfigAfterWriting.Invoke(fileConfig, (ulong)inputData.Length * ProgramConfig.numberSizeInBytes);
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
            reader = new StreamReader(File.Open(fileConfig.fileName, fileMode));
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
            writer = new StreamWriter(File.Open(fileConfig.fileName, fileMode));
        }
    }
}
