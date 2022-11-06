using Lab2;

var parentNode = new Node<int>(0, new[] { 2, 3, 5 });
var random = new Random();

NodeStructureSpecifics<int> decisionTree = new Tree<int>();
Node<int> insertedNode;
for (int i = 1; i < 8; i++)
{
    insertedNode =
        new Node<int>(i, new[] { (int)random.Next(0, 10), (int)random.Next(0, 10), (int)random.Next(0, 10) });
    parentNode.AddNode(insertedNode, decisionTree.pickInsertIndex);
}

parentNode.RemoveNode(5,decisionTree.pickReplacement,decisionTree.searchForKey);
parentNode.RemoveNode(1,decisionTree.pickReplacement,decisionTree.searchForKey);
parentNode.AddNode(new Node<int>(5, new[] { (int)random.Next(0, 10), (int)random.Next(0, 10), (int)random.Next(0, 10) }), decisionTree.pickInsertIndex);
