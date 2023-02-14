namespace Lab4
{
    internal class Ant
    {

        public Ant(AntType antType = AntType.Ordinary)
        {
            UnvisitedVertices = Enumerable.Range(0, Config.VerticesAmount).ToList();
            Path = new List<int>();
            Type = antType;

        }
        public enum AntType
        {
            Ordinary,
            FeromoneElite,
        }
        public AntType Type { get; set; }
        public List<int> UnvisitedVertices { get; set; }
        public List<int> Path { get; set; }
        public void MoveToVertix(int transitInd)
        {
            Path.Add(transitInd);
            UnvisitedVertices.Remove(transitInd);
        }
        public void MoveToNext(double[,] visibility, double[,] feromon)
        {
            int transitInd = GetTransitVertixInd(visibility, feromon);
            MoveToVertix(transitInd);

        }
        public int GetTransitVertixInd(double[,] visibility, double[,] feromon)//testing
        {
            int? transitVertixInd = null;

            List<double> transitProbabilities=new List<double>(UnvisitedVertices.Count);; 
            FindTransitProbabilities(ref transitProbabilities, visibility, feromon);
            if (Math.Round(transitProbabilities.Sum()) != 1)
                throw new Exception("Probabilities dont add up to 1");

            var minStr = transitProbabilities.Min().ToString();
            var start = minStr.IndexOf('.');
            int precision = minStr.Substring(start == -1 ? 0 : start).Length;
            double randValue = Config.Random.Next(0, precision) / (double)precision;

            double lowerBound = 0;
            for (int i = 0; i < transitProbabilities.Count; i++)
            {
                if (lowerBound <= randValue && randValue <= lowerBound + transitProbabilities[i])
                {
                    transitVertixInd = UnvisitedVertices[i];
                    i = transitProbabilities.Count;
                }
                else
                    lowerBound += transitProbabilities[i];
            }
            if (transitVertixInd == null) throw new Exception("Transition index search failed");


            return transitVertixInd.Value;
        }
        public List<double> FindTransitProbabilities( ref List<double> adjacentNodeHeuristics, double[,] visibility, double[,] feromon)//testing
        { 
            
            // if (UnvisitedVertices.Count != 1)
            // {
                for (int i = 0; i < UnvisitedVertices.Count; i++)
                {
                    adjacentNodeHeuristics.Add(
                        Math.Pow(feromon[Path.Last(), UnvisitedVertices[i]], Config.Alpha) *
                        Math.Pow(visibility[Path.Last(), UnvisitedVertices[i]], Config.Beta)
                        );
                }
                double adjacentNodeHeuristicSum = adjacentNodeHeuristics.Sum();
                for (int i = 0; i < adjacentNodeHeuristics.Count; i++)
                {
                    if (adjacentNodeHeuristicSum == 0) adjacentNodeHeuristics[i] = 1.0 / adjacentNodeHeuristics.Count;
                    else
                        adjacentNodeHeuristics[i] /= adjacentNodeHeuristicSum;
                }
            // }
            // else adjacentNodeHeuristics.Add(1);

            return adjacentNodeHeuristics;

        }
        public double GetFeromones(int[,] weights)//testing
        {
            if (Path.Count != Config.IterFeromonAdd + 1)
                throw new Exception("Can`t get feromones. Ant hasnt completed the path");
            if (Config.Lmin == null) throw new Exception("Lmin is null");

            int Lk = TspAlgorithm.GetCycleL(Path, weights);

            switch (Type)
            {
                case AntType.Ordinary: return (double)(Config.Lmin) / Lk;
                case AntType.FeromoneElite: return 2*(double)(Config.Lmin) / Lk;
                default: throw new Exception("Ant type is not specified, cant determine Feromones");
            }

        }
        public void PlantFeromon(double feromonAmount, double[,] feromon)
        {
            if (Path.Count != Config.IterFeromonAdd + 1)
                throw new Exception("Can`t get feromones. Ant hasnt completed the path");
            for (int i = 0; i < Path.Count - 1; i++)
            {
                feromon[Path[i], Path[i + 1]] += feromonAmount;
            }
        }
    }
}
