using System;
using System.Text;

namespace ParallelStringProcessing.Classes
{
    internal class StringProcessing
    {
        private Stage commands;

        private StringBuilder s;

        public StringBuilder GetString()
        {
            return s;
        }

        public void SetString(StringBuilder s)
        {
            this.s = s;
        }

        public StringProcessing(StringBuilder s)
        {
            this.s = s;
        }

        public StringOperations DequeAction()
        {
            if (commands.Operations.Count == 0)
            {
                throw new Exception("deque error. Queue is empty.");
            }
            return commands.Operations.Dequeue();
        }

        public void EnqueueAction(StringOperations operation)
        {
            commands.Enqueue(operation);
        }

        public bool Execute()
        {
            StringProcessingAPI.ProcessLine(ref s, commands);
            return true;
        }

        public void SetQueue(Stage stage)
        {
            commands = stage;
        }
    }
}