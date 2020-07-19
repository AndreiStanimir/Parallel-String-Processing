using Microsoft.Win32;
using ParallelStringProcessing.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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
                ProcessFile(openFileDialog.FileName);
                MainProcessing.WriteToFile(Path.GetFileNameWithoutExtension(openFileDialog.FileName) + ".out");
            }
            string[] filePaths = Directory.GetFiles("../../Data", "*.txt",
                                         SearchOption.TopDirectoryOnly);
            foreach (var file in filePaths)
            {
                ProcessFile(file);
                MainProcessing.WriteToFile("../../OutFiles/" + Path.GetFileNameWithoutExtension(file) + ".out");
            }
        }

        private static void ProcessFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            MainProcessing.LoadStringsFromFile(ref lines);

            var stage1 = new Stage(new StringOperations[] { StringOperations.Uppercase, StringOperations.Sort, StringOperations.Invert });
            Queue<Stage> stages = new Queue<Stage>();
            var stage2 = new Stage(new StringOperations[] { StringOperations.Invert });
            var stage3 = new Stage(new StringOperations[] { StringOperations.LowerCase });

            stages.Enqueue(stage1);
            stages.Enqueue(stage2);
            stages.Enqueue(stage3);
            try
            {
                MainProcessing.Execute(stages);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}