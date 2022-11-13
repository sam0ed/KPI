using Lab2.IComparer;

namespace Lab2;

internal class StatePrinter
{
    public StatePrinter(string sample)
    {
        stateHeight = sample.Count(c => c == '\n');
        stateWidth = sample.Substring(0, sample.IndexOf('\n')).Length;
    }
    public int? PreviousPathLength { get; set; } = null;
    public const string transitSign = " => "; 
    public int stateHeight { get; set; }
    public int stateWidth { get; set; }

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

    

    public void PrintStates(State state, int paddingY, int paddingX = 0)
    {
        OrderedList<State> states = FindPath(state);
        if (states.Any())
        {
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

    public OrderedList<State> FindPath(State current)
    {
        OrderedList<State> path = new OrderedList<State>(new DepthComparer());
        bool reachedBottom = false;
        path.Add(current);
        State? parent;
        while (!reachedBottom)
        {
            parent = current.ParentState;
            if (parent == null) reachedBottom = true;
            else path.Add(parent);
            current = parent;
        }
        return path;
    }
}