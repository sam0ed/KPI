using Lab2;

class Program
{
    public static void Main(string[] args)
    {
        int?[,] map1 =
        {
            { 1, 6, 2 },
            { 7, 4, 3 },
            { 5, null, 8 }
        };

        List<(State init, long counter, long deadEnd, long savedStates, int savedStatesInMemory)> run =
            new List<(State, long, long, long, int)>(2);
        StatePrinter printer = new StatePrinter(new State(0).ToString());
        for (int i = 0; i < run.Capacity; i++)
        {
            Console.WriteLine($"Iteration: {i + 1}");

            // var initialState = State.GenerateEasySolvableState();
            // var result = Algorithms.LDFS(initialState, 7, printer.PrintStates);
            var initialState = new State(0, null);
            var result = Algorithms.AStar(initialState, printer.PrintStates);
            run.Add((initialState, result.Item1, result.Item2, result.Item3, result.Item4)!);

            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");
            Console.WriteLine($"Amount of steps to find the solution: {run[i].counter}\n" +
                              $"Amount of dead ends: {run[i].deadEnd}\n" +
                              $"Amount of states: {run[i].savedStates}\n" +
                              $"Amount of states saved in memory: {run[i].savedStatesInMemory}");
            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");
        }


        var avgStepsForSolution = run.Where(x => x.counter != 0).Any()
            ? run.Where(x => x.counter != 0).Select(x => x.counter).Average()
            : (double?)null;
        var notFoundSolution = run.Count(x => x.counter == 0);
        var avgSavedStates = run.Select(x => x.savedStates).Average();
        var avgSavedStatesInMemory = run.Select(x => x.savedStatesInMemory).Average();

        Console.WriteLine(
            $"-----------------------------------------------------------------------------------------\n" +
            $"Average amount of steps to find the solution: {avgStepsForSolution}\n" +
            $"Amount of failed solution searches: {notFoundSolution}\n" +
            $"Average amount of steps made: {avgSavedStates}\n" +
            $"Average amount of states saved in memory at particular step: {avgSavedStates}\n" +
            $"-----------------------------------------------------------------------------------------\n");
    }

    public static int?[] squishArr(int?[,] arr)
    {
        int?[] res = new int?[arr.GetLength(0) * arr.GetLength(1)];
        int counter = 0;
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                res[counter++] = arr[i, j];
            }
        }

        return res;
    }

    public static bool Equal(int?[,] arr1, int?[,] arr2)
    {
        bool equal = true;
        if (arr1.GetLength(0) == arr2.GetLength(0))
        {
            for (int i = 0; i < arr1.GetLength(0); i++)
            {
                if (arr1.GetLength(1) == arr2.GetLength(1))
                {
                    for (int j = 0; j < arr1.GetLength(1); j++)
                    {
                        if (arr1[i, j] != arr2[i, j]) equal = false;
                    }
                }
            }
        }

        return equal;
    }
}