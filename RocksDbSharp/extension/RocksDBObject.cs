using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace RocksDbSharp
{
    public abstract class RocksDBObject : BDisposable
    {
        //其他类引用这个变量，就不会被垃圾回收掉
        internal dynamic References { get; } = new ExpandoObject();

        //对应dll中的指针
        public IntPtr Handle { get; protected set; }
    }
}
