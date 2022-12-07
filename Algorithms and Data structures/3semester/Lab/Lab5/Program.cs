using System.Diagnostics;
using System.Text;
using System.Xml;
using Lab5;

static class Program
{
    public static readonly Random Random = new Random();

    public static void Main(string[] args)
    {
        var graph = GraphConfig.GetGraph();
        graph.Print();
        
        GeneticAlgorithm.AlgorithmReinit(graph);
        var result = GeneticAlgorithm.Run(graph);
        Console.Read();
        // AlgorithmTesting();
    }

    public static void AlgorithmTesting()
    {
        var graph = GraphConfig.GetGraph();
        GeneticAlgorithm.AlgorithmReinit(graph);
        var watch = Stopwatch.StartNew();
        StreamWriter writer;
        for (int j = 0; j < 4; j++)
        {
            double? minTimeSoFar = null;
            int? bestParameterIteration = null;
            int? minIterSoFar = null;
            string testedParameterName;
            
            switch (j)
            {
                case 0:
                    testedParameterName = nameof(GeneticAlgorithm.MutationProbability);
                    break;
                case 1:
                    testedParameterName = nameof(GeneticAlgorithm.MaxMutationAddedGenes);
                    break;
                case 2:
                    testedParameterName = nameof(GeneticAlgorithm.MaxImprovementRemovedGenes);
                    break;
                case 3:
                    testedParameterName = nameof(GeneticAlgorithm.MaxInitGenSize);
                    break;
                default:
                    testedParameterName = String.Empty;
                    break;
            }

            writer = new StreamWriter(File.Open(testedParameterName + ".csv", FileMode.Create));
            writer.WriteLine(string.Join(';', testedParameterName,"pathCost", "counter"));

            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");
            Console.WriteLine($"Finding best {testedParameterName}");
            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");
            for (int i = 0; i < 100; i++)
            {
                switch (j)
                {
                    case 0:
                        GeneticAlgorithm.MutationProbability = i / 100.0;
                        writer.Write(GeneticAlgorithm.MutationProbability+";");
                        break;
                    case 1:
                        GeneticAlgorithm.MaxMutationAddedGenes = i;
                        writer.Write(GeneticAlgorithm.MaxMutationAddedGenes+";");
                        break;
                    case 2:
                        GeneticAlgorithm.MaxImprovementRemovedGenes = i;
                        writer.Write(GeneticAlgorithm.MaxImprovementRemovedGenes+";");
                        break;
                    case 3:
                        GeneticAlgorithm.MaxInitGenSize= i+2;
                        writer.Write(GeneticAlgorithm.MaxInitGenSize+";");
                        break;
                }

                Console.Write($"{testedParameterName}:");
                switch (j)
                {
                    case 0:
                        Console.WriteLine($"{GeneticAlgorithm.MutationProbability}");
                        break;
                    case 1:
                        Console.WriteLine($"{GeneticAlgorithm.MaxMutationAddedGenes}");
                        break;
                    case 2:
                        Console.WriteLine($"{GeneticAlgorithm.MaxImprovementRemovedGenes}");
                        break;
                    case 3:
                        Console.WriteLine($"{GeneticAlgorithm.MaxInitGenSize}");
                        break;
                }

                // GeneticAlgorithm.Generation = new List<List<int>?>(savedGeneration);
                GeneticAlgorithm.Generation = GeneticAlgorithm.GetInitialGeneration(graph);
                watch.Restart();
                var shortestPath = GeneticAlgorithm.Run(graph);
                watch.Stop();
                if (minTimeSoFar == null ||
                    (minTimeSoFar > watch.ElapsedMilliseconds && minIterSoFar > shortestPath.counter))
                {
                    minTimeSoFar = watch.ElapsedMilliseconds;
                    bestParameterIteration = i;
                    minIterSoFar = shortestPath.counter;
                }

                Console.WriteLine($"Cost of shortest path is: {GraphConfig.GetPathCost(shortestPath.optima, graph)}");
                shortestPath.optima.Print();
                Console.WriteLine(
                    $"Path was found in {watch.ElapsedMilliseconds} milliseconds in {shortestPath.counter} iterations.");
                Console.WriteLine(
                    $"-----------------------------------------------------------------------------------------");
                
                writer.WriteLine(String.Join(';',GraphConfig.GetPathCost(shortestPath.optima, graph),shortestPath.counter ));
            }
            writer.Close();

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            switch (j)
            {
                case 0:
                    Console.WriteLine(
                        $"The best {testedParameterName} was {bestParameterIteration.Value / 100.0} with time of {minTimeSoFar} and {minIterSoFar} iterations");
                    GeneticAlgorithm.MutationProbability = bestParameterIteration.Value / 100.0;
                    break;
                case 1:
                    Console.WriteLine(
                        $"The best {testedParameterName} was {bestParameterIteration} with time of {minTimeSoFar} and {minIterSoFar} iterations");
                    GeneticAlgorithm.MaxMutationAddedGenes = bestParameterIteration.Value;
                    break;
                case 2:
                    Console.WriteLine(
                        $"The best {testedParameterName} was {bestParameterIteration} with time of {minTimeSoFar} and {minIterSoFar} iterations");
                    GeneticAlgorithm.MaxImprovementRemovedGenes = bestParameterIteration.Value;
                    break;
                case 3:
                    Console.WriteLine(
                        $"The best {testedParameterName} was {bestParameterIteration+2} with time of {minTimeSoFar} and {minIterSoFar} iterations");
                    GeneticAlgorithm.MaxInitGenSize = bestParameterIteration.Value+2;
                    break;
            }

            Console.ResetColor();
            // Console.ReadLine();
        }

        Console.WriteLine(
            $"-----------------------------------------------------------------------------------------");
        Console.WriteLine($"Best algorithm configuration:\n" +
                          $"{nameof(GeneticAlgorithm.MutationProbability)}: {GeneticAlgorithm.MutationProbability}\n" +
                          $"{nameof(GeneticAlgorithm.MaxMutationAddedGenes)}: {GeneticAlgorithm.MaxMutationAddedGenes}\n" +
                          $"{nameof(GeneticAlgorithm.MaxImprovementRemovedGenes)}: {GeneticAlgorithm.MaxImprovementRemovedGenes}\n" +
                          $"{nameof(GeneticAlgorithm.MaxInitGenSize)}: {GeneticAlgorithm.MaxInitGenSize}");
        Console.WriteLine(
            $"-----------------------------------------------------------------------------------------");
        Console.ReadLine();
    }
}