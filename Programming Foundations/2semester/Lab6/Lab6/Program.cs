namespace Lab6 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fileName = "SomeProgram";
            Console.WriteLine("Enter the code of program in C/C++ (do not create identifiers with the same name but different nested level) :");
            List<string> input = new List<string>(ReadMultilineInp());
            using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (string line in input)
                {
                    writer.WriteLine(line);
                }
            }

            List<string> identifyerStartDeterm = new List<string>
            {
                "int",
                "long",
                //"long long",
                "double",
                "float",
                "bool",
                "char",
                "string",
                "auto",
                "void"
            };

            List<string> identifyerEndDeterm = new List<string>
            {
                "=",
                ";",
                "(",
            };

            List<string> typesStartDeterm = new List<string>
            {
                "class",
                "struct",
                "enum",
            };

            List<KeyValuePair<int, string>> allIdenifyers = GetSqueezedStr(input, identifyerStartDeterm, identifyerEndDeterm);
            Console.WriteLine();
            foreach (var item in allIdenifyers)
            {
                Console.WriteLine($"Row Number: {item.Key}\tIdentifyer: {item.Value}");
            }

            List<KeyValuePair<int, string>> allTypes = GetSqueezedStr(input, typesStartDeterm, new List<string>());
            foreach (var item in allTypes)
            {
                Console.WriteLine($"Row Number: {item.Key}\tType: {item.Value}");
            }
        }

        private static List<KeyValuePair<int, string>> GetSqueezedStr(List<string> input, List<string> identifyerStartDeterm, List<string> identifyerEndDeterm)
        {
            List<KeyValuePair<int, string>> identifyerList = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Length > 0)
                {
                    bool LineContainsIdentifyer = false;
                    for (int j = 0; j < identifyerStartDeterm.Count; j++)
                    {
                        int index;
                        while ((index = input[i].IndexOf(identifyerStartDeterm[j])) != -1)
                        {
                            input[i] = input[i].Substring(index+identifyerStartDeterm[j].Length);
                            LineContainsIdentifyer = true;
                        }
                    }
                    if (LineContainsIdentifyer)
                    {

                        List<string> sameLineIdentifyerList = new List<string>(input[i].Split(","));

                        for (int j = 0; j < sameLineIdentifyerList.Count; j++)
                        {
                            int topTrim = sameLineIdentifyerList[j].Length;
                            for (int k = 0; k < identifyerEndDeterm.Count; k++)
                            {
                                int index = sameLineIdentifyerList[j].IndexOf(identifyerEndDeterm[k]);
                                topTrim = (topTrim > index && index != -1) ? index : topTrim;
                            }
                            sameLineIdentifyerList[j] = sameLineIdentifyerList[j].Remove(topTrim).Trim();

                            bool foundSameIdenifyer = false;
                            for (int k = 0; k < identifyerList.Count; k++)
                            {
                                if (identifyerList[k].Value == sameLineIdentifyerList[j])
                                    foundSameIdenifyer = true;
                            }
                            if (!foundSameIdenifyer)
                            {
                                identifyerList.Add(new KeyValuePair<int, string>(i, sameLineIdentifyerList[j]));
                            }
                        }
                    }
                }
            }
            return identifyerList;
        }
        //private static void GetIdenifyers(List<string> input, List<string> identifyerStartDeterm, List<string> identifyerEndDeterm)
        //{
        //    List<KeyValuePair<int, string>> identifyerList = new List<KeyValuePair<int, string>>();
        //    for (int i = 0; i < input.Count; i++)
        //    {
        //        if (input[i].Length > 0)
        //        {
        //            TrimIdentidyerName(input[i], identifyerStartDeterm, identifyerEndDeterm);
        //            //int firstStartDetermIndex = input[i].Length;
        //            //string firstStartDeterm = null;
        //            //for (int j = 0; j < identifyerStartDeterm.Count; j++)
        //            //{
        //            //    string arrayVersion = identifyerStartDeterm[j] + "[]";
        //            //    if (input[i].Contains(identifyerStartDeterm[j]) && input[i].IndexOf(identifyerStartDeterm[j]) < firstStartDetermIndex)
        //            //    {
        //            //        firstStartDetermIndex = input[i].IndexOf(identifyerStartDeterm[j]);
        //            //        firstStartDeterm = identifyerStartDeterm[j];
        //            //    }
        //            //    if (input[i].Contains(arrayVersion) && input[i].IndexOf(arrayVersion) > firstStartDetermIndex)
        //            //    {
        //            //        firstStartDetermIndex = input[i].IndexOf(arrayVersion);
        //            //        firstStartDeterm = arrayVersion;
        //            //    }
        //            //}
        //            //if (firstStartDetermIndex != input[i].Length)
        //            //{

        //            //    if (firstStartDeterm.Length != input[i].Length)
        //            //    {
        //            //        int containsEndDeterm;
        //            //        string temp = input[i][(firstStartDetermIndex + firstStartDeterm.Length)..];
        //            //        for (int j = 0; j < identifyerEndDeterm.Count; j++)
        //            //        {
        //            //            if (temp.Contains(identifyerEndDeterm[j]))

        //            //        }
        //            //    }

        //            //}

        //        }
        //    }
        //}

        //private static string TrimIdentidyerName(string line, List<string> identifyerStartDeterm, List<string> identifyerEndDeterm)
        //{
        //    int firstStartDetermIndex = line.Length;
        //    string firstStartDeterm = null;
        //    for (int j = 0; j < identifyerStartDeterm.Count; j++)
        //    {
        //        string arrayVersion = identifyerStartDeterm[j] + "[]";
        //        if (line.Contains(identifyerStartDeterm[j]) && line.IndexOf(identifyerStartDeterm[j]) < firstStartDetermIndex)
        //        {
        //            firstStartDetermIndex = line.IndexOf(identifyerStartDeterm[j]);
        //            firstStartDeterm = identifyerStartDeterm[j];
        //        }
        //        if (line.Contains(arrayVersion) && line.IndexOf(arrayVersion) > firstStartDetermIndex)
        //        {
        //            firstStartDetermIndex = line.IndexOf(arrayVersion);
        //            firstStartDeterm = arrayVersion;
        //        }
        //    }
        //    if (firstStartDetermIndex != line.Length)
        //    {

        //        if (firstStartDeterm.Length != line.Length)
        //        {
        //            bool containsEndDeterm = false;
        //            string temp = line[(firstStartDetermIndex + firstStartDeterm.Length)..];
        //            for (int j = 0; j < identifyerEndDeterm.Count; j++)
        //            {
        //                if (temp.Contains(identifyerEndDeterm[j]))
        //                    containsEndDeterm = true;
        //            }
        //            if(containsEndDeterm==true)
        //            {
        //                //
        //                Console.WriteLine();
        //                Console.WriteLine(temp);
        //                //
        //                return TrimIdentidyerName(temp, identifyerStartDeterm, identifyerEndDeterm);
        //            }

        //        }

        //    }
        //    return line;
        //}

        public static string[] ReadMultilineInp()
        {
            ConsoleKeyInfo key;
            bool contInp;
            string temp = "";

            //added line numeration 
            int lineCounter = 1;
            Console.Write(lineCounter++.ToString() + " ");
            //
            do
            {

                contInp = true;
                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        temp += '\n';
                        Console.CursorTop++;
                        Console.Write(lineCounter++.ToString() + " ");
                        break;
                    case ConsoleKey.E when key.Modifiers == ConsoleModifiers.Control:
                        Console.Write(' ');
                        Console.CursorLeft--;
                        Console.CursorLeft--;
                        contInp = false;
                        break;
                    case ConsoleKey.Backspace:
                        if (Console.CursorLeft > 1)
                        {
                            Console.Write(' ');
                            Console.CursorLeft--;
                            if (temp.Length > 0)
                                temp = temp.Remove(temp.Length - 1);
                        }
                        else
                        {
                            Console.CursorLeft++;
                        }
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