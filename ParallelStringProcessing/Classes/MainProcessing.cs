using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ParallelStringProcessing.Classes
{
    static class MainProcessing
    {
        static List<StringBuilder> strings;
        static StringProcessing[] sps = new StringProcessing[5];
        public static void LoadStringsFromFile(ref string[] newStrings)
        {
            strings = new List<StringBuilder>(newStrings.Length);
            foreach (string s in newStrings)
            {
                strings.Add(new StringBuilder().Append(s));
            }

        }
        public static void Execute()
        {
            for (int i = 0; i < sps.Length; i++)
            {
                sps[i] = new StringProcessing(strings[i]);

                sps[i].QueueAction(sps[i].UpperCase);
                sps[i].QueueAction(sps[i].Sort);
                sps[i].Execute();
            }
        }
     
    }
}
