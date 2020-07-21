using Microsoft.Win32;
using ParallelStringProcessing.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParallelStringProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ProcessFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async Task<bool> ProcessFile(string filename)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string[] lines = File.ReadAllLines(filename);
            MainProcessing.LoadStringsFromFile(ref lines);

            var stage1 = new Stage(new StringOperations[] { StringOperations.Uppercase, StringOperations.Sort });
            Queue<Stage> stages = new Queue<Stage>();
            var stage2 = new Stage(new StringOperations[] { StringOperations.Invert });
            var stage3 = new Stage(new StringOperations[] { StringOperations.LowerCase });

            stages.Enqueue(stage1);
            stages.Enqueue(stage2);
            stages.Enqueue(stage3);

            var result = await MainProcessing.ExecuteWebAPI(stages);

            MainProcessing.WriteToFile("../../OutFiles/" + Path.GetFileNameWithoutExtension(filename) + ".out");
            var label = new Label();
            watch.Stop();
            double elapsedMs = watch.ElapsedMilliseconds;
            label.Content = "File " + Path.GetFileName(filename) + " finished after " + elapsedMs + "ms";
            StackPanelCompletedTasks.Children.Add(label);
            if (label.ActualHeight > StackPanelCompletedTasks.ActualHeight)
            {
                StackPanelCompletedTasks.Children.RemoveAt(0);
            }
            return true;
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            string[] filePaths = Directory.GetFiles("../../Data", "*.txt",
                                        SearchOption.TopDirectoryOnly);
            foreach (var file in filePaths)
            {
                try
                {
                    await ProcessFile(file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}