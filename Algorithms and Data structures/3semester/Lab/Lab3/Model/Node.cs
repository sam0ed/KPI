using System.Text.Json.Serialization;

namespace Lab3.Model;

using System.Collections.Generic;

public class Node<TK, TP>
{
    private readonly int _degree;

    public Node(int degree)
    {
        this._degree = degree;
        this.Children = new List<Node<TK, TP>>(degree);
        this.Entries = new List<Entry<TK, TP>>(degree);
    }

    [JsonConstructor]
    public Node()
    {
    }

    public List<Node<TK, TP>> Children { get; set; }

    public List<Entry<TK, TP>> Entries { get; set; }

    public bool IsLeaf
    {
        get { return this.Children.Count == 0; }
        set
        {
        }
    }

    public bool HasReachedMaxEntries
    {
        get { return this.Entries.Count == (2 * this._degree) - 1; }
        set
        {
        }
    }

    public bool HasReachedMinEntries
    {
        get { return this.Entries.Count == this._degree - 1; }
        set
        {
        }
    }
}