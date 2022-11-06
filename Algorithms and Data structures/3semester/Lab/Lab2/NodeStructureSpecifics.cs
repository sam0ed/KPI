namespace Lab2;

internal interface NodeStructureSpecifics<TValue>
{
    public int? pickInsertIndex(Node<TValue> insertedNode,Node<TValue> parentNode);
    public Node<TValue>? searchForKey(int key, Node<TValue> parentNode);
    public Node<TValue>? pickReplacement(Node<TValue> parentNode);
}