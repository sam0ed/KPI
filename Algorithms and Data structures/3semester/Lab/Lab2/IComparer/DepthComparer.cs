namespace Lab2.IComparer;

public class DepthComparer:IComparer<State>
{
    public int Compare(State x, State y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.Depth.CompareTo(y.Depth);
    }
}