using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Lab1;

internal class TxtFileManager : FileManager
{
    public string fileName;
    private StreamWriter sw;
    private StreamReader sr;
    public TxtFileManager(string fileName)
    {
        ReassignToEmptyFile(fileName);
    }
    public override void ReassignToEmptyFile(string fileName)
    {
        this.fileName = fileName;
        StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create));
        sw.Dispose();
    }

    public override ulong[] ReadFromFile(ulong requestedSizeInBytes, ulong? startIndex = null)
    {
        //if (srCashed == null || fileReadNameCashed != fileName)//
        //{
        //    srCashed?.Dispose();
        //    fileReadNameCashed = fileName;//
        //    try
        //    {
        //        srCashed = new StreamReader(File.Open(fileName, FileMode.Open));
        //    }
        //    catch (IOException)
        //    {
        //        swCashed?.Close();
        //    }
        //    finally
        //    {
        //        srCashed = new StreamReader(File.Open(fileName, FileMode.Open));
        //    }
        //}
        try
        {
            if (sr == null)
                sr = new StreamReader(File.Open(fileName, FileMode.Open));
        }
        catch (IOException)
        {
            sw?.Close();
            sr = new StreamReader(File.Open(fileName, FileMode.Open));
        }

        ulong resultSize = requestedSizeInBytes / (ulong)ProgramConfig.numberSizeInBytes;
        ulong[] result = new ulong[resultSize];
        for (ulong i = 0; i < resultSize; i++)
        {
            result[i] = ulong.Parse(sr.ReadLine()!);
        }
        //sr.Close();
        return result;

    }

    public override ulong WriteRandFromRangeToFile(ulong inputSizeInBytes, ulong minGeneratableValue = 0, ulong maxGeneratableValue = ulong.MaxValue)
    {
        if (inputSizeInBytes < 0) throw new ArgumentOutOfRangeException();
        ulong numberAmount = (ulong)Math.Ceiling((double)(inputSizeInBytes / (double)ProgramConfig.numberSizeInBytes));


        try
        {
            if (sw == null)
                sw = new StreamWriter(File.Open(fileName, FileMode.Append));
        }
        catch (IOException)
        {
            sr?.Close();
            sw = new StreamWriter(File.Open(fileName, FileMode.Append));
        }

        for (ulong i = 0; i < numberAmount; i++)
        {
            sw.WriteLine(LongRandom(minGeneratableValue, maxGeneratableValue));//
        }
        return numberAmount;

        //ulong targetStartLength = (ulong)new FileInfo(fileName).Length;

        //if (swCashed == null || fileWriteNameCashed != fileName)//
        //{
        //    swCashed?.Dispose();//was in the end
        //    fileWriteNameCashed = fileName;//
        //    try
        //    {
        //        swCashed = new StreamWriter(File.Open(fileName, FileMode.Append));
        //    }
        //    catch (IOException)
        //    {
        //        srCashed?.Close();
        //    }
        //    finally
        //    {

        //        swCashed = new StreamWriter(File.Open(fileName, FileMode.Append));
        //    }
        //}
        //sw.Dispose();

        //ulong targetLength=targetStartLength;
        //ulong numberAmount = 0;
        //while (targetLength<targetStartLength+inputSizeInBytes)
        //{
        //    if (numberAmount % 512 == 0) targetLength = (ulong)new FileInfo(fileName).Length;
        //    sw.WriteLine(LongRandom(minGeneratableValue, maxGeneratableValue));//
        //    numberAmount++;
        //}

    }



}
