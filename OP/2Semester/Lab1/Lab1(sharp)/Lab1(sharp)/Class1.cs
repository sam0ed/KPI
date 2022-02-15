using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_sharp_
{
    internal static class FileManager
    {
     
        public static void PrintContents(in string fileName)
        {
            Console.WriteLine($"You are viewing contents of {fileName.Substring(fileName.LastIndexOf('/') + 1)}  ");
            StreamReader streamReader = new StreamReader(fileName);
            Console.WriteLine(streamReader.ReadToEnd());
            streamReader.Close();
        }

        public static void AppendFile(in string fileName)
        {
            Console.WriteLine($"You are editing {fileName.Substring(fileName.LastIndexOf('/') + 1)} \n " +
                $"Press ctrl+E to stop the edit. \nPlace your input here:");

            StreamWriter streamWriter=new StreamWriter(fileName, true);
            string[] res = ReadMultilineInp();
            for (int i = 0; i < res.Length; i++)
            {
                streamWriter.WriteLine(res[i]);
            }
            Console.WriteLine('\n');
            streamWriter.Close();

        }

        public static void RewriteFile(in string fileName)
        {
            StreamWriter streamWriter = new StreamWriter(fileName, false);
            streamWriter.Close();
            AppendFile(fileName);
        }

        public static void InvertBinaryDigits(in string fileName, in string newFileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            StreamWriter streamWriter = new StreamWriter(newFileName, false);
            string temp;
            while(!streamReader.EndOfStream)
            {
                temp = streamReader.ReadLine();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] == '0')
                    {
                        char[] letters = temp.ToCharArray();
                        letters[i] = '1';
                        temp = new string(letters);
                    }
                    else if (temp[i] == '1')
                    {
                        char[] letters = temp.ToCharArray();
                        letters[i] = '0';
                        temp = new string(letters);
                    }
                }
                streamWriter.WriteLine(temp);
            }
            streamReader.Close();
            streamWriter.Close();
        }

        public static string[] ReadMultilineInp()
        {
            ConsoleKeyInfo key;
            bool contInp;
            string temp = "";

            do
            {
                contInp = true;
                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        temp += '\n';
                        Console.CursorTop++;
                        break;
                    case ConsoleKey.E when key.Modifiers == ConsoleModifiers.Control:
                        contInp = false;
                        break;
                    case ConsoleKey.Backspace:
                        Console.Write(' ');
                        Console.CursorLeft--;
                        break;
                    default:
                        temp += key.KeyChar.ToString();
                        break;
                }
            }
            while (contInp);

            return temp.Split('\n');
        }
    }
}
