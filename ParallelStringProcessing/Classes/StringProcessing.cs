using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelStringProcessing.Classes
{
    internal class StringProcessing
    {
        private Queue<Action> commands = new Queue<Action>();

        #region String Operations

        private StringBuilder s;

        public StringBuilder GetString() { return s; }  
        public void SetString(StringBuilder s) { this.s = s; }

        public void Invert()
        {
            for (int i = 0, j = s.Length - 1; i < j; i++, j--)
            {
                (s[i], s[j]) = (s[j], s[i]);
            }
        }

        public void LowerCase()
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToLower(s[i]);
            }
        }

        public void Sort()
        {
            var sortedLetters = s.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            s.Clear();
            s.Append(new string(sortedLetters));
        }

        public void UpperCase()
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToUpper(s[i]);
            }
        }

        #endregion String Operations

        public StringProcessing(StringBuilder s)
        {
            this.s = s;
        }

        public Action DequeAction()
        {
            if (commands.Count == 0)
            {
                return null;
            }
            return commands.Dequeue();
        }

        public void EnqueueAction(Action action)
        {
            //check if valid
            if (action.Target == null)
            {
                throw new Exception("invalid action " + action.ToString());
            }
            commands.Enqueue(action);
        }

        public bool Execute()
        {
            var commands = new Queue<Action>(this.commands);
            while (commands.Count > 0)
            {
                commands.Dequeue().Invoke();
            }

            return true;
        }

        public void SetQueue(Queue<Action> actions)
        {
            commands = actions;
        }
    }
}