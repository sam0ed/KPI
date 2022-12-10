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
using Lab3.Controllers;
using Lab3.Model;


namespace Lab3.View;

// Define the MainWindow class
public partial class MainWindow : Window
{
    private Controller<int, string> controller;

    public MainWindow()
    {
        InitializeComponent();

        // Create a new controller instance
        controller = new Controller<int, string>();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Get the key and value from the input fields
            int key = int.Parse(KeyInput.Text);
            string value = ValueInput.Text;

            // Create a new entry with the given key and value
            Entry<int, string> entry = new Entry<int, string>
            {
                Key = key,
                Pointer = value
            };

            // Add the entry to the Btree
            controller.AddEntry(entry);

            // Clear the input fields
            KeyInput.Clear();
            ValueInput.Clear();
        }
        catch (FormatException)
        {
            // Show an error message if the key is not an integer
            MessageBox.Show("Please enter a valid integer for the key.");
        }
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Get the key from the input field
            int key = int.Parse(KeyInput.Text);

            // Search for the entry with the given key
            var entry = controller.SearchEntry(key);

            // Show the value of the entry, or an error message if the entry was not found
            if (entry is null)
                MessageBox.Show("No entry found with the given key.");
            else
                MessageBox.Show(entry.Pointer);
        }
        catch (FormatException)
        {
            // Show an error message if the key is not an integer
            MessageBox.Show("Please enter a valid integer for the key.");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Get the key from the input field
            int key = int.Parse(KeyInput.Text);

            // Delete the entry with the given key
            controller.DeleteEntry(key);

            // Clear the input field
            KeyInput.Clear();
        }
        catch (FormatException)
        {
            // Show an error message if the key is not an integer
            MessageBox.Show("Please enter a valid integer for the key.");
        }
    }

    private void ModifyButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Get the key and value from the input fields
            int key = int.Parse(KeyInput.Text);
            string value = ValueInput.Text;

            // Modify the entry with the given key
            controller.ModifyEntry(key, value);

            // Clear the input fields
            KeyInput.Clear();
            ValueInput.Clear();
        }
        catch (FormatException)
        {
            // Show an error message if the key is not an integer
            MessageBox.Show("Please enter a valid integer for the key.");
        }
        catch (ArgumentException ex)
        {
            // Show an error message if the entry could not be modified
            MessageBox.Show(ex.Message);
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Store the Btree to the file when the window is closing
        controller.StoreBtree();
    }
}