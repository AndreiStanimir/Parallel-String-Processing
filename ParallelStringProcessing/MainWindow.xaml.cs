﻿using Microsoft.Win32;
using ParallelStringProcessing.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                MainProcessing.LoadStringsFromFile(ref lines);
                StringProcessing s = new StringProcessing();
                Queue<StringOperations> stage1 = new Queue<StringOperations>(new StringOperations[] {StringOperations.Uppercase,StringOperations.Sort});
                Queue<Queue<StringOperations>> stages = new Queue<Queue<StringOperations>>();
                stages.Enqueue(stage1);
                MainProcessing.Execute(stages);
            }
        }
    }
}
