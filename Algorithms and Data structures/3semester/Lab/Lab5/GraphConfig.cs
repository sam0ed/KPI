namespace Lab5;

public static class GraphConfig
{
    public const int VerticesAmount = 20;
    public static readonly (int Min, int Max) WeightRange = (5, 150);
    public static readonly (int Min, int Max) VertexPower = (1, 10);

    public static int?[,] GetGraph()
    {
        if (VerticesAmount < VertexPower.Max)
            throw new ArgumentException("amount of vertices is less then vertex power");

        int?[,] graph = new int?[VerticesAmount, VerticesAmount];
        List<int> connections = new List<int>();

        for (int row = 0; row < VerticesAmount; row++)
        {
            connections.Capacity = Program.Random.Next(VertexPower.Min, VertexPower.Max + 1) + 1;
            connections.Add(row);
            for (int col = 0; col < connections.Capacity - 1; col++)
            {
                int colIndex;
                do
                {
                    colIndex = Program.Random.Next(0, VerticesAmount);
                    if (!connections.Contains(colIndex))
                    {
                        graph[row, colIndex] = Program.Random.Next(WeightRange.Min, WeightRange.Max + 1);
                    }
                } while (connections.Contains(colIndex));

                connections.Add(colIndex);
            }

            connections.Clear();
        }

        return graph;
    }

    public static List<int> GetIngoingTo(int index, int?[,] graph)
    {
        List<int> ingoing = new List<int>();
        for (int i = 0; i < VerticesAmount; i++)
        {
            if (graph[i, index] != null) ingoing.Add(i);
        }

        return ingoing;
    }

    public static List<int> GetOutgoingFrom(int index, int?[,] graph)
    {
        List<int> outgoing = new List<int>();
        for (int i = 0; i < VerticesAmount; i++)
        {
            if (graph[index, i] != null) outgoing.Add(i);
        }

        return outgoing;
    }

    public static List<List<int>> GetConnectedComponents(int?[,] graph)
    {
        List<List<int>> components = new List<List<int>>();
        List<int> unvisited = Enumerable.Range(0, GraphConfig.VerticesAmount).ToList();
        int componentInd = 0;
        int vertexInd;
        while (unvisited.Any())
        {
            components.Add(new List<int>());
            components[componentInd].Add(unvisited[0]);
            unvisited.Remove(unvisited[0]);
            vertexInd = 0;
            while (vertexInd < components[componentInd].Count)
            {
                var adjacent = GetOutgoingFrom(components[componentInd][vertexInd], graph);
                for (int i = 0; i < adjacent.Count; i++)
                {
                    if (!components[componentInd].Contains(adjacent[i]))
                    {
                        components[componentInd].Add(adjacent[i]);
                        unvisited.Remove(adjacent[i]);
                    }
                }

                vertexInd++;
            }

            componentInd++;
        }

        return components;
    }

    public static bool AreConnectedVertices(int vertex1, int vertex2, int?[,] graph)
    {
        bool areConnected = false;
        var components = GetConnectedComponents(graph);
        foreach (var component in components)
        {
            if (component.Contains(vertex1) && component.Contains(vertex2))
                areConnected = true;
        }

        return areConnected;
    }

    public static List<int> GetRandPath(int startInd, int endInd, int?[,] graph)
    {
        List<int> path = new List<int>();
        List<int> visited = new List<int>();
        path.Add(startInd);
        visited.Add(startInd);

        while (path.Last() != endInd)
        {
            List<int> adjacent = GraphConfig.GetOutgoingFrom(path.Last(), graph);
            bool success = false;
            do
            {
                var rand = Program.Random.Next(0, adjacent.Count);
                if (!visited.Contains(adjacent[rand]))
                {
                    visited.Add(adjacent[rand]);
                    path.Add(adjacent[rand]);
                    success = true;
                }

                adjacent.Remove(adjacent[rand]);
            } while (!success && adjacent.Any());

            if (!success)
                path.Remove(path.Last());
        }

        if (!path.Any()) throw new ArgumentException("cant find rand path for some reason");
        if (path.Distinct().Count() != path.Count) throw new ArgumentException("path contains duplicates");
        return path;
    }

    public static int GetPathCost(List<int>? path, int?[,] graph)
    {
        int pathCost = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            pathCost += graph[path[i], path[i + 1]]!.Value;
        }

        return pathCost;
    }
}