using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ParallelStringProcessing.Classes
{
    static class MainProcessing
    {
        //static List<StringBuilder> strings;
        const int NUMBER_OF_THREADS = 5;
        static StringProcessing[] sps;
        static List<StringBuilder> strings;
        static int currentStringIndex;
        public static void LoadStringsFromFile(ref string[] newStrings)
        {
            strings = new List<StringBuilder>(newStrings.Length);
            foreach (string s in newStrings)
            {
                strings.Add(new StringBuilder().Append(s));
            }
        }
        public static void Execute(Queue<Stage> stages)
        {
            sps = new StringProcessing[Math.Min(NUMBER_OF_THREADS, strings.Count)];
            
            while (stages.Count > 0)
            {
                InitializeStringProcesssingCommands(stages);
                
                List<Task<bool>> tasks = new List<Task<bool>>(NUMBER_OF_THREADS);
                for (int i = 0; i < sps.Length; i++)
                {
                    Task<bool> thread = Task<bool>.Run(sps[i].Execute);
                    tasks.Add(thread);
                    currentStringIndex += 1;
                }
                currentStringIndex -= 1;
                object indexLock = new object();
                while (true)
                {
                    int index = Task.WaitAny(tasks.ToArray());
                    lock (indexLock)
                    {
                        currentStringIndex += 1;
                        if (currentStringIndex < strings.Count)
                        {
                            sps[index].SetString(strings[currentStringIndex]);
                            tasks[index] = Task<bool>.Run(sps[index].Execute);
                        }
                        else
                            break;
                    }
                }

                Task.WaitAll(tasks.ToArray());
            }
            WriteToFile("out.txt");
        }

        private static void InitializeStringProcesssingCommands(Queue<Stage> stages)
        {
            List<StringOperations> currentStage = stages.Dequeue().Operations.ToList();
            currentStringIndex = 0;
            for (int i = 0; i < sps.Length; i++)
            {
                sps[i] = new StringProcessing(strings[i]);
                foreach (var command in currentStage)
                {
                    ParseCommand(i, command, sps);
                }
            }
        }

        private static void ParseCommand(int i, StringOperations command, StringProcessing[] sps)
        {

            switch (command)
            {
                case StringOperations.Uppercase:
                    sps[i].EnqueueAction(sps[i].UpperCase);
                    break;
                case StringOperations.Sort:
                    sps[i].EnqueueAction(sps[i].Sort);
                    break;
                case StringOperations.LowerCase:
                    sps[i].EnqueueAction(sps[i].LowerCase);
                    break;
                case StringOperations.Invert:
                    sps[i].EnqueueAction(sps[i].Invert);
                    break;
                default:
                    break;

            }
        }

        static void WriteToFile(String filename)
        {
            System.IO.File.WriteAllLines(filename, Array.ConvertAll(strings.ToArray(), x => x.ToString()));
        }

    }



}

