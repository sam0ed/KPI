using System;
using System.IO;
using System.Text.Json;
using Lab3.Model;


// Define the controller class
namespace Lab3.Controllers;

public class Controller<TK, TP> where TK : IComparable<TK>
{
    public static readonly Random rand = new Random();
    private const int TreeDegree = 50;
    private const int EntriesToGenerate = 10000;
    private BTree<TK, TP>? _btree;

    public Controller()
    {
        // Check if the Btree file exists
        if (File.Exists("btree.json"))
        {
            // Upload the Btree from the file
            UploadBtree();
        }
        else
        {
            // Generate a new Btree
            GenerateBtree();
        }
    }

    public void StoreBtree()
    {
        // Serialize the Btree to JSON
        string json = JsonSerializer.Serialize(_btree);

        // Write the JSON to the Btree file
        File.WriteAllText("btree.json", json);
    }

    public void UploadBtree()
    {
        // Read the JSON from the Btree file
        string json = File.ReadAllText("btree.json");

        // Deserialize the JSON to a Btree instance
        _btree = JsonSerializer.Deserialize<BTree<TK, TP>>(json);
    }

    public void GenerateBtree()
    {
        // Generate a new Btree instance
        _btree = new BTree<TK, TP>(TreeDegree);

        // Generate 10000 random entries with integer keys and string values
        
        for (int i = 0; i < EntriesToGenerate; i++)
        {
            TK key = (TK)Convert.ChangeType(rand.Next(10000), typeof(TK));
            TP value = (TP)Convert.ChangeType("Value " + rand.Next(10000), typeof(TP));
            _btree.Insert(key, value);
        }
    }

    public void AddEntry(Entry<TK, TP> entry)
    {
        // Add the entry to the Btree
        _btree.Insert(entry.Key, entry.Pointer);
    }

    public Entry<TK, TP>? SearchEntry(TK key)
    {
        // Search for the entry with the given key
        return _btree.Search(key);
    }

    public void DeleteEntry(TK key)
    {
        // Delete the entry with the given key
        _btree.Delete(key);
    }

    public void ModifyEntry(TK key, TP newPointer)
    {
        // Modify the entry with the given key
        var entry = _btree.Search(key);
        if (entry is null)
            throw new ArgumentException("Could not modify entry with the given key");
        else
            entry.Pointer = newPointer;
    }
}