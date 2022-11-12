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
        int?[,] map2 =
        {
            { 8, 6, null, },
            { 5, 4, 3 },
            { 1, 2, 7 }
        };
        
        List<(State init, State? solution, long counter, long avgSavedStates)> run =
            new List<(State, State?, long, long)>(10);
        for (int i = 0; i < run.Capacity; i++)
        {
            var initialState = new State(0,null);
            Console.WriteLine($"Iteration: {i + 1}\n");
            // var result = Algorithms.LDFS(initialState, 10);
            var result = Algorithms.AStar(initialState);
            if (result.Item1 == null)
                Console.WriteLine("Solution couldn`t be found");
            else
            {
                // Console.WriteLine($"-----------------------------------------------------------------------------------------\n" +
                //                   $"Amount of steps to find the solution: {result.Item2}\n" +
                //                   $"Amount of failed solution searches: {0}\n" +
                //                   $"Amount of steps made: {avgStepsAmount}\n" +
                //                   $"Amount of states saved in memory at particular step: {avgSavedStates}\n" +
                //                   $"-----------------------------------------------------------------------------------------\n");
            }
        
            run.Add((initialState, result.Item1, result.Item2, result.Item3)!);
        }
        
        
        var avgStepsForSolution = run.Where(x => x.solution != null).Any()
            ? run.Where(x => x.solution != null).Select(x => x.counter).Average()
            : (double?)null;
        var notFoundSolution = run.Count(x => x.solution == null);
        var avgStepsAmount = run.Select(x => x.counter).Average();
        var avgSavedStates = run.Select(x => x.avgSavedStates).Average();
        
        Console.WriteLine(
            $"-----------------------------------------------------------------------------------------\n" +
            $"Average amount of steps to find the solution: {avgStepsForSolution}\n" +
            $"Amount of failed solution searches: {notFoundSolution}\n" +
            $"Average amount of steps made: {avgStepsAmount}\n" +
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