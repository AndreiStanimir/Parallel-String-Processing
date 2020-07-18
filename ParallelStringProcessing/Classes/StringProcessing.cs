using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ParallelStringProcessing.Classes
{
    internal class StringProcessing
    {
        private Queue<Action> commands = new Queue<Action>();

        #region String Operations

        public StringBuilder S { get; set; }
        public void Sort()
        {
            var sortedLetters = S.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            S.Clear();
            S.Append(new string(sortedLetters));
        }

        public void Invert()
        {
            for (int i = 0, j = S.Length - 1; i < j; i++, j--)
            {
                (S[i], S[j]) = (S[j], S[i]);
            }
        }

        public void UpperCase()
        {
            for (int i = 0; i < S.Length; i++)
            {
                S[i] = char.ToUpper(S[i]);
            }
        }

        public void LowerCase()
        {
            for (int i = 0; i < S.Length; i++)
            {
                S[i] = char.ToLower(S[i]);
            }
        }

        #endregion String Operations

        public bool Execute()
        {
            try
            {
                var commands = new Queue<Action>(this.commands);
                while (commands.Count > 0)
                {
                    commands.Dequeue().Invoke();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        public void EnqueueAction(Action action)
        {
            //check if valid
            if(action.Target==null)
                MessageBox.Show("invalid action "+action.ToString());
            commands.Enqueue(action);
        }

        public void SetQueue(Queue<Action> actions)
        {
            commands = actions;
        }

        public StringProcessing(StringBuilder s)
        {
            this.S = s;
        }

        public StringProcessing()
        {
        }

        
    }
}