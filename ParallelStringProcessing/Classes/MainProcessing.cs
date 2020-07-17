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
        static StringProcessing[] sps = new StringProcessing[THREAD_NUMBER];
        static BlockingCollection<StringBuilder> strings;
        public static void LoadStringsFromFile(ref string[] newStrings)
        {
            strings = new BlockingCollection<StringBuilder>(newStrings.Length);
            foreach (string s in newStrings)
            {
                strings.Add(new StringBuilder().Append(s));
            }
            strings.CompleteAdding();
        }
        public static async void Execute()
        {
            ConcurrentBag<StringBuilder> processedStrings = new ConcurrentBag<StringBuilder>();
            for (int i = 0; i < sps.Length; i++)
            {
                sps[i] = new StringProcessing(strings.Take());

                sps[i].QueueAction(sps[i].UpperCase);
                sps[i].QueueAction(sps[i].Sort);
                //sps[i].Execute();
            }
            List<Task<bool>> tasks = new List<Task<bool>>(THREAD_NUMBER);
            for (int i = 0; i < sps.Length; i++)
            {
                Task<bool> thread = Task<bool>.Run(sps[i].Execute);
                tasks.Add(thread);
            }
            StringBuilder dummy;
            while (true)
            {
                int index = Task.WaitAny(tasks.ToArray());
                lock (processedStrings)
                {
                    processedStrings.Add(sps[index].GetString());
                    
                    if (strings.TryTake(out dummy))
                        sps[index].SetString(dummy);
                    else
                        break;
                    tasks[index] = Task<bool>.Run(sps[index].Execute);
                }
            }
            Task.WaitAll(tasks.ToArray());
            strings = new BlockingCollection<StringBuilder>(processedStrings);//????
            WriteToFile("out.txt");
        }

        static void WriteToFile(String filename)
        {
            System.IO.File.WriteAllLines(filename, Array.ConvertAll(strings.ToArray(), x => x.ToString()));
        }

    }
}
