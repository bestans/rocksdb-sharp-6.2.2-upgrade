using System;
using System.Collections.Generic;
using System.Text;

namespace RocksDbSharp
{
    public class TransactionDB : RocksDb
    {
        private TransactionDB(IntPtr handle, DbOptions dbOptions, ColumnFamilies columnFamilyOptions, Dictionary<string, ColumnFamilyHandleInternal> cfHandleMap = null)
            : base(handle, dbOptions, columnFamilyOptions, cfHandleMap)
        {
        }
        protected override void CloseDB()
        {
            Native.Instance.rocksdb_transactiondb_close(Handle);
        }

        public static TransactionDB Open(DbOptions dbOptions, TransactionDBOptions txnDBOptions, string path)
        {
            IntPtr db = Native.Instance.rocksdb_transactiondb_open(dbOptions.Handle, txnDBOptions.Handle, path);
            return new TransactionDB(db, dbOptions, null);
        }
    }
}
