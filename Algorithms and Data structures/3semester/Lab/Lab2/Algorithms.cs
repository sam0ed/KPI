using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Lab2.IComparer;

namespace Lab2;

public static class Algorithms
{
    public static (long, long, int) LDFS(State start, int depthConstraint, Action<State, int, int> printer)
    {
        bool solutionFound = false;
        long stepCounter = 0;
        long statesAmount = 1;
        (int y, int x) cursorCoord = (Console.CursorTop, Console.CursorLeft);

        var stack = new Stack<State>();
        stack.Push(start);

        while (stack.Count > 0 && !solutionFound)
        {
            var vertex = stack.Pop();
            stepCounter++;
            printer(vertex, cursorCoord.y, cursorCoord.x);

            if (vertex.Depth < depthConstraint)
            {
                var proceedingStates = vertex.GetProceedingStates();
                statesAmount += proceedingStates.Count;
                foreach (var state in proceedingStates)
                {
                    if (state.IsSolution()) solutionFound = true;

                    stack.Push(state);
                }
            }
        }
        return ( stepCounter, statesAmount, stack.Count);
    }

    public static (long, long, int) AStar(State start, Action<State, int, int> printer)
    {
        bool isSolvable = !Convert.ToBoolean(inversionCount(Program.squishArr(start.Map)) % 2);
        bool solutionFound = false;
        long stepCounter = 0;
        long statesAmount = 1;
        (int y, int x) cursorCoord = (Console.CursorTop, Console.CursorLeft);

        OrderedList<State> lowPriorityQuee = new OrderedList<State>(new HeuristicsComparer());
        lowPriorityQuee.Add(start);

        while (!solutionFound && isSolvable)
        {
            var vertex = lowPriorityQuee.First();
            lowPriorityQuee.RemoveAt(0);
            stepCounter++;
            if (vertex.IsSolution()) solutionFound = true;

            printer(vertex, cursorCoord.y, cursorCoord.x);

            var proceedingStates = vertex.GetProceedingStates();
            foreach (var state in proceedingStates)
            {
                if (!Program.Equal(start.Map, state.Map) &&
                    !lowPriorityQuee.Any(x => Program.Equal(x.Map, state.Map)))
                {
                    lowPriorityQuee.Add(state);
                    statesAmount++;
                }
            }

            start = vertex;
        }

        return (stepCounter, statesAmount, lowPriorityQuee.Count);
    }

    public static int inversionCount(int?[] arr)
    {
        int counter = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (arr[i] != null && arr[j] != null && arr[j] > arr[i]) counter++;
            }
        }

        return counter;
    }
}