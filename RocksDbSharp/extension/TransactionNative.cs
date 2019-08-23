using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Transitional;

namespace RocksDbSharp
{
    public abstract partial class Native
    {
        public void rocksdb_transaction_put(
            /* rocksdb_transaction_t* */ IntPtr txn,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            string key,
            string val,
            ColumnFamilyHandle cf = null,
            Encoding encoding = null)
        {
            IntPtr errptr = IntPtr.Zero;
            unsafe
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;
                fixed (char* k = key, v = val)
                {
                    int klength = key.Length;
                    int vlength = val.Length;
                    int bklength = encoding.GetByteCount(k, klength);
                    int bvlength = encoding.GetByteCount(v, vlength);
                    var buffer = Marshal.AllocHGlobal(bklength + bvlength);
                    byte* bk = (byte*)buffer.ToPointer();
                    encoding.GetBytes(k, klength, bk, bklength);
                    byte* bv = bk + bklength;
                    encoding.GetBytes(v, vlength, bv, bvlength);
                    UIntPtr sklength = (UIntPtr)bklength;
                    UIntPtr svlength = (UIntPtr)bvlength;

                    if (cf == null)
                        rocksdb_transaction_put(txn, bk, sklength, bv, svlength, out errptr);
                    else
                        rocksdb_transaction_put_cf(txn, cf.Handle, bk, sklength, bv, svlength, out errptr);
#if DEBUG
                    Zero(bk, bklength);
#endif
                    Marshal.FreeHGlobal(buffer);
                }
            }
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_transaction_put(
            /* rocksdb_transaction_t* */ IntPtr txn,
            IntPtr writeOptions,
            byte[] key,
            long keyLength,
            byte[] value,
            long valueLength,
            ColumnFamilyHandle cf)
        {
            IntPtr errptr;
            UIntPtr sklength = (UIntPtr)keyLength;
            UIntPtr svlength = (UIntPtr)valueLength;
            if (cf == null)
                rocksdb_transaction_put(txn, key, sklength, value, svlength, out errptr);
            else
                rocksdb_transaction_put_cf(txn, cf.Handle, key, sklength, value, svlength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }
        public string raw_rocksdb_transaction_get(
            /* rocksdb_transaction_t* */ IntPtr txn,
            /*const rocksdb_readoptions_t**/ IntPtr read_options,
            string key,
            out IntPtr errptr,
            ColumnFamilyHandle cf = null,
            Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            unsafe
            {
                fixed (char* k = key)
                {
                    int klength = key.Length;
                    int bklength = encoding.GetByteCount(k, klength);
                    var buffer = Marshal.AllocHGlobal(bklength);
                    byte* bk = (byte*)buffer.ToPointer();
                    encoding.GetBytes(k, klength, bk, bklength);
                    UIntPtr sklength = (UIntPtr)bklength;

                    var resultPtr = cf == null
                        ? rocksdb_transaction_get(txn, read_options, bk, sklength, out UIntPtr bvlength, out errptr)
                        : rocksdb_transaction_get_cf(txn, read_options, cf.Handle, bk, sklength, out bvlength, out errptr);
#if DEBUG
                    Zero(bk, bklength);
#endif
                    Marshal.FreeHGlobal(buffer);

                    if (errptr != IntPtr.Zero)
                        return null;
                    if (resultPtr == IntPtr.Zero)
                        return null;

                    return MarshalAndFreeRocksDbString(resultPtr, (long)bvlength, encoding);
                }
            }
        }
        public byte[] raw_rocksdb_transaction_get(
            /* rocksdb_transaction_t* */ IntPtr txn,
            IntPtr read_options,
            byte[] key,
            long keyLength,
            out IntPtr errptr,
            ColumnFamilyHandle cf = null)
        {
            UIntPtr skLength = (UIntPtr)keyLength;
            var resultPtr = cf == null
                ? rocksdb_transaction_get(txn, read_options, key, skLength, out UIntPtr valueLength, out errptr)
                : rocksdb_transaction_get_cf(txn, read_options, cf.Handle, key, skLength, out valueLength, out errptr);
            if (errptr != IntPtr.Zero)
                return null;
            if (resultPtr == IntPtr.Zero)
                return null;
            var result = new byte[(ulong)valueLength];
            Marshal.Copy(resultPtr, result, 0, (int)valueLength);
            rocksdb_free(resultPtr);
            return result;
        }

        public string rocksdb_transaction_get(
            /* rocksdb_transaction_t* */ IntPtr txn,
            /*const rocksdb_readoptions_t**/ IntPtr read_options,
            string key,
            ColumnFamilyHandle cf,
            System.Text.Encoding encoding = null)
        {
            var result = raw_rocksdb_transaction_get(txn, read_options, key, out IntPtr errptr, cf, encoding);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public IntPtr rocksdb_transaction_get(
            /* rocksdb_transaction_t* */ IntPtr txn,
            IntPtr read_options,
            byte[] key,
            long keyLength,
            out long vallen,
            ColumnFamilyHandle cf)
        {
            UIntPtr sklength = (UIntPtr)keyLength;
            var result = cf == null
                ? rocksdb_transaction_get(txn, read_options, key, sklength, out UIntPtr valLength, out IntPtr errptr)
                : rocksdb_transaction_get_cf(txn, read_options, cf.Handle, key, sklength, out valLength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            vallen = (long)valLength;
            return result;
        }

        public byte[] rocksdb_transaction_get(
            /* rocksdb_transaction_t* */ IntPtr txn,
            IntPtr read_options,
            byte[] key,
            long keyLength = 0,
            ColumnFamilyHandle cf = null)
        {
            var result = raw_rocksdb_transaction_get(txn, read_options, key, keyLength == 0 ? key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }


        //public void rocksdb_transaction_delete(
        //    /*rocksdb_t**/ IntPtr db,
        //    /*const*/ string key,
        //    ColumnFamilyHandle cf)
        //{
        //    rocksdb_transaction_delete(db, key, cf);
        //    if (errptr != IntPtr.Zero)
        //        throw new RocksDbException(errptr);
        //}

        public void rocksdb_transaction_delete(
            /* rocksdb_transaction_t* */ IntPtr txn,
            /*const*/ string key,
            out IntPtr errptr,
            ColumnFamilyHandle cf,
            Encoding encoding = null)
        {
            var bkey = (encoding ?? Encoding.UTF8).GetBytes(key);
            UIntPtr kLength = (UIntPtr)bkey.GetLongLength(0);
            if (cf == null)
                rocksdb_transaction_delete(txn, bkey, kLength, out errptr);
            //rocksdb_transaction_delete(db, writeOptions, bkey, kLength, out errptr);
            else
                rocksdb_transaction_delete_cf(txn, cf.Handle, bkey, kLength, out errptr);
        }
    }
}
