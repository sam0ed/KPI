using Lab5;

static class Program
{
    public static readonly Random random = new Random();

    public static void Main(string[] args)
    {
        var graph = GraphConfig.GetGraph();
        // graph.Print();
        GeneticAlgorithm.AlgorithmReinit(graph);
        // UnitTest.RunTesting(graph);
        var shortestPath=GeneticAlgorithm.Run(graph);
        shortestPath.Print();
    }
}