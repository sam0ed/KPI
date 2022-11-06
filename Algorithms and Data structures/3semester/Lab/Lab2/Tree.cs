namespace Lab2;

internal class Tree<TValue> : NodeStructureSpecifics<TValue>
{
    public int? pickInsertIndex(Node<TValue> insertedNode, Node<TValue> parentNode)
    {
        for (int i = 0; i < parentNode.Children.Count; i++)
        {
            if (parentNode.Children[i] == null) return i;
        }

        return null;
    }

    public Node<TValue>? searchForKey(int key, Node<TValue> parentNode)
    {
        //in depth traversal
        Node<TValue>? found = null;
        for (int i = 0; i < parentNode.Children.Count && found == null; i++)
        {
            found = parentNode.Children[i].GetNode(key, searchForKey);
        }
        return found;
    }

    public Node<TValue>? pickReplacement(Node<TValue> parentNode)
    {
        return parentNode?.Children?[0] ?? null;
    }
}