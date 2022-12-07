using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Lab5;

public static class GeneticAlgorithm
{
    private static int? _startVertexInd;
    private static int? _endVertexInd;

    //can be parametrized
    public static int MaxInitGenSize = 15;
    public static int MaxMutationAddedGenes = 2;
    public static int MaxImprovementRemovedGenes = 2;
    public static double MutationProbability = 1 / 100.0;
    //can be parametrized
    private static int _parentChromosomesAmount = 2;
    private static int _minCrossingOverPointsAmount = _parentChromosomesAmount - 1;
    private static int? _maxCrossingOverPointsAmount = 1;

    public static List<List<int>?>? Generation;

    private const int CrossingOverProhibitedForOpeningGenesAmount = 1;
    private const int CrossingOverProhibitedForClosingGenesAmount = 1;
    private const int CrossingOverMinMiddleGenesAmount = 1;


    private static int? MaxCrossingOverPointsAmount
    {
        get { return _maxCrossingOverPointsAmount; }
        set
        {
            if (value < _minCrossingOverPointsAmount) throw new ArgumentOutOfRangeException();
            _maxCrossingOverPointsAmount = value;
        }
    }

    public static (List<int>? optima, int counter) Run(int?[,] graph)
    {
        Debug.Assert(Generation != null, nameof(Generation) + " != null");

        List<int>? optima = Generation.MinBy(sample => GraphConfig.GetPathCost(sample, graph));
        int counter = 0;
        while (Generation.Distinct().Count() >= _parentChromosomesAmount && GetParents(out var parents,
                   out var possibleCrossingVertexIndices, graph))
        {
            var child = GetChild(parents, possibleCrossingVertexIndices);

            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");
            PrintIteration(counter, parents, child, graph);

            var mutationProbability = Program.Random.Next(0, (int)Math.Pow(10, 6)) / Math.Pow(10, 6);
            if (mutationProbability < MutationProbability)
            {
                child = GetMutatedSample(child, graph);
            }

            try
            {
                child = GetLocallyImproved(child, graph);
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Local improvement failed.");
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

            Console.WriteLine($"Optimal solution with cost {GraphConfig.GetPathCost(optima, graph)}: ");
            optima.Print();
            Console.WriteLine(
                $"-----------------------------------------------------------------------------------------");

            counter++;
        }

        return (optima, counter);
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

    public static bool GetParents(out List<List<int>> parents,
        out List<int> possibleCrossingVertexIndices, int?[,] graph)
    {
        var canSelectParents = true;
        var parentsFound = false;
        var triedParentIndices = new List<int>(Generation.Count(sample =>
            sample.Count > CrossingOverProhibitedForOpeningGenesAmount + CrossingOverProhibitedForClosingGenesAmount));
        do
        {
            var currentParentIndices = GetSelectedIndices(graph);
            if (_parentChromosomesAmount != currentParentIndices.Count) throw new Exception();
            
            int counter = 0;
            parents = Generation.Where(sample => currentParentIndices.Contains(counter++)).ToList();
            IEnumerable<int> temp = parents[0];
            for (int i = 1; i < _parentChromosomesAmount; i++)
            {
                temp = temp.Intersect(parents[i]);
            }

            possibleCrossingVertexIndices = temp.Except(new List<int>()
            {
                parents.First()[CrossingOverProhibitedForOpeningGenesAmount - 1],
                parents.First()[^CrossingOverProhibitedForClosingGenesAmount]
            }).ToList();

            if (possibleCrossingVertexIndices.Count() >= _minCrossingOverPointsAmount)
                parentsFound = true;
            if (Enumerable.Range(0,Generation.Count).All(index => triedParentIndices.Contains(index))) 
                canSelectParents = false;
            triedParentIndices.AddRange(currentParentIndices);
        } while (!parentsFound && canSelectParents);

        return canSelectParents;
    }

    public static List<int>? GetChild(List<List<int>> parents, List<int> possibleCrossingVertexIndices)
    {
        if (possibleCrossingVertexIndices.Count < _minCrossingOverPointsAmount) throw new Exception();
        var possibleCrossingVertexIndicesAsLists =
            possibleCrossingVertexIndices.Select(x => new List<int>() { x }).ToList();
        List<List<int>> possibleCrossingVertexSets = new List<List<int>>();
        for (int i = _minCrossingOverPointsAmount; i <= _maxCrossingOverPointsAmount.Value; i++)
        {
            IEnumerable<List<int>> allPossibleSetsOfSizeI = possibleCrossingVertexIndicesAsLists;
            for (int j = 0; j < i - 1; j++)
            {
                allPossibleSetsOfSizeI = allPossibleSetsOfSizeI.SelectMany(y => possibleCrossingVertexIndicesAsLists,
                    (y, x) => y.Concat(x).ToList());
            }

            allPossibleSetsOfSizeI = allPossibleSetsOfSizeI.Where(set => set.Distinct().Count() == set.Count);
            possibleCrossingVertexSets.AddRange(allPossibleSetsOfSizeI);
        }

        possibleCrossingVertexSets.Reverse();
        List<int>? child = null;
        for (int i = 0; i < possibleCrossingVertexSets.Count && child == null; i++)
        {
            child = GetCrossingOver(possibleCrossingVertexSets[i], parents);
        }

        if (child == null)
        {
            Console.WriteLine(
                $"CrossingOver failed with:" +
                $"minCrossingVertexAmount-{_minCrossingOverPointsAmount}" +
                $"maxCrossingVertexAmount-{_maxCrossingOverPointsAmount}" +
                $"crossing vertices: ");
        }
        return child;
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
        List<int> parents = new List<int>(_parentChromosomesAmount);
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

        if (parents.Count > _parentChromosomesAmount) throw new Exception();
        return parents;
    }

    public static List<double> GetSelectionProbabilities(int?[,] graph)
    {
        if (!Generation.Any()) throw new Exception("Generation is not initialized");

        List<double> sampleSelectionProbability = new(Generation.Count);
        foreach (var sample in Generation)
        {
            if (sample.Count > CrossingOverProhibitedForOpeningGenesAmount +
                CrossingOverProhibitedForClosingGenesAmount)
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

    public static List<int>
        GetCrossingOver(List<int> crossingVertices, List<List<int>> parents) //balanced crossingover
    {
        if (crossingVertices.Count < _minCrossingOverPointsAmount)
            throw new ArgumentException("less crossing vertices then needed were passed");
        if (!parents.All(parent => crossingVertices.All(vertex => parent.Contains(vertex))))
            throw new ArgumentException("parents dont contain crossing vertex");

        crossingVertices.Insert(0, parents[0].First());
        crossingVertices.Insert(crossingVertices.Count, parents[0].Last());
        List<List<List<int>>> splittedParts = new List<List<List<int>>>(crossingVertices.Count - 1);
        for (int i = 0; i < crossingVertices.Count - 1; i++)
        {
            splittedParts.Add(new List<List<int>>());
            for (int j = 0; j < parents.Count; j++)
            {
                int skipInd = parents[j].IndexOf(crossingVertices[i]);
                int takeInd = parents[j].IndexOf(crossingVertices[i + 1]);
                splittedParts[i].Add(parents[j].Skip(skipInd).Take(takeInd - skipInd).ToList());
            }
        }

        List<List<int>> possibleChildren = splittedParts[0];
        for (int i = 1; i < splittedParts.Count; i++)
        {
            List<List<int>> allPossibleCombinationOfIParts = new List<List<int>>();
            for (int j = 0; j < possibleChildren.Count; j++)
            {
                for (int k = 0; k < splittedParts[i].Count; k++)
                {
                    if (k != j)
                        allPossibleCombinationOfIParts.Add(possibleChildren[j].Concat(splittedParts[i][k]).ToList());
                }
            }

            possibleChildren = allPossibleCombinationOfIParts;
        }
        
        possibleChildren = possibleChildren.Distinct().ToList();

        if (!possibleChildren.Any()) throw new ArgumentException("Cant perform crossing over for given parents");
        int returnIndex = Program.Random.Next(0, possibleChildren.Count());
        possibleChildren[returnIndex].Add(parents.Last().Last());
        return possibleChildren[returnIndex];
    }

    public static List<int>? GetMutatedSample(List<int> sample, int?[,] graph)
    {
        for (int k = 0; k < MaxMutationAddedGenes; k++)
        {
            var insertionIndicesCount = sample.Count - CrossingOverProhibitedForOpeningGenesAmount;
            var insertionIndicesOptions = Enumerable.Range(CrossingOverProhibitedForOpeningGenesAmount,
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
            // else Console.WriteLine("Mutation failed at some complexity level");
        }

        return sample;
    }

    public static List<int>? GetLocallyImproved(List<int> sample, int?[,] graph)
    {
        if (sample.Count() != sample.Distinct().Count()) throw new Exception();
        List<int> nodesToRemove = new List<int>();
        for (int i = 0; i < sample.Count - (CrossingOverProhibitedForOpeningGenesAmount+CrossingOverProhibitedForClosingGenesAmount); i++)
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
                    var temp=transitPath.Except(directPath).ToList();
                    if (directPathCost < transitPathCost && temp.Count<=MaxImprovementRemovedGenes && temp.Count>nodesToRemove.Count)
                    {
                        nodesToRemove = temp;
                    }
                }
            }
        }

        // if (!nodesToRemove.Any()) throw new Exception("Local improvement failed(cant shorten path)");

        return sample.Except(nodesToRemove).ToList();
    }

    public static void PrintIteration(int counter, List<List<int>> parents, List<int> child,int?[,] graph )
    {
        Console.WriteLine($"Iteration {counter}:\n");
        for (int i = 0; i < parents.Count; i++)
        {
            Console.WriteLine($"Parent {i} with cost {GraphConfig.GetPathCost(parents[i], graph)}: ");
            parents[i].Print();
        }
        
        Console.WriteLine($"Child with cost {GraphConfig.GetPathCost(child, graph)}: ");
        child.Print();
    }
}