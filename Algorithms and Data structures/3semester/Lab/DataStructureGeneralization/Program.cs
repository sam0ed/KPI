using Lab2;

var parentNode = new Node<int>( 8);
var random = new Random();

NodeStructureSpecifics<int> decisionTree = new BinaryTree<int>();
Node<int> insertedNode;
for (int i = 1; i < 20; i++)
{
    insertedNode = new Node<int>(random.Next(0,15));
    parentNode.AddNode(insertedNode, decisionTree.pickInsertIndex);
}

parentNode.ReplaceOrDeleteNode(10,decisionTree.pickReplacement,decisionTree.pickProcedingNodeIndex);
parentNode.ReplaceOrDeleteNode(3,decisionTree.pickReplacement, decisionTree.pickProcedingNodeIndex);
parentNode.ReplaceOrDeleteNode(1,decisionTree.pickReplacement, decisionTree.pickProcedingNodeIndex);
parentNode.ReplaceOrDeleteNode(5,decisionTree.pickReplacement, decisionTree.pickProcedingNodeIndex);
// parentNode.AddNode(new Node<int>(5, new[] { (int)random.Next(0, 10), (int)random.Next(0, 10), (int)random.Next(0, 10) }), decisionTree.pickInsertIndex);

