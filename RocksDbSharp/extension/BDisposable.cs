using System;
using System.Collections.Generic;
using System.Text;

namespace RocksDbSharp
{
    //线程安全资源回收
    public abstract class BDisposable : IDisposable
    {
        private readonly object lockObj = new object();
        private bool isDispose = false;

        ~BDisposable()
        {
            Dispose();
        }

        public void Dispose()
        {
            lock(lockObj)
            {
                if (!isDispose)
                {
                    isDispose = true;
                    OnDispose();
                }
            }
        }

        protected abstract void OnDispose();
    }
}
