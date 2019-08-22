using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RocksDbSharp
{
    public class Transaction : RocksDBObject, IDisposable
    {
        private static readonly object Lock = new object();

        public Transaction(TransactionDB txnDB, WriteOptions wop, TransactionOptions txnOptions)
        {
            Handle = Native.Instance.rocksdb_transaction_begin(txnDB.Handle, wop.Handle, txnOptions.Handle, IntPtr.Zero);
        }

        ~Transaction()
        {
            Dispose();
        }

        public void Dispose()
        {
            lock(Lock)  //防止重复析构
            {
                if (Handle != IntPtr.Zero)
                {
                    Native.Instance.rocksdb_transaction_destroy(Handle);
                    Handle = IntPtr.Zero;
                }
            }
        }

        public void Commit()
        {
            Native.Instance.rocksdb_transaction_commit(Handle);
        }

        public void Rollback()
        {
            Native.Instance.rocksdb_transaction_rollback(Handle);
        }
    }
}
