using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private static HttpClient client = new HttpClient();

        public static async Task<bool> ExecuteWebAPI(Queue<Stage> stages)
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("https://localhost:44380/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
            try
            {
                while (stages.Count > 0)
                {
                    var stage = stages.Dequeue();
                    var tasks = strings.AsParallel().WithDegreeOfParallelism(5).Select(async s =>
                          {
                              var result = await GetProcessedStringAsync(s, stage.Operations.ToArray());
                          });
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
            return true;
        }

        private static async Task<StringBuilder> GetProcessedStringAsync(StringBuilder line, StringOperations[] operations)
        {
            int[] op = new int[operations.Length];
            string requestUri = string.Format("https://localhost:44380/api/values/?s={0}", line.ToString());
            for (int i = 0; i < operations.Length; i++)
            {
                op[i] = (int)operations[i];
                requestUri += "&operations=" + (int)operations[i];
            }
            HttpResponseMessage response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                string result_string = await response.Content.ReadAsAsync<string>();
                line.Clear();
                line.Append(result_string);
            }
            return line;
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