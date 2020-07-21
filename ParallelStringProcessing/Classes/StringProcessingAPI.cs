using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelStringProcessing.Classes
{
    internal class StringProcessingAPI
    {
        public static StringBuilder ProcessLine(StringBuilder line, Queue<Stage> stages)
        {
            while (stages.Count > 0)
            {
                var stage = stages.Dequeue();
                ProcessLine(ref line, stage);
            }
            return line;
        }

        public static StringBuilder ProcessLine(ref StringBuilder line, Stage stage)
        {
            var operations = stage.Operations.ToArray();
            foreach (var op in operations)
            {
                ParseCommand(ref line, op);
            }
            return line;
        }

        private static void ParseCommand(ref StringBuilder line, StringOperations command)
        {
            switch (command)
            {
                case StringOperations.Uppercase:
                    UpperCase(ref line);
                    break;

                case StringOperations.Sort:
                    Sort(ref line);
                    break;

                case StringOperations.LowerCase:
                    LowerCase(ref line);
                    break;

                case StringOperations.Invert:
                    Invert(ref line);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private static void Invert(ref StringBuilder s)
        {
            for (int i = 0, j = s.Length - 1; i < j; i++, j--)
            {
                (s[i], s[j]) = (s[j], s[i]);
            }
        }

        private static void LowerCase(ref StringBuilder s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToLower(s[i]);
            }
        }

        private static void Sort(ref StringBuilder s)
        {
            var sortedLetters = s.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            s.Clear();
            s.Append(new string(sortedLetters));
        }

        private static void UpperCase(ref StringBuilder s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToUpper(s[i]);
            }
        }
    }
}