using Lab2;

namespace DataStructure;

internal class BinaryTree<TValue> : NodeStructureSpecifics<TValue> where TValue : IComparable<TValue>, IEquatable<TValue>
{
    public readonly int parentIndex = 0;
    public readonly int leftChildIndex = 1;
    public readonly int rightChildIndex = 2;
    public int? pickInsertIndex(Node<TValue>? insertedNode, Node<TValue> parentNode)
    {
        if (parentNode.Connections.Any()) return parentIndex;
        else return pickProcedingNodeIndex(insertedNode.Value, parentNode);
    }

    public int pickProcedingNodeIndex(TValue wanted, Node<TValue> parentNode)
    {
        if (wanted.CompareTo(parentNode.Value) == -1) return leftChildIndex;
        else return rightChildIndex;
    }

    public Node<TValue>? pickReplacement(Node<TValue> removedNode)
    {
        if (removedNode.Connections.Count > 3) throw new Exception();

        Node<TValue>? replacement = null;
        if (removedNode.Connections[leftChildIndex] != null)
        {
            var left = GoToSubtree(leftChildIndex, removedNode, 1);
            replacement = GoToSubtree(rightChildIndex, removedNode);
        }
        else if (removedNode.Connections[rightChildIndex] != null)
        {
            var left = GoToSubtree(rightChildIndex, removedNode, 1);
            replacement = GoToSubtree(leftChildIndex, removedNode);
        }
        return replacement;

    }

    public Node<TValue> GoToSubtree(int connectionIndex, Node<TValue> parentNode, int steps = -1)//tested working
    {
        Node<TValue> nextNode;
        if (connectionIndex == leftChildIndex || connectionIndex == rightChildIndex)
            nextNode = parentNode.Connections[connectionIndex];
        else
            throw new ArgumentException("wrong parameter passed as a type of subtree");


        if (steps == 0)
            return parentNode;
        else if (steps == -1)
        {
            if (nextNode != null)
                return GoToSubtree(connectionIndex, nextNode, steps);
            else
                return parentNode;
        }
        else if (steps > 0 && nextNode != null)
            return GoToSubtree(connectionIndex, nextNode, steps - 1);
        else
            throw new ArgumentException("incrorrect steps parameter");

    }

}