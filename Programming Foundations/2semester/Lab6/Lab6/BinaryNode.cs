using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    internal class BinaryNode
    {
        #region Fields
        public BinaryNode LeftChild { get; set; }
        public BinaryNode RightChild { get; set; }

        public int? Key { get; set; }
        public List<string> Value { get; set; }
        #endregion

        #region Constructors
        public BinaryNode(int? key=null, List<string> value=null)
        {
            LeftChild = null;
            RightChild = null;

            Key = key;
            Value = value;
        }

        #endregion

        #region Methods
        public void AddNode(BinaryNode binaryNode)//tested working
        {
            string editable;
            if (Key != null)
            {
                if (binaryNode.Key == Key) throw new ArgumentException("Can`t add node with same Key");
                else if (binaryNode.Key > Key)
                {
                    editable = "RightChild";
                }
                else
                {
                    editable = "LeftChild";
                }

                if (editable == "RightChild")
                {
                    if (RightChild != null)
                        RightChild.AddNode(binaryNode);
                    else
                        RightChild = binaryNode;
                }
                else if (editable == "LeftChild")
                {
                    if (LeftChild != null)
                        LeftChild.AddNode(binaryNode);
                    else
                        LeftChild = binaryNode;
                }
            }
            else
            {
                this.ChangeNodeTo(binaryNode);
            }


        }

        public void ChangeNodeTo(BinaryNode binaryNode)
        {
            Key = binaryNode.Key;
            Value = binaryNode.Value;
            LeftChild = binaryNode.LeftChild;
            RightChild = binaryNode.RightChild;
        }
        public BinaryNode? GetNode(int key)//tested-working
        {
            if (key == Key)
                return this;
            else if (key < Key && LeftChild != null)
                return LeftChild.GetNode(key);
            else if (key > Key && RightChild != null)
                return RightChild.GetNode(key);
            else
                return null;
        }

        public void RemoveNode(int key)
        {
            if (key == Key)
            {
                BinaryNode replacementNode;
                if (LeftChild != null)
                {
                    replacementNode = LeftChild.GoToSubtree(LeftChild.RightChild);
                    this.ChangeNodeTo(replacementNode);
                    replacementNode.ChangeNodeTo(new BinaryNode());
                }
                else if (RightChild != null)
                {
                    replacementNode = RightChild.GoToSubtree(RightChild.LeftChild);
                    this.ChangeNodeTo(replacementNode);
                    replacementNode.ChangeNodeTo(new BinaryNode());
                }
                else
                {
                    this.ChangeNodeTo(new BinaryNode());
                }

            }
            else
            {
                if (key < Key && LeftChild != null) LeftChild.RemoveNode(key);
                else if (key > Key && RightChild != null) RightChild.RemoveNode(key);
                else throw new ArgumentException("Can`t remove the node--wasn`t found");
            }
        }

        public BinaryNode GoToSubtree(BinaryNode subtree, int steps = -1)//tested working
        {
            BinaryNode nextNode;
            if (subtree == RightChild || subtree == LeftChild)
                nextNode = subtree;
            else
                throw new ArgumentException("wrong parameter passed as a type of subtree");


            if (steps == 0)
                return this;
            else if (steps == -1)
            {
                if (nextNode != null)
                    return nextNode.GoToSubtree(subtree, steps);
                else
                    return this;
            }
            else if (steps > 0 && nextNode != null)
                return nextNode.GoToSubtree(subtree, steps - 1);
            else
                throw new ArgumentException("incrorrect steps parameter");

        }


        //public int GetTreeWidth()
        //{
        //    int width = 1;
        //    BinaryNode node = this;
        //    while (true)
        //    {
        //        try
        //        {
        //            node = node.GoToSubtree(RightChild, 1);
        //            width++;
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            break;
        //        }
        //    }
        //    while (true)
        //    {
        //        try
        //        {
        //            node = node.GoToSubtree(LeftChild, 1);
        //            width++;
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            break;
        //        }
        //    }
        //    return width;
        //}
        #endregion
    }
}
