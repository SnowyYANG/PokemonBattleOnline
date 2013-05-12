using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging.Primitive
{
    internal interface IOperationResult
    {
        bool IsCompleted { get; }
        Exception Exception { get; }
        object Result { get; }
    }

    internal interface IOperationResult<T> : IOperationResult
    {
        new T Result { get; }
    }
}
