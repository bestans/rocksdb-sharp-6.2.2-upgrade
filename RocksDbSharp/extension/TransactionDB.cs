using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public static TransactionDB Open(DbOptions options, TransactionDBOptions txnDBOptions, string path, ColumnFamilies columnFamilies)
        {
            string[] cfnames = columnFamilies.Names.ToArray();
            IntPtr[] cfoptions = columnFamilies.OptionHandles.ToArray();
            IntPtr[] cfhandles = new IntPtr[cfnames.Length];
            IntPtr db = Native.Instance.rocksdb_transactiondb_open_column_families(options.Handle, txnDBOptions.Handle, path, cfnames.Length, cfnames, cfoptions, cfhandles);
            var cfHandleMap = new Dictionary<string, ColumnFamilyHandleInternal>();
            foreach (var pair in cfnames.Zip(cfhandles.Select(cfh => new ColumnFamilyHandleInternal(cfh)), (name, cfh) => new { Name = name, Handle = cfh }))
                cfHandleMap.Add(pair.Name, pair.Handle);
            return new TransactionDB(db, options, columnFamilies, cfHandleMap);
        }

        public Transaction NewTransaction(WriteOptions wop, TransactionOptions txnOptions)
        {
            return new Transaction(this, wop, txnOptions);
        }
    }
}
