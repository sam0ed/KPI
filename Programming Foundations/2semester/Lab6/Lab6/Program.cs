namespace Lab6 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fileName = "SomeProgram";
            Console.WriteLine("Enter the code of program in C/C++ (do not create identifiers with the same name but different nested level) :");
            List<string> input = new List<string>(ReadMultilineInp());
            Console.WriteLine();
            using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (string line in input)
                {
                    writer.WriteLine(line);
                }
            }

            List<string> identifyerStartDef = new List<string>
            {
                "int ",
                "long ",
                "double ",
                "float ",
                "bool ",
                "char ",
                "string ",
                "auto ",
                "void "
            };
            List<string> identifyerEndDef = new List<string> { "=", ";", "(", };

            List<string> typesStartDef = new List<string> { "class", "struct", "enum", };
            List<string> CustomTypes = GetSqueezedStr(input, typesStartDef, new List<string>());
            foreach (var item in CustomTypes)
            {
                Console.WriteLine($"Type: {item}");
            }

            //adding custom types to types
            identifyerStartDef.AddRange(CustomTypes);

            //adding pointers to types
            int ptrDimensAbleToRead = 5;
            int identifyerStartDefLength = identifyerStartDef.Count;
            for (int i = 0; i < identifyerStartDefLength; i++)
            {
                for (int j = 0; j < ptrDimensAbleToRead; j++)
                {

                    identifyerStartDef.Add(identifyerStartDef[i] + new String('*', j) + " ");
                }
            }

            //getting all identifyers in program
            List<string> allIdentifyers = GetSqueezedStr(new List<string>(input), identifyerStartDef, identifyerEndDef);
            foreach (var item in allIdentifyers)
            {
                Console.WriteLine($"Identifyer: {item}");
            }


            //Dictionary<int, List<string>> treeSource = GetLineIdntifyerPairs(input, allIdentifyers);
            //foreach (var item in treeSource)
            //{
            //    Console.Write($"String number: {item.Key}\tIdentifyers: ");
            //    foreach (var str in item.Value)
            //    {
            //        Console.Write(str + " ");
            //    }
            //    Console.WriteLine();
            //}

            //BinaryNode root = BuildTree(treeSource);

            //for (int i = 0; i < treeSource.Count; i++)
            //{
            //    Console.Write($"String number: {root.Key}\tIdentifyers: ");
            //    foreach (var str in root.Value)
            //    {
            //        Console.Write(str + " ");
            //    }
            //    Console.WriteLine();
            //    root.RemoveNode(root.Key);
            //}

            /////////////////////////////////////////////////////////////////////
            BinaryNode root = GetLineIdentifyerTree(input, allIdentifyers);
            //while (root.RightChild != null || root.LeftChild != null)
            //{
            //    Console.Write($"String number: {root.Key}\tIdentifyers: ");
            //    foreach (var str in root.Value)
            //    {
            //        Console.Write(str + " ");
            //    }
            //    Console.WriteLine();
            //    root.RemoveNode((int)root.Key);
            //}
            List<BinaryNode> ascendingNodes = new List<BinaryNode>();
            GetTreeNodesAscending(root, ascendingNodes);
            for (int i = 0; i < ascendingNodes.Count; i++)
            {
                Console.Write($"String number: {ascendingNodes[i].Key}\tIdentifyers: ");
                foreach (var str in ascendingNodes[i].Value)
                {
                    Console.Write(str + " ");
                }
                Console.WriteLine();
            }
            /////////////////////////////////////////////////////////////////////




            //PrintTree(root);

        }

        public static void GetTreeNodesAscending(BinaryNode root, List<BinaryNode> ascendingNodes)
        {
            if (root.LeftChild != null)
            {
                GetTreeNodesAscending(root.LeftChild, ascendingNodes);
            }
            ascendingNodes.Add(root);
            if(root.RightChild != null)
            {
                GetTreeNodesAscending(root.RightChild, ascendingNodes);
            }

        }

        private static BinaryNode GetLineIdentifyerTree(List<string> input, List<string> allIdentifyers)
        {
            BinaryNode root = new BinaryNode();
            allIdentifyers = new List<string>(allIdentifyers.OrderByDescending(p => p.Length).Select(p => p.Trim()));
            for (int i = 0; i < input.Count; i++)
            {
                input[i] = input[i].Trim();

                for (int j = 0; j < allIdentifyers.Count(); j++)
                {
                    int index;
                    string temp = input[i];
                    while ((index = temp.IndexOf(allIdentifyers[j].Trim())) != -1)
                    {
                        bool prevCharIsLetter = false;
                        bool nextCharIsLetter = false;

                        char? prevChar = null;
                        char? nextChar = null;
                        if (index - 1 >= 0)
                        {
                            prevChar = temp[index - 1];
                            if ((65 <= prevChar && prevChar <= 90) || (97 <= prevChar && prevChar <= 122))
                                prevCharIsLetter = true;
                        }
                        if (index + allIdentifyers[j].Length < temp.Length)
                        {
                            nextChar = temp[index + allIdentifyers[j].Length];
                            if ((65 <= nextChar && nextChar <= 90) || (97 <= nextChar && nextChar <= 122))
                                nextCharIsLetter = true;
                        }

                        if ((!prevCharIsLetter || prevChar == null) && (!nextCharIsLetter || nextChar == null))
                        {
                            if (root.GetNode(i + 1) != null)
                            {
                                if (!root.GetNode(i + 1).Value.Contains(allIdentifyers[j]))
                                    root.GetNode(i + 1).Value.Add(allIdentifyers[j]);
                            }
                            else
                            {
                                root.AddNode(new BinaryNode(i + 1, new List<string> { allIdentifyers[j] }));
                            }
                            input[i] = input[i].Remove(index, allIdentifyers[j].Length);
                            temp = input[i];
                        }
                        else
                        {
                            temp = temp.Substring(index + allIdentifyers[j].Length);
                        }

                    }
                }
            }
            return root;
        }

        //private static BinaryNode BuildTree(Dictionary<int, List<string>> treeSource)
        //{
        //    var treeKeys = treeSource.Keys;
        //    int rootKey = treeKeys.ElementAt(treeKeys.Count/2);
        //    BinaryNode root = new BinaryNode(rootKey, treeSource[rootKey]);
        //    for (int i = 0; i < treeKeys.Count; i++)
        //    {
        //        int currentKey = treeKeys.ElementAt(i);
        //        if (currentKey!=rootKey)
        //        {

        //            root.AddNode(new BinaryNode(currentKey, treeSource[currentKey]));
        //        }
        //    }
        //    return root;
        //}

        //private static Dictionary<int, List<string>> GetLineIdntifyerPairs(List<string> input, List<string> allIdentifyers)
        //{
        //    Dictionary<int, List<string>> result = new Dictionary<int, List<string>>();
        //    allIdentifyers = new List<string>(allIdentifyers.OrderByDescending(p => p.Length).Select(p=>p.Trim()));
        //    for (int i = 0; i < input.Count; i++)
        //    {
        //        input[i] = input[i].Trim();

        //        for (int j = 0; j < allIdentifyers.Count(); j++)
        //        {
        //            int index;
        //            string temp = input[i];
        //            while ((index = temp.IndexOf(allIdentifyers[j].Trim())) != -1)
        //            {
        //                bool prevCharIsLetter = false;
        //                bool nextCharIsLetter = false;

        //                char? prevChar = null;
        //                char? nextChar = null;
        //                if (index - 1 >= 0)
        //                {
        //                    prevChar = temp[index - 1];
        //                    if ((65 <= prevChar && prevChar <= 90) || (97 <= prevChar && prevChar <= 122))
        //                        prevCharIsLetter = true;
        //                }
        //                if (index + allIdentifyers[j].Length < temp.Length)
        //                {
        //                    nextChar = temp[index + allIdentifyers[j].Length];
        //                    if ((65 <= nextChar && nextChar <= 90) || (97 <= nextChar && nextChar <= 122))
        //                        nextCharIsLetter = true;
        //                }

        //                if ((!prevCharIsLetter || prevChar == null) && (!nextCharIsLetter || nextChar == null))
        //                {
        //                    if (result.ContainsKey(i+1))
        //                    {
        //                        if (!result[i+1].Contains(allIdentifyers[j]))
        //                            result[i+1].Add(allIdentifyers[j]);
        //                    }
        //                    else
        //                    {
        //                        result.Add(i+1, new List<string> { allIdentifyers[j] });
        //                    }
        //                    input[i]=input[i].Remove(index, allIdentifyers[j].Length);
        //                    temp = input[i];
        //                }
        //                else
        //                {
        //                    temp = temp.Substring(index+ allIdentifyers[j].Length);
        //                }

        //            }
        //        }
        //    }
        //    return result;
        //}


        private static List<string> GetSqueezedStr(List<string> input, List<string> StartDef, List<string> EndDef)
        {
            List<string> identifyerList = new List<string>();
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Length > 0)
                {
                    bool LineContainsIdentifyer = false;
                    for (int j = 0; j < StartDef.Count; j++)
                    {
                        int index;
                        string newTrimmedStr;
                        while ((index = input[i].IndexOf(StartDef[j])) != -1)
                        {
                            newTrimmedStr = input[i].Substring(index + StartDef[j].Length).Trim();
                            input[i] = newTrimmedStr;

                            LineContainsIdentifyer = true;
                        }
                    }
                    if (LineContainsIdentifyer)
                    {

                        List<string> sameLineIdentifyerList = new List<string>(input[i].Split(","));

                        for (int j = 0; j < sameLineIdentifyerList.Count; j++)
                        {
                            int topTrim = sameLineIdentifyerList[j].Length;
                            for (int k = 0; k < EndDef.Count; k++)
                            {
                                int index = sameLineIdentifyerList[j].IndexOf(EndDef[k]);
                                topTrim = (topTrim > index && index != -1) ? index : topTrim;
                            }
                            sameLineIdentifyerList[j] = sameLineIdentifyerList[j].Remove(topTrim).Trim();

                            bool foundSameIdenifyer = false;
                            for (int k = 0; k < identifyerList.Count; k++)
                            {
                                if (identifyerList[k] == sameLineIdentifyerList[j])
                                    foundSameIdenifyer = true;
                            }
                            if (!foundSameIdenifyer && sameLineIdentifyerList[j] != "")
                            {
                                identifyerList.Add(sameLineIdentifyerList[j] + " ");
                            }
                        }
                    }
                }
            }
            return identifyerList;
        }

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