using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Transitional;

namespace RocksDbSharp
{
    public class RocksDb : BDisposable
    {
        internal static ReadOptions DefaultReadOptions { get; } = new ReadOptions();
        internal static OptionsHandle DefaultOptions { get; } = new DbOptions();
        internal static WriteOptions DefaultWriteOptions { get; } = new WriteOptions();
        internal static Encoding DefaultEncoding => Encoding.UTF8;
        private Dictionary<string, ColumnFamilyHandleInternal> columnFamilies;

        // Managed references to unmanaged resources that need to live at least as long as the db
        internal dynamic References { get; } = new ExpandoObject();

        public IntPtr Handle { get; protected set; }

        private RocksDb(IntPtr handle, dynamic optionsReferences, dynamic cfOptionsRefs, Dictionary<string, ColumnFamilyHandleInternal> columnFamilies = null)
        {
            this.Handle = handle;
            References.Options = optionsReferences;
            References.CfOptions = cfOptionsRefs;
            this.columnFamilies = columnFamilies;
        }
        protected RocksDb(IntPtr handle, DbOptions dbOptions, ColumnFamilies columnFamilyOptions, Dictionary<string, ColumnFamilyHandleInternal> cfHandleMap = null)
        {
            this.Handle = handle;
            if (dbOptions != null)
            {
                References.Options = dbOptions.References;
            }
            if (columnFamilyOptions != null)
            {
                References.CfOptions = columnFamilyOptions.Select(cfd => cfd.Options.References).ToArray();
            }
            this.columnFamilies = cfHandleMap;
        }

        protected override void OnDispose()
        {
            if (columnFamilies != null)
            {
                foreach (var cfh in columnFamilies.Values)
                    cfh.Dispose();
            }
            CloseDB();
        }

        protected virtual void CloseDB()
        {
            Native.Instance.rocksdb_close(Handle);
        }

        public static RocksDb Open(OptionsHandle options, string path)
        {
            IntPtr db = Native.Instance.rocksdb_open(options.Handle, path);
            return new RocksDb(db, optionsReferences: null, cfOptionsRefs: null);
        }

        public static RocksDb OpenReadOnly(OptionsHandle options, string path, bool errorIfLogFileExists)
        {
            IntPtr db = Native.Instance.rocksdb_open_for_read_only(options.Handle, path, errorIfLogFileExists);
            return new RocksDb(db, optionsReferences: null, cfOptionsRefs: null);
        }

        public static RocksDb OpenWithTtl(OptionsHandle options, string path, int ttlSeconds)
        {
            IntPtr db = Native.Instance.rocksdb_open_with_ttl(options.Handle, path, ttlSeconds);
            return new RocksDb(db, optionsReferences: null, cfOptionsRefs: null);
        }

        public static RocksDb Open(DbOptions options, string path, ColumnFamilies columnFamilies)
        {
            string[] cfnames = columnFamilies.Names.ToArray();
            IntPtr[] cfoptions = columnFamilies.OptionHandles.ToArray();
            IntPtr[] cfhandles = new IntPtr[cfnames.Length];
            IntPtr db = Native.Instance.rocksdb_open_column_families(options.Handle, path, cfnames.Length, cfnames, cfoptions, cfhandles);
            var cfHandleMap = new Dictionary<string, ColumnFamilyHandleInternal>();
            foreach (var pair in cfnames.Zip(cfhandles.Select(cfh => new ColumnFamilyHandleInternal(cfh)), (name, cfh) => new { Name = name, Handle = cfh }))
                cfHandleMap.Add(pair.Name, pair.Handle);
            return new RocksDb(db,
                optionsReferences: options.References,
                cfOptionsRefs: columnFamilies.Select(cfd => cfd.Options.References).ToArray(),
                columnFamilies: cfHandleMap);
        }

        public static RocksDb OpenReadOnly(DbOptions options, string path, ColumnFamilies columnFamilies, bool errIfLogFileExists)
        {
            string[] cfnames = columnFamilies.Names.ToArray();
            IntPtr[] cfoptions = columnFamilies.OptionHandles.ToArray();
            IntPtr[] cfhandles = new IntPtr[cfnames.Length];
            IntPtr db = Native.Instance.rocksdb_open_for_read_only_column_families(options.Handle, path, cfnames.Length, cfnames, cfoptions, cfhandles, errIfLogFileExists);
            var cfHandleMap = new Dictionary<string, ColumnFamilyHandleInternal>();
            foreach (var pair in cfnames.Zip(cfhandles.Select(cfh => new ColumnFamilyHandleInternal(cfh)), (name, cfh) => new { Name = name, Handle = cfh }))
                cfHandleMap.Add(pair.Name, pair.Handle);
            return new RocksDb(db,
                optionsReferences: options.References,
                cfOptionsRefs: columnFamilies.Select(cfd => cfd.Options.References).ToArray(),
                columnFamilies: cfHandleMap);
        }

        /// <summary>
        /// Usage:
        /// <code><![CDATA[
        /// using (var cp = db.Checkpoint())
        /// {
        ///     cp.Save("path/to/checkpoint");
        /// }
        /// ]]></code>
        /// </summary>
        /// <returns></returns>
        public Checkpoint Checkpoint()
        {
            var checkpoint = Native.Instance.rocksdb_checkpoint_object_create(Handle);
            return new Checkpoint(checkpoint);
        }

        /// <summary>
        /// Builds an openable snapshot of RocksDB on the same disk, which
        /// accepts an output directory on the same disk, and under the directory
        /// (1) hard-linked SST files pointing to existing live SST files
        /// SST files will be copied if output directory is on a different filesystem
        /// (2) a copied manifest files and other files
        /// The directory should not already exist and will be created by this API.
        /// The directory will be an absolute path
        /// log_size_for_flush: if the total log file size is equal or larger than
        /// this value, then a flush is triggered for all the column families. The
        /// default value is 0, which means flush is always triggered. If you move
        /// away from the default, the checkpoint may not contain up-to-date data
        /// if WAL writing is not always enabled.
        /// Flush will always trigger if it is 2PC.
        /// 
        /// </summary>
        /// <param name="checkpointDir">绝对路径，又该接口创建文件夹</param>
        /// <param name="logSizeForFlush">if the total log file size is equal or larger than this value, then a flush is triggered for all the column families.</param>
        public void CheckPoint(string checkpointDir, ulong logSizeForFlush = 0)
        {
            var cp = Checkpoint();
            cp.Save(checkpointDir, logSizeForFlush);
            cp.Dispose();
        }

        public void SetOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            var keys = options.Select(e => e.Key).ToArray();
            var values = options.Select(e => e.Value).ToArray();
            Native.Instance.rocksdb_set_options(Handle, keys.Length, keys, values);
        }

        public string Get(string key, ColumnFamilyHandle cf = null, ReadOptions readOptions = null, Encoding encoding = null)
        {
            return Native.Instance.rocksdb_get(Handle, (readOptions ?? DefaultReadOptions).Handle, key, cf, encoding ?? DefaultEncoding);
        }

        public byte[] Get(byte[] key, ColumnFamilyHandle cf = null, ReadOptions readOptions = null)
        {
            return Get(key, key.GetLongLength(0), cf, readOptions);
        }

        public byte[] Get(byte[] key, long keyLength, ColumnFamilyHandle cf = null, ReadOptions readOptions = null)
        {
            return Native.Instance.rocksdb_get(Handle, (readOptions ?? DefaultReadOptions).Handle, key, keyLength, cf);
        }

        /// <summary>
        /// Reads the contents of the database value associated with <paramref name="key"/>, if present, into the supplied
        /// <paramref name="buffer"/> at <paramref name="offset"/> up to <paramref name="length"/> bytes, returning the
        /// length of the value in the database, or -1 if the key is not present.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="cf"></param>
        /// <param name="readOptions"></param>
        /// <returns>The actual length of the database field if it exists, otherwise -1</returns>
        public long Get(byte[] key, byte[] buffer, long offset, long length, ColumnFamilyHandle cf = null, ReadOptions readOptions = null)
        {
            return Get(key, key.GetLongLength(0), buffer, offset, length, cf, readOptions);
        }

        /// <summary>
        /// Reads the contents of the database value associated with <paramref name="key"/>, if present, into the supplied
        /// <paramref name="buffer"/> at <paramref name="offset"/> up to <paramref name="length"/> bytes, returning the
        /// length of the value in the database, or -1 if the key is not present.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="cf"></param>
        /// <param name="readOptions"></param>
        /// <returns>The actual length of the database field if it exists, otherwise -1</returns>
        public long Get(byte[] key, long keyLength, byte[] buffer, long offset, long length, ColumnFamilyHandle cf = null, ReadOptions readOptions = null)
        {
            unsafe
            {
                var ptr = Native.Instance.rocksdb_get(Handle, (readOptions ?? DefaultReadOptions).Handle, key, keyLength, out long valLength, cf);
                if (ptr == IntPtr.Zero)
                    return -1;
                var copyLength = Math.Min(length, valLength);
                Marshal.Copy(ptr, buffer, (int)offset, (int)copyLength);
                Native.Instance.rocksdb_free(ptr);
                return valLength;
            }
        }

        public KeyValuePair<byte[],byte[]>[] MultiGet(byte[][] keys, ColumnFamilyHandle[] cf = null, ReadOptions readOptions = null)
        {
            return Native.Instance.rocksdb_multi_get(Handle, (readOptions ?? DefaultReadOptions).Handle, keys);
        }

        public KeyValuePair<string, string>[] MultiGet(string[] keys, ColumnFamilyHandle[] cf = null, ReadOptions readOptions = null)
        {
            return Native.Instance.rocksdb_multi_get(Handle, (readOptions ?? DefaultReadOptions).Handle, keys);
        }

        public void Write(WriteBatch writeBatch, WriteOptions writeOptions = null)
        {
            Native.Instance.rocksdb_write(Handle, (writeOptions ?? DefaultWriteOptions).Handle, writeBatch.Handle);
        }

        public void Write(WriteBatchWithIndex writeBatch, WriteOptions writeOptions = null)
        {
            Native.Instance.rocksdb_write_writebatch_wi(Handle, (writeOptions ?? DefaultWriteOptions).Handle, writeBatch.Handle);
        }

        public void Remove(string key, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            Native.Instance.rocksdb_delete(Handle, (writeOptions ?? DefaultWriteOptions).Handle, key, cf);
        }

        public void Remove(byte[] key, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            Remove(key, key.Length, cf, writeOptions);
        }

        public void Remove(byte[] key, long keyLength, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            if (cf == null)
                Native.Instance.rocksdb_delete(Handle, (writeOptions ?? DefaultWriteOptions).Handle, key, (UIntPtr)keyLength);
            else
                Native.Instance.rocksdb_delete_cf(Handle, (writeOptions ?? DefaultWriteOptions).Handle, cf.Handle, key, (UIntPtr)keyLength);
        }

        public void Put(string key, string value, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null, Encoding encoding = null)
        {
            Native.Instance.rocksdb_put(Handle, (writeOptions ?? DefaultWriteOptions).Handle, key, value, cf, encoding ?? DefaultEncoding);
        }

        public void Put(byte[] key, byte[] value, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            Put(key, key.GetLongLength(0), value, value.GetLongLength(0), cf, writeOptions);
        }

        public void Put(byte[] key, long keyLength, byte[] value, long valueLength, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            Native.Instance.rocksdb_put(Handle, (writeOptions ?? DefaultWriteOptions).Handle, key, keyLength, value, valueLength, cf);
        }

        public Iterator NewIterator(ColumnFamilyHandle cf = null, ReadOptions readOptions = null)
        {
            IntPtr iteratorHandle = cf == null
                ? Native.Instance.rocksdb_create_iterator(Handle, (readOptions ?? DefaultReadOptions).Handle)
                : Native.Instance.rocksdb_create_iterator_cf(Handle, (readOptions ?? DefaultReadOptions).Handle, cf.Handle);
            // Note: passing in read options here only to ensure that it is not collected before the iterator
            return new Iterator(iteratorHandle, readOptions);
        }

        public Iterator[] NewIterators(ColumnFamilyHandle[] cfs, ReadOptions[] readOptions)
        {
            throw new NotImplementedException("TODO: Implement NewIterators()");
            // See rocksdb_create_iterators
        }

        public Snapshot CreateSnapshot()
        {
            IntPtr snapshotHandle = Native.Instance.rocksdb_create_snapshot(Handle);
            return new Snapshot(Handle, snapshotHandle);
        }

        public static IEnumerable<string> ListColumnFamilies(DbOptions options, string name)
        {
            return Native.Instance.rocksdb_list_column_families(options.Handle, name);
        }

        public ColumnFamilyHandle CreateColumnFamily(ColumnFamilyOptions cfOptions, string name)
        {
            var cfh = Native.Instance.rocksdb_create_column_family(Handle, cfOptions.Handle, name);
            var cfhw = new ColumnFamilyHandleInternal(cfh);
            columnFamilies.Add(name, cfhw);
            return cfhw;
        }

        public void DropColumnFamily(string name)
        {
            var cf = GetColumnFamily(name);
            Native.Instance.rocksdb_drop_column_family(Handle, cf.Handle);
            columnFamilies.Remove(name);
        }
        
        public ColumnFamilyHandle GetDefaultColumnFamily()
        {
            return GetColumnFamily(ColumnFamilies.DefaultName);
        }

        public ColumnFamilyHandle GetColumnFamily(string name)
        {
            if (columnFamilies == null)
                throw new RocksDbSharpException("Database not opened for column families");
            return columnFamilies[name];
        }

        public Dictionary<string, ColumnFamilyHandleInternal> GetColumnFamilyHandleMap()
        {
            return columnFamilies;
        }
        
        public string GetProperty(string propertyName)
        {
            foreach (var it in columnFamilies)
            {

            }
            return Native.Instance.rocksdb_property_value_string(Handle, propertyName);
        }

        public string GetProperty(string propertyName, ColumnFamilyHandle cf)
        {
            return Native.Instance.rocksdb_property_value_cf_string(Handle, cf.Handle, propertyName);
        }

        public void IngestExternalFiles(string[] files, IngestExternalFileOptions ingestOptions, ColumnFamilyHandle cf = null)
        {
            if (cf == null)
                Native.Instance.rocksdb_ingest_external_file(Handle, files, (UIntPtr)files.GetLongLength(0), ingestOptions.Handle);
            else
                Native.Instance.rocksdb_ingest_external_file_cf(Handle, cf.Handle, files, (UIntPtr)files.GetLongLength(0), ingestOptions.Handle);
        }

        public void CompactRange(byte[] start, byte[] limit, ColumnFamilyHandle cf = null)
        {
            if (cf == null)
                Native.Instance.rocksdb_compact_range(Handle, start, (UIntPtr)(start?.GetLongLength(0) ?? 0L), limit, (UIntPtr)(limit?.GetLongLength(0) ?? 0L));
            else
                Native.Instance.rocksdb_compact_range_cf(Handle, cf.Handle, start, (UIntPtr)(start?.GetLongLength(0) ?? 0L), limit, (UIntPtr)(limit?.GetLongLength(0) ?? 0L));
        }

        public void CompactRange(string start, string limit, ColumnFamilyHandle cf = null, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            CompactRange(start == null ? null : encoding.GetBytes(start), limit == null ? null : encoding.GetBytes(limit), cf);
        }
    }
}
