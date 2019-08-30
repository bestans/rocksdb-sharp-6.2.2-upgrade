using System;
using System.Collections.Generic;
using System.Text;

namespace RocksDbSharp
{
    public class Checkpoint : BDisposable
    {
        public IntPtr Handle { get; }

        public Checkpoint(IntPtr handle)
        {
            Handle = handle;
        }

        protected override void OnDispose()
        {
            Native.Instance.rocksdb_checkpoint_object_destroy(Handle);
        }

        public void Save(string checkpointDir, ulong logSizeForFlush = 0)
            => Native.Instance.rocksdb_checkpoint_create(Handle, checkpointDir, logSizeForFlush);
    }
}
