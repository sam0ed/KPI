using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Lab2.IComparer;

namespace Lab2;

public static class Algorithms
{
    public static (State?, long, long) LDFS(State start, int depthConstraint)
    {
        State? solution = null;
        long stepCounter = 0;
        long statesAmount = 0;

        var stack = new Stack<State>();
        stack.Push(start);

        while (stack.Count > 0 && solution == null)
        {
            var vertex = stack.Pop();
            stepCounter++;
            statesAmount += stack.Count;
            if (stepCounter % 1_000_000 == 0)
                Console.WriteLine($"Stack count: {stack.Count}\tAmount of steps: {stepCounter}");

            if (vertex.Depth < depthConstraint)
            {
                var proceedingStates = vertex.GetProceedingStates();
                foreach (var state in proceedingStates)
                {
                    if (state.IsSolution())
                    {
                        solution = state;
                        // Console.WriteLine($"Solution found at iteration: {stepCounter}");
                    }

                    stack.Push(state);
                }
            }
        }

        if (stack.Count == 0) Console.WriteLine($"The stack was emptied at iteration: {stepCounter}");
        return (solution, stepCounter, statesAmount / stepCounter);
    }

    public static ( long, long) AStar(State start)
    {
        bool isSolvable = !Convert.ToBoolean(inversionCount(Program.squishArr(start.Map)) % 2);
        State? solution = null;
        long stepCounter = 0;
        long statesAmount = 0;
        OrderedList<State> path = new OrderedList<State>(new DepthComparer());
        (int y, int x) cursorCoord = (Console.CursorTop, Console.CursorLeft);
        StatePrinter printer = new StatePrinter();

        OrderedList<State> lowPriorityQuee = new OrderedList<State>(new HeuristicsComparer());
        lowPriorityQuee.Add(start);

        while (solution == null && isSolvable)
        {
            var vertex = lowPriorityQuee.First();
            lowPriorityQuee.RemoveAt(0);

            path.Clear();
            bool reachedBottom = false;
            State current = vertex;
            while (!reachedBottom)
            {
                State? parent = current.ParentState;
                if (parent == null) reachedBottom=true;
                else path.Add(parent);
                current = parent;
            }
            printer.PrintStates(path, cursorCoord.y, cursorCoord.x);

            stepCounter++;
            statesAmount += lowPriorityQuee.Count;
            // Console.WriteLine($"Stack count: {lowPriorityQuee.Count}\tAmount of steps: {stepCounter}");

            if (vertex.IsSolution())
            {
                solution = vertex;
                Console.WriteLine($"Solution found at iteration: {stepCounter}");
            }

            var proceedingStates = vertex.GetProceedingStates();
            foreach (var state in proceedingStates)
            {
                if (!Program.Equal(start.Map, state.Map) &&
                    !lowPriorityQuee.Any(x => Program.Equal(x.Map, state.Map)))
                    lowPriorityQuee.Add(state);
            }

            start = vertex;
        }

        return ( stepCounter, stepCounter == 0 ? 0 : statesAmount / stepCounter);
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