using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelStringProcessing.Classes
{
    internal static class MainProcessing
    {
        private const int NUMBER_OF_THREADS = 5;
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

            while (stages.Count > 0)
            {
                InitializeStringProcesssingCommands(stages);

                List<Task<bool>> tasks = new List<Task<bool>>(NUMBER_OF_THREADS);
                for (int i = 0; i < sps.Length; i++)
                {
                    Task<bool> thread = Task<bool>.Run(sps[i].Execute);
                    tasks.Add(thread);
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
            List<StringOperations> currentStage = stages.Dequeue().Operations.ToList();
            currentStringIndex = 0;
            for (int i = 0; i < sps.Length; i++)
            {
                sps[i] = new StringProcessing(strings[i]);
                foreach (var command in currentStage)
                {
                    ParseCommand(command, sps[i]);
                }
            }
        }

        private static void ParseCommand(StringOperations command, StringProcessing sp)
        {
            switch (command)
            {
                case StringOperations.Uppercase:
                    sp.EnqueueAction(sp.UpperCase);
                    break;

                case StringOperations.Sort:
                    sp.EnqueueAction(sp.Sort);
                    break;

                case StringOperations.LowerCase:
                    sp.EnqueueAction(sp.LowerCase);
                    break;

                case StringOperations.Invert:
                    sp.EnqueueAction(sp.Invert);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static void WriteToFile(String filename)
        {
            System.IO.File.WriteAllLines(filename, Array.ConvertAll(strings.ToArray(), x => x.ToString()));
        }
    }
}