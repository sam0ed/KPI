using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab2
{
    static class ProgramMethods
    {
        //method for displaying lists of programs
        public static void displayFilePrograms(List<ProgramTV> programTVs)
        {
            foreach (var program in programTVs)
            {
                program.Print();
            }
        }

        //method for getting all the programs from a file
        public static List<ProgramTV> GetFilePrograms(string filePath)
        {
            List<ProgramTV> fileContent = new List<ProgramTV>();
            BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open));
            Console.WriteLine($"Reading content of file \"{filePath}\". ");

            while (reader.PeekChar() != -1)
            {
                fileContent.Add(GetProgramTVFromFile(reader));
            }
            return fileContent;
        }

        //method for reading single program from file
        public static ProgramTV GetProgramTVFromFile(BinaryReader reader)
        {
            ProgramTV fileContent = new ProgramTV();

            fileContent.Title = reader.ReadString();
            fileContent.StartTime = DateTime.Parse(reader.ReadString());
            fileContent.EndTime = DateTime.Parse(reader.ReadString());
            fileContent.TimeSpan = new TimeSpan(fileContent.EndTime.Hour - fileContent.StartTime.Hour, fileContent.EndTime.Minute - fileContent.StartTime.Minute, fileContent.EndTime.Second - fileContent.StartTime.Second);

            return fileContent;
        }

        //method for writing a list of some objects to file
        public static void WriteIEnumerableToFile<T>(string filePath, IEnumerable<T> someObject, FileMode mode = FileMode.Create)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(filePath, mode));
            foreach (var part in someObject)
            {
                //this switch construction is not obligatory in this case, i just wrote it for training
                //we can make this class non generic easily its just more fun this way
                switch (part)
                {
                    case ProgramTV pr:
                        writer.Write(pr.Title);
                        writer.Write(pr.StartTime.ToString());
                        writer.Write(pr.EndTime.ToString());
                        break;
                    default:
                        Console.WriteLine($"The way of serialization wasn`t specified for given object." +
                            $" Error occured in {typeof(Program).GetMethod("WriteIEnumerableToFile").Name}");
                        break;
                }

            }
            writer.Flush();
            writer.Close();
        }


        //returns all programs from input as a List? tracks if user uses stop button 
        public static List<ProgramTV> GetInputPrograms()
        {

            List<ProgramTV> myProgrames = new List<ProgramTV>();
            ConsoleKeyInfo key;
            bool continueInput;
            do
            {
                myProgrames.Add(GetProgramTVFromConsole());

                Console.WriteLine("To stop prress ctrl+e now, to continue press Enter");
                key = Console.ReadKey(true);
                continueInput = key.Key != ConsoleKey.E && key.Modifiers != ConsoleModifiers.Control;
            } while (continueInput);
            Console.Write('\n');
            return myProgrames;

        }

        //processes one program input and returns it as an object of ProgramTV
        public static ProgramTV GetProgramTVFromConsole()
        {
            ProgramTV input = new ProgramTV();
            DateTime temp;

            Console.Write("Enter program name: ");
            input.Title = Console.ReadLine();

            Console.Write("Enter program start time: ");
            DateTime.TryParseExact(Console.ReadLine(), "H:m", null, System.Globalization.DateTimeStyles.None, out temp);
            input.StartTime = temp;

            Console.Write("Enter program end time: ");
            DateTime.TryParseExact(Console.ReadLine(), "H:m", null, System.Globalization.DateTimeStyles.None, out temp);
            input.EndTime = temp;

            input.TimeSpan = new TimeSpan(input.EndTime.Hour - input.StartTime.Hour, input.EndTime.Minute - input.StartTime.Minute, input.EndTime.Second - input.StartTime.Second);

            return input;
        }
    }
}
