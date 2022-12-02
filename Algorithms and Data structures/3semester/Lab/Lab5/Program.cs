using Lab5;

static class Program
{
    public static readonly Random Random = new Random();

    public static void Main(string[] args)
    {
        var graph = GraphConfig.GetGraph();
        graph.Print();
        
        GeneticAlgorithm.AlgorithmReinit(graph);
        var shortestPath=GeneticAlgorithm.Run(graph);
        Console.WriteLine($"Cost of shortest path is: {GraphConfig.GetPathCost(shortestPath, graph)}");
        Console.ReadLine();
    }
}