namespace Lab5;

public class UnitTest
{
    public static void RunTesting(int? [,] graph)
    {
        GetMutatedSampleTest(graph);
    }
    public static void GetLocallyImprovedTest(int?[,] graph)
    {
        Console.WriteLine("Before: ");
        GeneticAlgorithm.Generation[0].Print();
        Console.WriteLine(GraphConfig.GetPathCost(GeneticAlgorithm.Generation[0], graph));
        var improved = GeneticAlgorithm.GetLocallyImproved(GeneticAlgorithm.Generation[0], graph);
        Console.WriteLine("After: ");
        improved.Print();
        Console.WriteLine(GraphConfig.GetPathCost(improved, graph));
    }

    public static void GetMutatedSampleTest(int?[,] graph)
    {
        Console.WriteLine("Before: ");
        GeneticAlgorithm.Generation[0].Print();
        Console.WriteLine("After: ");
        GeneticAlgorithm.GetMutatedSample(GeneticAlgorithm.Generation[0],graph);
    }
}