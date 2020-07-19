﻿using Microsoft.Win32;
using ParallelStringProcessing.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
                    MainProcessing.WriteToFile(Path.GetFileNameWithoutExtension(openFileDialog.FileName) + ".out");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            string[] filePaths = Directory.GetFiles("../../Data", "*.txt",
                                         SearchOption.TopDirectoryOnly);
            foreach (var file in filePaths)
            {
                try
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    ProcessFile(file);
                    MainProcessing.WriteToFile("../../OutFiles/" + Path.GetFileNameWithoutExtension(file) + ".out");
                    var label = new Label();
                    watch.Stop();
                    double elapsedMs = watch.ElapsedMilliseconds;
                    label.Content = "File " + file + " finished after "+ elapsedMs +"ms";
                    StackPanelCompletedTasks.Children.Add(label);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
           
                MainProcessing.Execute(stages);
            
          
        }
    }
}