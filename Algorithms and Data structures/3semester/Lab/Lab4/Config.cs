namespace Lab4
{
    internal static class Config
    {
        public static readonly Random Random = new Random();

        public const int VerticesAmount = 100;
        public static readonly (int Min, int Max) WeightRange = (1, 40);

        public const int Alpha = 3;
        public const int Beta = 2;
        public const double Ro = 0.7;

        public const int AntAmount = 45;
        public const int EliteAntAmount = 10;
        public static int? Lmin;
        public const int IterFeromonAdd = VerticesAmount;

        public static void LminInit(int[,] weights) //needed refactoring like in FindTransitProbability
        {
            if (Lmin == null)
            {
                List<int> visited = new List<int>(VerticesAmount);
                visited.Add(0);
                for (int i = 0; i < VerticesAmount-1; i++)
                {
                    int lowestWeightInd = visited.Contains((visited.Last() + 1) % weights.GetLength(1)) ?
                        (visited.Last() + 2) % weights.GetLength(1) :
                        (visited.Last() + 1) % weights.GetLength(1);

                    for (int j = 0; j < weights.GetLength(1); j++)
                    {
                        if (j != visited.Last() && !visited.Contains(j) &&
                            weights[visited.Last(), j] < weights[visited.Last(), lowestWeightInd])
                        {
                            lowestWeightInd = j;
                        }
                    }
                    visited.Add(lowestWeightInd);
                }
                visited.Add(0);

                Lmin=TspAlgorithm.GetCycleL(visited, weights);
                Program.PrintCycle(visited, weights);
            }
            else throw new Exception();
        }
    }
}
