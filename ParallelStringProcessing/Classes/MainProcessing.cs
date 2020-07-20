using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelStringProcessing.Classes
{
    internal static class MainProcessing
    {
        private const int NUMBER_OF_THREADS = 1;
        private static StringProcessing[] sps;

        private static List<StringBuilder> strings;
        private static int currentStringIndex;

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
            InitializeStringProcesssingCommands(stages);
            List<Task<bool>> tasks = new List<Task<bool>>(NUMBER_OF_THREADS);
            for (int i = 0; i < sps.Length; i++)
            {
                tasks.Add(new Task<bool>(sps[i].Execute));
            }
            while (stages.Count > 0)
            {
                var stage = stages.Dequeue();
                currentStringIndex = 0;
                for (int i = 0; i < sps.Length; i++)
                {
                    sps[i].SetString(strings[i]);
                    sps[i].SetQueue(stage);
                }
                for (int i = 0; i < sps.Length; i++)
                {
                    tasks[i] = Task<bool>.Run(sps[i].Execute);
                }
                currentStringIndex = sps.Length - 1;
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
                            sps[index].SetQueue(stage);
                            tasks[index] = Task<bool>.Run(sps[index].Execute);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

        private static void InitializeStringProcesssingCommands(Queue<Stage> stages)
        {
            for (int i = 0; i < sps.Length; i++)
            {
                sps[i] = new StringProcessing(strings[i]);
            }
        }

        

        public static void WriteToFile(String filename)
        {
            System.IO.File.WriteAllLines(filename, Array.ConvertAll(strings.ToArray(), x => x.ToString()));
        }
    }
}