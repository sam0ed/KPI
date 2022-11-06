namespace Lab2
{
    internal class Node<TValue>
    {
        #region Fields

        public List<Node<TValue>?>? Children { get; set; }

        private int? Key { get; set; }
        private TValue[]? Value { get; set; }

        #endregion

        #region Constructors

        public Node(int? key = null, TValue[]? value = null)
        {
            Key = key;
            Value = value;
            Children = null;
        }

        #endregion

        #region Methods

        public void AddNode(Node<TValue> node, Func<Node<TValue>, Node<TValue>, int?> pickInsertIndex) //tested working
        {
            if (this.Children == null) this.Children = new List<Node<TValue>>();
            int? insertIndex = pickInsertIndex(node, this);
            if (insertIndex != null)
            {
                Children[(int)insertIndex].AddNode(node, pickInsertIndex);
            }
            else
            {
                Children.Add(node);
            }

            //     if (binaryNode.Key == Key) throw new ArgumentException("Can`t add node with same Key");
            //     else if (binaryNode.Key > Key)
            //     {
            //         editable = "RightChild";
            //     }
            //     else
            //     {
            //         editable = "LeftChild";
            //     }
            //
            //     if (editable == "RightChild")
            //     {
            //         if (RightChild != null)
            //             RightChild.AddNode(binaryNode);
            //         else
            //             RightChild = binaryNode;
            //     }
            //     else if (editable == "LeftChild")
            //     {
            //         if (LeftChild != null)
            //             LeftChild.AddNode(binaryNode);
            //         else
            //             LeftChild = binaryNode;
            //     }
            // }
            // else
            // {
            //     this.ChangeNodeTo(binaryNode);
            // }
        }

        private void ChangeNodeTo(Node<TValue> node)
        {
            Key = node.Key;
            Value = node.Value;
            Children = node.Children;
        }

        private void DeleteChild(Node<TValue> node)
        {
            if (Children == null) throw new ArgumentNullException();
            for (int i = 0; i < Children?.Count; i++)
            {
                if (Children[i] == node)
                    Children[i] = null;
            }
        }

        public Node<TValue>? GetNode(int key, Func<int, Node<TValue>, Node<TValue>> searchForKey) //tested-working
        {
            if (key == Key)
                return this;
            else if (Children != null )
                return searchForKey(key, this);
            else
                return null;
            // else if (key < Key && LeftChild != null)
            //     return LeftChild.GetNode(key);
            // else if (key > Key && RightChild != null)
            //     return RightChild.GetNode(key);
        }

        public void RemoveNode(int key, Func<Node<TValue>, Node<TValue>> pickReplacement,
            Func<int, Node<TValue>, Node<TValue>> searchForKey)
        {
            if (key == Key)
            {
                Node<TValue>? replacementNode = pickReplacement(this);
                this.ChangeNodeTo(replacementNode);
                replacementNode.ChangeNodeTo(new Node<TValue>());
                // if (LeftChild != null)
                // {
                //     replacementNode = LeftChild.GoToSubtree(LeftChild.RightChild);
                //     this.ChangeNodeTo(replacementNode);
                //     replacementNode.ChangeNodeTo(new Node<TValue>());
                // }
                // else if (RightChild != null)
                // {
                //     replacementNode = RightChild.GoToSubtree(RightChild.LeftChild);
                //     this.ChangeNodeTo(replacementNode);
                //     replacementNode.ChangeNodeTo(new Node<TValue>());
                // }
                // else
                // {
                //     this.ChangeNodeTo(new Node<TValue>());
                // }
            }
            else
            {
                Node<TValue> nextNodeToCheck = GetNode(key, searchForKey);
                nextNodeToCheck.RemoveNode(key, pickReplacement, searchForKey);
                // if (key < Key && LeftChild != null) LeftChild.RemoveNode(key);
                // else if (key > Key && RightChild != null) RightChild.RemoveNode(key);
                // else throw new ArgumentException("Can`t remove the node--wasn`t found");
            }
        }

        // public Node<TValue> GoToSubtree(Node<TValue> subtree, int steps = -1) //tested working
        // {
        //     Node<TValue>? nextNode=null;
        //     foreach (var node in Children!.Where(node => node == subtree))
        //     {
        //         nextNode = subtree;
        //     }
        //     if(nextNode==null)
        //         throw new ArgumentException("wrong parameter passed as a type of subtree");
        //
        //
        //     if (steps == 0)
        //         return this;
        //     else if (steps == -1)
        //     {
        //         if (nextNode != null)
        //             return nextNode.GoToSubtree(subtree, steps);
        //         else
        //             return this;
        //     }
        //     else if (steps > 0)
        //         return nextNode.GoToSubtree(subtree, steps - 1);
        //     else
        //         throw new ArgumentException("incorrect steps parameter");
        // }

        #endregion
    }
}