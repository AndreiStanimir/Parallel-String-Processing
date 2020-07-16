using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelStringProcessing.Classes
{
    class StringProcessing
    {
        StringBuilder s ;
        Queue<Action> commands=new Queue<Action>();

        public void Sort()
        {
            var sortedLetters = s.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            s.Clear();
            s.Append(new string(sortedLetters));
        }
        public void Reverse()
        {
            for (int i = 0,j=s.Length-1; i<j; i++,j--)
            {
                (s[i],s[j])=(s[j],s[i]);
            }
        }
        public void UpperCase()
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToUpper(s[i]);
            }
        }
        public void Execute()
        {
            while(commands.Count>0)
            {
                commands.Dequeue().Invoke();
            }
        }
        public void QueueAction(Action action)
        {
            //check
            commands.Enqueue(action);
        }
        public StringProcessing(StringBuilder s)
        {
            this.s = s;
        }
        public void SetString(StringBuilder s)
        {
            this.s = s;
        }
    }
}
