using System.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace Lab2;

public class State
{
    public State(int depth, State? parentState=null, int?[,]? map = null, (int, int)? emptyEntryCoord = null)
    {
        Depth = depth;
        ParentState = parentState;
        Heuristic = AStarHeuristic;
        IsSolution = AStarIsSolution;
        if (map == null)
        {
            Map = new int?[3, 3];
            var rand = new Random();
            List<int?> tiles = new List<int?>() { 1, 2, 3, 4, 5, 6, 7, 8, null };
            int? randTile;

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    randTile = tiles[rand.Next(0, tiles.Count)];
                    tiles.RemoveAll(tile => tile == randTile);
                    Map[i, j] = randTile;
                }
            }
        }
        else Map = map;

        if (emptyEntryCoord == null)
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == null) EmptyEntryCoord = (i, j);
                }
            }
        }
        else EmptyEntryCoord = ((int y, int x))emptyEntryCoord;
    }

    public State? ParentState { get; set; }
    public int?[,] Map { get; init; }
    public (int y, int x) EmptyEntryCoord { get; init; }
    public int Depth { get; init; }
    public Func<int> Heuristic { get; set; }
    public Func<bool> IsSolution { get; set; }

    private static readonly int?[,] SolvedState =
    {
        { 1, 2, 3 },
        { 4, 5, 6 },
        { 7, 8, null }
    };
    
    public List<State> GetProceedingStates()
    {
        List<State> proceedingStates = new List<State>();
        int[] added = new int[2];
        for (int i = -1; i < 2; i += 2)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < added.Length; k++)
                {
                    added[k] = 0;
                }

                added[j] = i;

                (int y, int x) swapTileCoord = (EmptyEntryCoord.y + added[0], EmptyEntryCoord.x + added[1]);
                if (swapTileCoord.y < Map.GetLength(0) && swapTileCoord.y >= 0 &&
                    swapTileCoord.x < Map.GetLength(1) && swapTileCoord.x >= 0)
                {
                    int?[,] newStateMap = Map.Clone() as int?[,]; // do normal object cloning
                    newStateMap[EmptyEntryCoord.y, EmptyEntryCoord.x] = newStateMap[swapTileCoord.y, swapTileCoord.x];
                    newStateMap[swapTileCoord.y, swapTileCoord.x] = null;
                    proceedingStates.Add(new State(Depth + 1, this, newStateMap, swapTileCoord));
                }
            }
        }

        return proceedingStates;
    }

    public override string ToString()
    {
        string result = String.Empty;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                result += (Map[i, j] != null ? Map[i, j] : "N") + ", ";
            }

            result += "\n";
        }

        return result;
    }

    public bool PositionBasedIsSolution()
    {
        return this.Heuristic() == 9;
    }

    public int PositionBasedHeuristic()
    {
        int score = 0;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] == SolvedState[i, j]) score++;
            }
        }

        return score;
    }

    public bool AStarIsSolution()
    {
        bool isSolution = true;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (GetManhattenDistance(i, j) != 0) isSolution = false;
            }
        }

        return isSolution;
    }
    public int AStarHeuristic()
    {
        int score = Depth;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                score += GetManhattenDistance(i, j);
            }
        }

        return score;
    }

    public int GetManhattenDistance(int tileMapY, int tileMapX)
    {
        int? tileSolutionY = null, tileSolutionX = null;
        for (int i = 0; i < SolvedState.GetLength(0); i++)
        {
            for (int j = 0; j < SolvedState.GetLength(1); j++)
            {
                if (SolvedState[i, j] == Map[tileMapY, tileMapX])
                {
                    tileSolutionY = i;
                    tileSolutionX = j;
                }
            }
        }

        if (tileSolutionX == null || tileSolutionY == null) throw new InvalidDataException();
        return Math.Abs(tileMapY - (int)tileSolutionY) + Math.Abs(tileMapX - (int)tileSolutionX);
    }

    public static bool operator <(State st1, State st2)
    {
        return st1.Heuristic() < st2.Heuristic();
    }

    public static bool operator >(State st1, State st2)
    {
        return st1.Heuristic() > st2.Heuristic();
    }

    public static bool operator ==(State? st1, State? st2)
    {
        return st1?.Heuristic() == st2?.Heuristic();
    }

    public static bool operator !=(State st1, State st2)
    {
        return !(st1 == st2);
    }
}