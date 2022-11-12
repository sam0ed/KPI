using Lab2;

var parentNode = new Node<int>( 5);
var random = new Random();

NodeStructureSpecifics<int> decisionTree = new Tree<int>();
Node<int> insertedNode;
for (int i = 1; i < 8; i++)
{
    insertedNode =
        new Node<int>(i);
    parentNode.AddNode(insertedNode, decisionTree.pickInsertIndex);
    for (int j = 0; j < 4; j++)
    {
        insertedNode =
            new Node<int>(j*i);
        parentNode.GetNode(i, decisionTree.search)?.AddNode(insertedNode, decisionTree.pickInsertIndex);
    }
}

parentNode.RemoveNode(10,decisionTree.pickReplacementIndex,decisionTree.search);
parentNode.RemoveNode(3,decisionTree.pickReplacementIndex,decisionTree.search);
parentNode.RemoveNode(1,decisionTree.pickReplacementIndex,decisionTree.search);
parentNode.RemoveNode(5,decisionTree.pickReplacementIndex,decisionTree.search);
// parentNode.AddNode(new Node<int>(5, new[] { (int)random.Next(0, 10), (int)random.Next(0, 10), (int)random.Next(0, 10) }), decisionTree.pickInsertIndex);

