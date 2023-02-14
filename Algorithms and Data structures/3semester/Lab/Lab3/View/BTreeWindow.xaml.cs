using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Lab3.Controllers;
using Lab3.Model;

namespace Lab3.View;

public class BTreeCanvas : Canvas
{
    public BTreeCanvas(BTree<int, string> btree)
    {
        this.Background = Brushes.White;
        BTree = btree;
    }

    public BTree<int, string> BTree { get; set; }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        if (this.BTree == null)
        {
            return;
        }

        double x = this.ActualWidth / 2;
        double y = 10;
        double verticalSpacing = 30;

        this.RenderNode(dc, this.BTree.Root, ref x, ref y, verticalSpacing);
    }

    private void RenderNode(DrawingContext dc, Node<int, string> node, ref double x, ref double y,
        double verticalSpacing)
    {
        double horizontalSpacing = this.ActualWidth / (node.Entries.Count + 1);

        for (int i = 0; i < node.Entries.Count; i++)
        {
            dc.DrawLine(new Pen(Brushes.Black, 3), new Point(x, y), new Point(x, y + verticalSpacing));

            if (!node.IsLeaf)
            {
                this.RenderNode(dc, node.Children[i], ref x, ref y, verticalSpacing);
            }

            x += horizontalSpacing;

            FormattedText text = new FormattedText(
                node.Entries[i].Key.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black);

            dc.DrawText(text,
                new Point(x - horizontalSpacing / 2 - text.Width / 2, y + verticalSpacing / 2 - text.Height / 2));

            if (!node.IsLeaf)
            {
                this.RenderNode(dc, node.Children[i + 1], ref x, ref y, verticalSpacing);
            }
        }

        y += verticalSpacing;
    }
}
