using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;

namespace losowanie
{
    public partial class MainPage : ContentPage
{
    List<string> students = new List<string>();
    string filePath = Path.Combine(FileSystem.AppDataDirectory, "klasy.txt");

    public MainPage()
    {
        InitializeComponent();
        studentsList.ItemsSource = students;
    }

    private void RefreshList()
    {
        studentsList.ItemsSource = null;
        studentsList.ItemsSource = students;
    }

    private void OnAddStudentClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(studentEntry.Text))
        {
            students.Add(studentEntry.Text);
            studentEntry.Text = "";
            RefreshList();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string className = classEntry.Text;

        if (string.IsNullOrWhiteSpace(className))
            return;

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"[{className}]");

            foreach (var student in students)
            {
                writer.WriteLine(student);
            }

            writer.WriteLine();
        }

        await DisplayAlert("Sukces", "Zapisano klasê", "OK");
    }

    private void OnLoadClicked(object sender, EventArgs e)
    {
        string className = classEntry.Text;

        if (!File.Exists(filePath) || string.IsNullOrWhiteSpace(className))
            return;

        students.Clear();
        bool found = false;

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (line == $"[{className}]")
            {
                found = true;
                continue;
            }

            if (found)
            {
                if (line.StartsWith("["))
                    break;

                if (!string.IsNullOrWhiteSpace(line))
                    students.Add(line);
            }
        }

        RefreshList();
    }

    private void OnRandomClicked(object sender, EventArgs e)
    {
        if (students.Count == 0)
            return;

        Random rnd = new Random();
        int index = rnd.Next(students.Count);

        resultLabel.Text = "Wylosowano: " + students[index];
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (studentsList.SelectedItem != null)
        {
            students.Remove(studentsList.SelectedItem.ToString());
            RefreshList();
        }
    }
}
}