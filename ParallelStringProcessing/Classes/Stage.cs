using System;
using System.Collections.Generic;
using System.Linq;

namespace ParallelStringProcessing.Classes
{
    internal class Stage
    {
        public Queue<StringOperations> Operations { get; private set; }

        private Stage()
        {
            Operations = null;
        }

        public Stage(StringOperations[] operations)
        {
            if (operations.Count() > 5)
            {
                Array.Resize(ref operations, 5);
            }
            this.Operations = new Queue<StringOperations>(operations);
        }

        public Stage(Stage stage)
        {
            this.Operations = new Queue<StringOperations>(stage.Operations);
        }

        internal void Enqueue(StringOperations operation)
        {
            Operations.Enqueue(operation);
        }
    }
}