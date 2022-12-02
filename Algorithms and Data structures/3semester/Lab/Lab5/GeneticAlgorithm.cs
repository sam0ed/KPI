using System.Diagnostics;

namespace Lab5;

public static class GeneticAlgorithm
{
    private static int? _startVertexInd;
    private static int? _endVertexInd;
    public static List<List<int>?>? Generation;

    private const int CrossingOverProhibitedForFirstAmount = 1;
    private const int CrossingOverProhibitedForLastAmount = 1;
    private const int CrossingOverMinMiddleVerticesAmount = 1;
    private const int MaxInitGenSize = 15;

    public static List<int>? Run(int?[,] graph)
    {
        Debug.Assert(Generation != null, nameof(Generation) + " != null");

        List<int>? optima = Generation.MinBy(sample => GraphConfig.GetPathCost(sample, graph));
        int counter = 0;
        while (Generation.Distinct().Count() > 1 && GetParents(out var parent0, out var parent1,
                   out var possibleCrossingVertexIndices, graph))
        {
            var child = GetChild(parent0, parent1, possibleCrossingVertexIndices);

            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");
            if (parent0 != null && parent1 != null) PrintIteration(counter, parent0, parent1, child);


            var mutationProbability = Program.Random.Next(0, (int)Math.Pow(10, 6)) / Math.Pow(10, 6);
            if (mutationProbability < 1.0 / child.Count)
            {
                try
                {
                    child = GetMutatedSample(child, graph);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Mutation failed");
                }
            }

            try
            {
                child = GetLocallyImproved(child, graph);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Local improvement failed.");
            }

            bool genContainsSample = false;
            foreach (var initGenSample in Generation)
            {
                if (initGenSample.SequenceEqual(child)) genContainsSample = true;
            }

            if (!genContainsSample)
                Generation.Add(child);
            Generation.Remove(Generation.MaxBy(sample => GraphConfig.GetPathCost(sample, graph)));
            optima = Generation.MinBy(sample => GraphConfig.GetPathCost(sample, graph));

            Console.WriteLine("Optimal solution: ");
            optima.Print();
            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");

            counter++;
        }

        return optima;
    }

    public static bool GetParents(out List<int>? parent0, out List<int>? parent1,
        out List<int> possibleCrossingVertexIndices, int?[,] graph)
    {
        var canSelectParents = true;
        var parentsFound = false;
        var triedParentIndices = new List<int>(Generation.Count(sample => sample.Count > 2));
        do
        {
            var currentParentIndices = GetSelectedIndices(graph);
            parent0 = Generation[currentParentIndices[0]];
            parent1 = Generation[currentParentIndices[1]];
            possibleCrossingVertexIndices =
                parent0.Intersect(parent1).Except(new List<int>() { parent0[0], parent0[^1] })
                    .ToList();

            if (possibleCrossingVertexIndices.Any())
                parentsFound = true;
            if (triedParentIndices.Contains(currentParentIndices[0]) &&
                triedParentIndices.Contains(currentParentIndices[1]))
                canSelectParents = false;
            triedParentIndices.AddRange(currentParentIndices);
        } while (!parentsFound && canSelectParents);

        return canSelectParents;
    }

    public static List<int>? GetChild(List<int>? parent0, List<int>? parent1, List<int> possibleCrossingVertexIndices)
    {
        List<int> triedCrossingVertexIndices = new List<int>();
        List<int>? child = null;
        while (child == null)
        {
            try
            {
                triedCrossingVertexIndices.Add(Program.Random.Next(0, possibleCrossingVertexIndices.Count));
                child = GetCrossingOver(possibleCrossingVertexIndices[triedCrossingVertexIndices.Last()], parent0,
                    parent1);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    $"CrossingOver failed for crossing vertex: {possibleCrossingVertexIndices[triedCrossingVertexIndices.Last()]}");
            }
        }

        return child;
    }

    public static void AlgorithmReinit(int?[,] graph)
    {
        do
        {
            Console.Write("Enter start vertex index (current start vertex is {0}) : ",
                _startVertexInd == null ? "null" : _startVertexInd);
            _startVertexInd = Convert.ToInt32(Console.ReadLine());
            Console.Write($"Enter end vertex index (current end vertex is {_endVertexInd}) : ");
            _endVertexInd = Convert.ToInt32(Console.ReadLine());
        } while (!GraphConfig.AreConnectedVertices(_startVertexInd.Value, _endVertexInd.Value, graph) ||
                 _startVertexInd is < -1 or > GraphConfig.VerticesAmount ||
                 _endVertexInd is < -1 or > GraphConfig.VerticesAmount);

        Generation = GetInitialGeneration(graph);
    }

    public static List<List<int>?>? GetInitialGeneration(int?[,] graph)
    {
        if (!GraphConfig.AreConnectedVertices(_startVertexInd.Value, _endVertexInd.Value, graph))
            throw new ArgumentException("Start and end vertices are not connected");

        List<List<int>>? initialGeneration = new List<List<int>>(MaxInitGenSize);
        for (int i = 0; i < MaxInitGenSize; i++)
        {
            var sampleToBeAdded = GraphConfig.GetRandPath(_startVertexInd.Value, _endVertexInd.Value, graph);
            bool initGenContainsSample = false;
            foreach (var initGenSample in initialGeneration)
            {
                if (initGenSample.SequenceEqual(sampleToBeAdded)) initGenContainsSample = true;
            }

            if (!initGenContainsSample)
                initialGeneration.Add(sampleToBeAdded);
        }

        return initialGeneration;
    }

    public static List<int> GetSelectedIndices(int?[,] graph)
    {
        List<int> parents = new List<int>(2);
        var selectionProbabilities = GetSelectionProbabilities(graph);
        while (parents.Count != parents.Capacity)
        {
            int maxValue = (int)Math.Pow(10, 6);
            double randValue = Program.Random.Next(0, maxValue) / (double)maxValue;

            double lowerBound = 0;
            for (int j = 0; j < selectionProbabilities.Count; j++)
            {
                if (lowerBound <= randValue && randValue <= lowerBound + selectionProbabilities[j] &&
                    !parents.Contains(j))
                {
                    parents.Add(j);
                    j = selectionProbabilities.Count;
                }
                else
                    lowerBound += selectionProbabilities[j];
            }
        }

        if (parents.Count > 2) throw new Exception();
        return parents;
    }

    public static List<double> GetSelectionProbabilities(int?[,] graph)
    {
        if (!Generation.Any()) throw new Exception("Generation is not initialized");

        List<double> sampleSelectionProbability = new(Generation.Count);
        foreach (var sample in Generation)
        {
            if (sample.Count > CrossingOverProhibitedForFirstAmount + CrossingOverProhibitedForLastAmount)
                sampleSelectionProbability.Add(1.0 / GraphConfig.GetPathCost(sample, graph));
            else
                sampleSelectionProbability.Add(0);
        }

        var generationFitness = sampleSelectionProbability.Sum();
        for (int i = 0; i < Generation.Count; i++)
        {
            sampleSelectionProbability[i] /= generationFitness;
        }

        if (1 - sampleSelectionProbability.Sum() > 1.0 / 100_000)
            throw new Exception("probabilities dont add up to 1");
        return sampleSelectionProbability;
    }

    public static List<int> GetCrossingOver(int crossingVertex, params List<int>[] parents) //balanced crossingover
    {
        if (parents.Length > 2) throw new ArgumentException("more then 2 parents passed");
        if (parents.Count(parent => !parent.Contains(crossingVertex)) != 0)
            throw new ArgumentException("parents dont contain crossing vertex");

        List<List<int>> firstParts = new List<List<int>>(parents.Length);
        List<List<int>> secondParts = new List<List<int>>(parents.Length);
        for (int i = 0; i < parents.Length; i++)
        {
            firstParts.Add(parents[i].Take(parents[i].IndexOf(crossingVertex)).ToList());
            secondParts.Add(parents[i].Skip(parents[i].IndexOf(crossingVertex)).ToList());
        }

        List<List<int>> possibleChildren = new List<List<int>>();
        for (int i = 0; i < firstParts.Count; i++)
        {
            for (int j = 0; j < secondParts.Count; j++)
            {
                if (!firstParts[i].Intersect(secondParts[j]).Any() && i != j)
                {
                    possibleChildren.Add(firstParts[i].Concat(secondParts[j]).ToList());
                }
            }
        }

        if (!possibleChildren.Any()) throw new ArgumentException("Cant perform crossing over for given parents");
        return possibleChildren[Program.Random.Next(0, possibleChildren.Count)];
    }

    public static List<int>? GetMutatedSample(List<int> sample, int?[,] graph)
    {
        var insertionIndicesCount = sample.Count - CrossingOverProhibitedForFirstAmount;
        var insertionIndicesOptions = Enumerable.Range(CrossingOverProhibitedForFirstAmount,
            insertionIndicesCount).OrderBy(index => Program.Random.Next(0, insertionIndicesCount)).ToList();
        var insertionOptions = new List<int>(insertionIndicesCount);
        int? insertionIndex = null;
        for (int i = 0; i < insertionIndicesCount; i++)
        {
            var outgoingFromPrev = GraphConfig.GetOutgoingFrom(sample[insertionIndicesOptions[i] - 1], graph);
            for (int j = 0; j < outgoingFromPrev.Count; j++)
            {
                if (GraphConfig.GetOutgoingFrom(outgoingFromPrev[j], graph)
                        .Contains(sample[insertionIndicesOptions[i]]) &&
                    !sample.Contains(outgoingFromPrev[j]))
                {
                    insertionOptions.Add(outgoingFromPrev[j]);
                }
            }

            if (insertionOptions.Any())
            {
                insertionIndex = insertionIndicesOptions[i];
                i = insertionIndicesCount;
            }
        }

        if (insertionIndex != null)
            sample.Insert(insertionIndex.Value, insertionOptions[Program.Random.Next(0, insertionOptions.Count)]);
        else throw new Exception();
        return sample;
    }

    public static List<int>? GetLocallyImproved(List<int> sample, int?[,] graph)
    {
        if (sample.Count() != sample.Distinct().Count()) throw new Exception();
        List<int> nodesToRemove = new List<int>();
        for (int i = 0; i < sample.Count - 2; i++)
        {
            var currentOutgoing = GraphConfig.GetOutgoingFrom(sample[i], graph);
            for (int j = i + 2; j < sample.Count && !nodesToRemove.Any(); j++)
            {
                if (currentOutgoing.Contains(sample[j]))
                {
                    var transitPath = sample.Skip(i).Take(j - i + 1).ToList();
                    var directPath = new List<int>() { sample[i], sample[j] };
                    var transitPathCost = GraphConfig.GetPathCost(transitPath, graph);
                    var directPathCost = GraphConfig.GetPathCost(directPath, graph);
                    if (directPathCost < transitPathCost)
                    {
                        nodesToRemove = transitPath.Except(directPath).ToList();
                    }
                }
            }
        }

        if (!nodesToRemove.Any()) throw new Exception("Local improvement failed(cant shorten path)");

        return sample.Except(nodesToRemove).ToList();
    }

    public static void PrintIteration(int counter, List<int> parent0, List<int> parent1, List<int> child)
    {
        Console.WriteLine($"Iteration {counter}:\n");
        Console.WriteLine("Parent 0: ");
        parent0.Print();
        Console.WriteLine("Parent 1: ");
        parent1.Print();
        Console.WriteLine("Child: ");
        child.Print();
    }
}