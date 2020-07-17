using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ParallelStringProcessing.Classes
{
    internal class StringProcessing : IStringProcessingMethods
    {
        private StringBuilder s;
        private Queue<Action> commands = new Queue<Action>();

        #region String Operations

        public void Sort()
        {
            var sortedLetters = s.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            s.Clear();
            s.Append(new string(sortedLetters));
        }

        public void Invert()
        {
            for (int i = 0, j = s.Length - 1; i < j; i++, j--)
            {
                (s[i], s[j]) = (s[j], s[i]);
            }
        }

        public void UpperCase()
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToUpper(s[i]);
            }
        }

        public void LowerCase()
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToLower(s[i]);
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
            commands.Enqueue(action);
        }

        public void SetQueue(Queue<Action> actions)
        {
            commands = actions;
        }

        public StringProcessing(StringBuilder s)
        {
            this.s = s;
        }

        public StringProcessing()
        {
        }

        public void SetString(StringBuilder s)
        {
            this.s = s;
        }

        public StringBuilder GetString()
        {
            return s;
        }
    }
}