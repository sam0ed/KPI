namespace Lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = BuildGraph();
            Config.LminInit(graph);
            var minCycle = TspAlgorithm.AntColonyOptimization(graph);
        }

        public static void PrintCycle(List<int> cycle, int[,] graph)
        {
            Console.WriteLine("The shortest gamilton cycle contains vertices in order: ");
            cycle.Print();
            Console.WriteLine($"Lmin: {Config.Lmin}\tL: {TspAlgorithm.GetCycleL(cycle, graph)}");
        }

        public static int[,] BuildGraph()
        {
            int[,] graph = new int[Config.VerticesAmount, Config.VerticesAmount];
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (i != j)
                    {
                        graph[i, j] = Config.Random.Next(Config.WeightRange.Min, Config.WeightRange.Max + 1);
                    }
                }
            }

            return graph;
        }
    }
}