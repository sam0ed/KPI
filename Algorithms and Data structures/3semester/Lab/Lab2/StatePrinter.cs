namespace Lab2;

internal class StatePrinter
{
    public int? PreviousPathLength { get; set; } = null;

    public void PrintInLine(int paddingY, int paddingX, int width, string filling)
    {
        Console.SetCursorPosition(paddingX, paddingY);
        int fillingIndex;
        string printedString = String.Empty;
        for (int i = 0; i < width; i++)
        {
            fillingIndex = i % filling.Length;
            printedString += filling[fillingIndex];
        }

        if (printedString.Length <= Console.WindowWidth)
            Console.Write(printedString);
        else throw new ArgumentException();
    }

    public void PrintStates(OrderedList<State> states, int paddingY, int paddingX = 0)
    {
        if (states.Any())
        {
            string stateStr = states[0].ToString();
            int stateHeight = stateStr.Count(c => c == '\n');
            int stateWidth = stateStr.Substring(0, stateStr.IndexOf('\n')).Length;

            string transitSign = " => ";

            if (PreviousPathLength != null)
            {
                int linesToRemove =
                    (int)Math.Ceiling((double)(((int)PreviousPathLength * (stateWidth + transitSign.Length))) /
                                      Console.WindowWidth);
                for (int i = 0; i < linesToRemove; i++)
                {
                    for (int j = 0; j < stateHeight; j++)
                    {
                        PrintInLine(paddingY + i * (stateHeight + 1) + j, 0, Console.WindowWidth, " ");
                    }
                }

                Console.SetCursorPosition(paddingX, paddingY);
            }

            // int linesToWrite=(int)Math.Ceiling((double)(((int)states.Count * (stateWidth + transitSign.Length))) /
            //                                    Console.WindowWidth);
            int blocksInOneLine = (int)Math.Floor((double)Console.WindowWidth / (stateWidth + transitSign.Length)) - 1;

            for (int i = 0; i < states.Count; i++)
            {
                var rows = states[i].ToString().Split('\n');
                // (int Y, int X) cursorCoord = (Console.CursorTop, Console.CursorLeft);
                (int Y, int X) cursorCoord = (
                    paddingY + (stateHeight + 1) * (int)Math.Floor((double)i / blocksInOneLine),
                    paddingX + (i % blocksInOneLine) * (stateWidth + transitSign.Length)
                );
                for (int j = 0; j < rows.Length; j++)
                {
                    PrintInLine(cursorCoord.Y, cursorCoord.X, rows[j].Length, rows[j]);
                    cursorCoord.Y++;
                }


                Console.SetCursorPosition(
                    paddingX + stateWidth + (i % blocksInOneLine) * (stateWidth + transitSign.Length),
                    paddingY + (stateHeight + 1) * (int)Math.Floor((double)i / blocksInOneLine));
                Console.Write(transitSign);
            }

            PreviousPathLength = states.Count;
            Console.SetCursorPosition(0, Console.CursorTop + stateHeight);

            // if (PreviousPathLength != null)
            // {
            //     var statesToRemove = (int)PreviousPathLength - states.Count;
            //     var widthToRemove = statesToRemove * (stateWidth + transitSign.Length);
            //     int removalAmount =
            //         (int)Math.Ceiling((double)(Console.CursorLeft + widthToRemove / Console.WindowWidth));
            //     for (int i = 0; i < removalAmount; i++)
            //     {
            //         int toRemove = widthToRemove > Console.WindowWidth ? Console.WindowWidth : widthToRemove;
            //         (int Y,int X) cursorCoord=(Console.CursorTop, Console.CursorLeft);
            //         for (int j = 0; j < stateHeight; j++)
            //         {
            //             PrintInLine(cursorCoord.Y,cursorCoord.X, toRemove
            //                 , String.Empty);
            //             widthToRemove -= toRemove;
            //         }
            //         Console.SetCursorPosition();
            //     }
            // }
            //
        }
    }
}