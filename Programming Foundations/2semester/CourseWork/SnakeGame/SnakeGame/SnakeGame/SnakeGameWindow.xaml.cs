using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for SnakeGameWindow.xaml
    /// </summary>
    public partial class SnakeGameWindow : Window
    {
        public SnakeGameWindow()
        {
            InitializeComponent();
            SnakeFieldCellsInit();
        }

        public void SnakeFieldCellsInit()
        {
            for (int i = 0; i < SnakeField.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < SnakeField.ColumnDefinitions.Count; j++)
                {
                    Canvas canvas = new Canvas();
                    canvas.Background = Brushes.Azure;
                    SnakeField.Children.Add(canvas);
                }
            }
        }

    }
}
