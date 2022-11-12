namespace Lab2;

internal class Tree<TValue> : NodeStructureSpecifics<TValue> where TValue : IEquatable<TValue>
{
    public readonly int parentIndex = 0;

    public int? pickInsertIndex(Node<TValue>? insertedNode, Node<TValue> parentNode)
    {
        int? insertIndex = null;

        for (int i = 0; i < parentNode.Connections.Count(); i++)
        {
            if (parentNode.Connections[i] == null && i != parentIndex) insertIndex = i;
        }
        return insertIndex;
    }

    public Node<TValue>? search(TValue? wanted, Node<TValue> parentNode)
    {
        //in depth traversal
        Node<TValue>? found = null;
        for (int i = 0; i < parentNode.Connections.Count && found == null; i++)
        {
            if (i != parentIndex)
                found = parentNode.Connections[i]?.GetNode(wanted, search);
        }

        return found;
    }

    public int? pickReplacementIndex(Node<TValue>? removedNode, Node<TValue> parentNode)
    {
        return parentNode.Connections.Count(node => node != null) > 0
            ? parentNode.Connections.IndexOf(parentNode.Connections.Find(x => x != null))
            : null;
    }
}