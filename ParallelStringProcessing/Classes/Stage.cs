using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelStringProcessing.Classes
{
    class Stage
    {
        public Queue<StringOperations> Operations { get; private set; }
        
        Stage()
        {
            Operations = null;
        }
        public Stage(StringOperations[] operations)
        {
            if (operations.Count() > 5)
                Array.Resize(ref operations, 5);
            this.Operations = new Queue<StringOperations>(operations);
        }
    }
}
