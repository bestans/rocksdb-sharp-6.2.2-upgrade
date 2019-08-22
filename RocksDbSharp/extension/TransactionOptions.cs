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
        ~TransactionOptions()
        {
            if (Handle != IntPtr.Zero)
            {
                Native.Instance.rocksdb_transaction_options_destroy(Handle);
                Handle = IntPtr.Zero;
            }
        }
        public void SetExpiration(Int64 expiration)
        {
            Native.Instance.rocksdb_transaction_options_set_expiration(Handle, expiration);
        }
    }
}
