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
        const int THREAD_NUMBER = 5;
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
        public static async void Execute()
        {
            sps = new StringProcessing[Math.Min(THREAD_NUMBER, strings.Count)];
            ConcurrentBag<StringBuilder> processedStrings = new ConcurrentBag<StringBuilder>();
            for (int i = 0; i < sps.Length; i++)
            {
                sps[i] = new StringProcessing(strings[i]);

                sps[i].QueueAction(sps[i].UpperCase);
                sps[i].QueueAction(sps[i].Sort);
                //sps[i].Execute();
            }
            List<Task<bool>> tasks = new List<Task<bool>>(THREAD_NUMBER);
            for (int i = 0; i < sps.Length; i++)
            {
                Task<bool> thread = Task<bool>.Run(sps[i].Execute);
                tasks.Add(thread);
                currentStringIndex += 1;
            }
            currentStringIndex -= 1;
            StringBuilder dummy;
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
            WriteToFile("out.txt");
        }
        static void WriteToFile(String filename)
        {
            System.IO.File.WriteAllLines(filename, Array.ConvertAll(strings.ToArray(), x => x.ToString()));
        }

    }


}

