using System;
using System.Collections.Generic;
using System.Text;

namespace RocksDbSharp
{
    public class TransactionOptions : RocksDBObject
    {
        public TransactionOptions()
        {
            Handle = Native.Instance.rocksdb_transaction_options_create();
        }
        protected override void OnDispose()
        {
            Native.Instance.rocksdb_transaction_options_destroy(Handle);
        }
        public void SetExpiration(Int64 expiration)
        {
            Native.Instance.rocksdb_transaction_options_set_expiration(Handle, expiration);
        }
    }
}
