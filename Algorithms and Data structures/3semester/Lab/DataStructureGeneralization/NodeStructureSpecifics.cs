namespace Lab2;

internal interface NodeStructureSpecifics<TValue> where TValue : IEquatable<TValue>
{
    public int? pickInsertIndex(Node<TValue>? insertedNode,Node<TValue> parentNode);//Node<TValue>, Node<TValue>, int?
    public Node<TValue>? search(TValue? wanted, Node<TValue> parentNode); //TValue?, Node<TValue>, Node<TValue>
    public int? pickReplacementIndex(Node<TValue>? removedNode, Node<TValue> parentNode);//Node<TValue>, Node<TValue>, int?
}