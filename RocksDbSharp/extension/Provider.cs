using System;
using System.Collections.Generic;
using System.Text;

/*=============================================================================================== 
 Author   : yeyouhuan
 Created  : 2019/9/9 20:01:05 
 Summary  : 单例泛型
 ===============================================================================================*/
namespace RocksDbSharp
{
    public class DBSingletonProvider<T> where T : class, new()
    {
        protected DBSingletonProvider()
        {
        }

        private static T _instance;
        // 用于lock块的对象
        private static readonly object _synclock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synclock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
            set { _instance = value; }
        }
    }
}
