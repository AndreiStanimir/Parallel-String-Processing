using Microsoft.Win32;
using ParallelStringProcessing.Classes;
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
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                MainProcessing.LoadStringsFromFile(ref lines);
               
                var stage1 = new Stage(new StringOperations[] { StringOperations.Uppercase, StringOperations.Sort });
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
}