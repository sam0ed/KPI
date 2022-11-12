using System.Reflection.Metadata;

namespace Lab2
{
    internal class Node<TValue> where TValue : IEquatable<TValue>
    {
        #region Fields

        public List<Node<TValue>?> Connections { get; set; }
        public TValue? Value { get; set; }

        #endregion

        #region Constructors

        public Node(TValue? value = default)
        {
            Value = value;
            Connections = new List<Node<TValue>?>();
        }

        #endregion

        #region Methods

        public void AddNode(Node<TValue>? node, Func<Node<TValue>?, Node<TValue>, int?> pickInsertIndex) //tested working
        {
            if (!Connections.Contains(node))
            {
                int? insertIndex = pickInsertIndex(node, this);
                if (insertIndex != null)
                {
                    if (Connections![(int)insertIndex] != null)
                        Connections[(int)insertIndex]!.AddNode(node, pickInsertIndex);
                    else
                    {
                        Connections[(int)insertIndex] = node;
                        try
                        {
                            node?.AddNode(this, pickInsertIndex);
                        }
                        catch
                        {
                        } //add exception processing here
                    }
                }
                else
                {
                    Connections.Add(node);
                    try
                    {
                        node?.AddNode(this, pickInsertIndex);
                    }
                    catch
                    {
                    } //add exception processing here
                }
            }
            else
                throw new ArgumentException(
                    "Current node already contains connection to the node you are trying to add");
        }

        private void ChangeNodeTo(Node<TValue> node)
        {
            Value = node.Value;
            Connections = node.Connections;
        }
        
        public Node<TValue>? GetNode(TValue? value, Func<TValue?, Node<TValue>, Node<TValue>?> search)
        {
            if (Value != null && Value.Equals(value))
                return this;
            else if (Connections.Count(node => node != null) != 0)
                return search(value, this);
            else
                return null;
        }

        public void RemoveNode(TValue value, Func<Node<TValue>, Node<TValue>, int?> pickReplacementIndex,
            Func<TValue?, Node<TValue>, Node<TValue>?> search)
        {
            Node<TValue>? nodeToRemove = GetNode(value, search);
            if (nodeToRemove != null)
            {
                var replacementNodeIndex = pickReplacementIndex(nodeToRemove, this);
                if (replacementNodeIndex != null)
                {
                    var replacementNode = nodeToRemove.Connections[(int)replacementNodeIndex];
                    if (replacementNode != null)
                    {
                        var nodeToRemoveConnectionsToCopy =
                            nodeToRemove.Connections.Where(x => x != replacementNode).ToList();
                        replacementNode.Connections.AddRange(nodeToRemoveConnectionsToCopy);
                        replacementNode.Connections =
                            replacementNode.Connections.Distinct().ToList(); //distinct uses default 
                        replacementNode.Connections.Remove(nodeToRemove);
                        // nodeToRemove.ChangeNodeTo(replacementNode!);
                        nodeToRemove = replacementNode; //?????????????????????
                    }
                    else throw new ArgumentNullException();
                }
                else
                {
                    for (int i = 0; i < nodeToRemove.Connections.Count(); i++)
                    {
                        if (nodeToRemove.Connections[i] != null)
                            nodeToRemove!.Connections[i]!.Connections.Remove(nodeToRemove);
                    }
                }
            }
        }

        #endregion
    }
}
        // private void ChangeConnection(Node<TValue>? oldNode, Node<TValue>? newNode)
        // {
        //     if (Connections == null) throw new ArgumentNullException();
        //     for (int i = 0; i < Connections?.Count; i++)
        //     {
        //         if (Connections[i] == oldNode)
        //             Connections[i] = newNode;
        //     }
        // }
