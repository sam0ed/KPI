using System.Reflection.Metadata;

namespace Lab2
{
    internal class Node<TValue> where TValue : IEquatable<TValue>
    {
        #region Fields

        public List<Node<TValue>> Connections { get; set; }
        public TValue Value { get; set; }

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
                    Connections[(int)insertIndex].AddNode(node, pickInsertIndex);
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

        private void ChangeNodeContentTo(Node<TValue> node)
        {
            Value = node.Value;
            Connections = node.Connections;
        }

        public Node<TValue>? GetNode(TValue value, Func<TValue?, Node<TValue>?, int> pickProcedingNodeIndex)
        {
            if (Value != null && Value.Equals(value))
                return this;
            else if (Connections.Any())
            {
                int procedingNodeIndex = pickProcedingNodeIndex(value, this);
                return Connections[(int)procedingNodeIndex].GetNode(value, pickProcedingNodeIndex);
            }
            else
                return null;
        }

        public void ReplaceOrDeleteNode(TValue value, Func<Node<TValue>, Node<TValue>?> pickReplacement,
            Func<TValue?, Node<TValue>?, int> pickProcedingNodeIndex)
        {
            Node<TValue>? nodeToRemove = GetNode(value, pickProcedingNodeIndex);
            if (nodeToRemove != null)
            {
                var replacementNode = pickReplacement(nodeToRemove);
                if (replacementNode != null)
                { 
                    var replacementNodeConnectionsToCopy = replacementNode.Connections;
                    for (int i = 0; i < replacementNodeConnectionsToCopy.Count; i++)
                    {
                        replacementNodeConnectionsToCopy[i].Connections.Remove(replacementNode);
                        replacementNodeConnectionsToCopy[i].Connections.Add(nodeToRemove);
                    }

                    nodeToRemove.Connections.AddRange(replacementNodeConnectionsToCopy);
                    nodeToRemove.Connections =
                        nodeToRemove.Connections.Distinct().ToList(); //distinct uses default 

                    nodeToRemove.Connections.Remove(replacementNode);
                    nodeToRemove.Connections.Remove(nodeToRemove);
                    nodeToRemove.Value = replacementNode.Value;

                }
                else
                {
                    DeleteNode(nodeToRemove);
                }
            }
        }

        public void DeleteNode(Node<TValue> nodeToRemove)
        {
            for (int i = 0; i < nodeToRemove.Connections.Count(); i++)
            {
                nodeToRemove!.Connections[i]!.Connections.Remove(nodeToRemove);
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
