using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
    internal class OperationResult<T> : IOperationResult<T>
    {
        public bool IsCompleted
        { get; private set; }

        public Exception Exception
        { get; private set; }

        public T Result
        { get; private set; }
        
        object IOperationResult.Result
        {
            get { return Result; }
        }

        public OperationResult(T result)
        {
            this.IsCompleted = true;
            this.Result = result;
        }

        public OperationResult(Exception exception)
        {
            this.IsCompleted = false;
            this.Exception = exception;
        }
    }
}
