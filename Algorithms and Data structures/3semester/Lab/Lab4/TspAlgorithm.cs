namespace Lab4
{
    internal static class TspAlgorithm
    {
        public static List<int> AntColonyOptimization(int[,] weights)
        {
            double[,] visibility = new double[Config.VerticesAmount, Config.VerticesAmount];
            double[,] feromon = new double[Config.VerticesAmount, Config.VerticesAmount];

            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    if (i == j) continue;
                    visibility[i, j] = 1 / (double)weights[i, j];
                    feromon[i, j] = Config.Random.Next(1, 4) / 10.0;
                }
            }
            List<int> minCycle = new List<int>(Config.VerticesAmount + 1);
            int prevL = -1;
            int curL;
            int stableLength = 0;
            int counter = 0;
            StreamWriter streamWriter = new StreamWriter(File.Open("plot.csv", FileMode.Create));
            do
            {
                List<Ant> ant = new List<Ant>(Config.AntAmount);
                for (int i = 0; i < Config.AntAmount; i++)
                {
                    Ant.AntType type;
                    type = i < Config.EliteAntAmount ? Ant.AntType.FeromoneElite : Ant.AntType.Ordinary;
                    ant.Add(new Ant(type));

                    //ant travel
                    int plantingIndex = Config.Random.Next(0, Config.VerticesAmount);
                    ant[i].MoveToVertix(plantingIndex);
                    while (ant[i].UnvisitedVertices.Count > 0)
                    {
                        ant[i].MoveToNext(visibility, feromon);
                    }
                    ant[i].MoveToVertix(plantingIndex);

                    if (!minCycle.Any()) minCycle = ant[i].Path;
                    else if (GetCycleL(ant[i].Path, weights) < GetCycleL(minCycle, weights)) minCycle = ant[i].Path;
                }

                for (int i = 0; i < Config.VerticesAmount; i++)
                {
                    for (int j = 0; j < Config.VerticesAmount; j++)
                    {
                        feromon[i, j] -= Config.Ro * feromon[i, j]; //feromon evaporation
                    }
                }
                for (int i = 0; i < Config.AntAmount; i++)
                {
                    double feromonAmount = ant[i].GetFeromones(weights);
                    ant[i].PlantFeromon(feromonAmount, feromon);
                }

                curL = GetCycleL(minCycle, weights);
                if (curL == prevL) stableLength++;
                else
                {
                    Console.WriteLine($"Length changed to: {curL}");
                    stableLength = 0;
                }
                prevL = curL;

                counter++;
                if(counter%20==0)
                    streamWriter.WriteLine(String.Join(';', counter, curL));
                // if(curL<Config.Lmin) Console.WriteLine($"Current unstabilized length: {curL} ");

            }
            while ( /*curL>Config.Lmin && stableLength<150 */ counter<1000 );
            Program.PrintCycle(minCycle, weights);
            streamWriter.Close();

            return minCycle;

        }

        public static int GetCycleL(List<int> cycle, int[,] weights)
        {
            int L = 0;
            for (int i = 0; i < cycle.Count - 1; i++)
            {
                L += weights[cycle[i], cycle[i + 1]];
            }
            return L;
        }

    }
}
