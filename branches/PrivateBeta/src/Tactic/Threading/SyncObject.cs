using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LightStudio
{
    internal abstract class SyncObject : DisposableObject
    {
        protected ReaderWriterLockSlim SyncLock
        { get; private set; }

        protected SyncObject()
        {
            SyncLock = new ReaderWriterLockSlim();
        }

        protected void PerformRead(Action readMethod)
        {
            SyncLock.EnterReadLock();
            try
            {
                readMethod();
            }
            finally
            {
                SyncLock.ExitReadLock();
            }
        }

        protected T PerformRead<T>(Func<T> readFunc)
        {
            SyncLock.EnterReadLock();
            try
            {
                return readFunc();
            }
            finally
            {
                SyncLock.ExitReadLock();
            }
        }

        protected void PerformWrite(Action writeMethod)
        {
            SyncLock.EnterWriteLock();
            try
            {
                writeMethod();
            }
            finally
            {
                SyncLock.ExitWriteLock();
            }
        }

        protected T PerformWrite<T>(Func<T> writeFunc)
        {
            SyncLock.EnterWriteLock();
            try
            {
                return writeFunc();
            }
            finally
            {
                SyncLock.ExitWriteLock();
            }
        }

        protected override void DisposeManagedResources()
        {
            SyncLock.Dispose();
        }

    }
}
