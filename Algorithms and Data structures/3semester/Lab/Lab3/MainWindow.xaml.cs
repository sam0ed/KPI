using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab3.Model;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Random Random = new Random();
        public MainWindow()
        {
            // InitializeComponent();
            BTree<int,char> tree = new BTree<int, char>(5);
            var amountOfNodesToInsert = 150;
            var sequenceToInsert = Enumerable.Range(0, amountOfNodesToInsert).OrderBy(x => Random.Next());
            for (int i = 0; i < amountOfNodesToInsert; i++)
            {
                    tree.Insert(sequenceToInsert.ElementAt(i),(char)Random.Next());
            }
            
            Console.ReadLine();

            Debug.WriteLine(tree.Search(97).Pointer);

        }
    }
}
