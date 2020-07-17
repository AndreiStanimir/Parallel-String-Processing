﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParallelStringProcessing.Classes
{
    class StringProcessing : IStringProcessingMethods
    {
        StringBuilder s;
        Queue<Action> commands = new Queue<Action>();

        public void Sort()
        {
            var sortedLetters = s.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            s.Clear();
            s.Append(new string(sortedLetters));
        }
        public void Reverse()
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
        public bool Execute()
        {
            try
            {
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
        public void QueueAction(Action action)
        {
            //check if valid
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
        public StringBuilder GetString()
        {
            return s;
        }

    }
}