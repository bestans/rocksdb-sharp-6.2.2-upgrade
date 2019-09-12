using System;
using System.Collections.Generic;
using System.Text;

namespace RocksDbSharp
{
    public class TransactionDBOptions : RocksDBObject
    {
        public static TransactionDBOptions DEFAULT = new TransactionDBOptions();

        public TransactionDBOptions()
        {
            Handle = Native.Instance.rocksdb_transactiondb_options_create();
        }
        protected override void OnDispose()
        {
            Native.Instance.rocksdb_transactiondb_options_destroy(Handle);
        }
        public void SetMaxNumLocks(Int64 param)
        {
            Native.Instance.rocksdb_transactiondb_options_set_max_num_locks(Handle, param);
        }
    }
}
