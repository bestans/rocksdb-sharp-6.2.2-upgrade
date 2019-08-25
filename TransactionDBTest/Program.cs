using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RocksDbSharp;

namespace TransactionDBTest
{
    class TestDestruct
    {
        public int value = 10;
        ~TestDestruct()
        {
            Console.WriteLine("destruct");
        }
    }

    class Program
    {
        static string data = "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时" +
            "俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时代峻峰看见爱上了肯定积分卡萨看来大家福利卡加适量快递费即可拉伸俺的沙发斯蒂芬结案率时";

        static string key = "1111";
        static string testkey = "aaa";
        static byte[] keyb = System.Text.Encoding.Default.GetBytes(key);
        static void Main(string[] args)
        {
            test5();
            Console.ReadKey();
        }

        static void test5()
        {
            var db = TransactionDB.Open(new DbOptions().SetCreateIfMissing(true), new TransactionDBOptions(), "transaction_db_test");
            using (var it = db.NewIterator())
            {
                it.SeekToFirst();
                while (it.Valid())
                {
                    var key = Encoding.Default.GetString(it.Key());
                    var value = Encoding.Default.GetString(it.Value());
                    Console.WriteLine("key=" + key + ",value=" + value);
                    it.Next();
                }
            }
            
        }
        static void test4()
        {
            var db = TransactionDB.Open(new DbOptions().SetCreateIfMissing(true), new TransactionDBOptions(), "transaction_db_test");
            var txnOp = new TransactionOptions();
            var wop = new WriteOptions();
            int count = 0;
            while (true)
            {
                var txn = new Transaction(db, wop, txnOp);
                string key = testkey;
                txn.Put(key, data + count++);
                txn.Get(key);
                txn.Put(key, data + count++);
                txn.Get(key);
                txn.Put(key, data + count++);
                txn.Get(key);
                if (count % 2 == 0)
                    txn.Commit();
                else
                    txn.Rollback();
                if (count % 30000 == 0)
                {
                    Console.WriteLine("run count:" + count / 3);
                }
                Thread.Sleep(1);
            }
        }
        static void test3()
        {
            var db = TransactionDB.Open(new DbOptions().SetCreateIfMissing(true), new TransactionDBOptions(), "transaction_db_test");
            
            {
                {
                    {
                        // With strings
                        string value = db.Get("key");
                        Console.WriteLine("value=" + value);
                        db.Put("key", "value");
                        value = db.Get("key");
                        Console.WriteLine("value=" + value);
                        string iWillBeNull = db.Get("non-existent-key");
                        db.Remove("key");
                    }

                    {
                        // With bytes
                        var key = Encoding.UTF8.GetBytes("key");
                        byte[] value = Encoding.UTF8.GetBytes("value");
                        db.Put(key, value);
                        value = db.Get(key);
                        Console.WriteLine("value=" + Encoding.UTF8.GetString(value));
                        byte[] iWillBeNull = db.Get(new byte[] { 0, 1, 2 });
                        db.Remove(key);

                        db.Put(key, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                    }

                    {
                        // With buffers
                        var key = Encoding.UTF8.GetBytes("key");
                        var buffer = new byte[100];
                        long length = db.Get(key, buffer, 0, buffer.Length);
                        Console.WriteLine("length=" + length + "," + key.Length);
                    }
                }
            }
        }
        static void test2()
        {
            Int64 count = 0;
            var r = new Random();
            while (true)
            {
                var db = TransactionDB.Open(new DbOptions().SetCreateIfMissing(true), new TransactionDBOptions(), "transaction_db_test");
                var txnOp = new TransactionOptions();
                var wop = new WriteOptions();
                using (var txn = new Transaction(db, wop, txnOp))
                {
                    int ret = (int)(count % 2);
                    string str = data + count;
                    switch (ret)
                    {
                        case 0:
                            txn.Put("1111", str);
                            break;
                        default:
                            txn.Put(keyb, Encoding.Default.GetBytes(str));
                            break;
                    }
                    if (r.Next(0, 2) == 0)
                        txn.Commit();
                    else
                        txn.Rollback();
                    if (count++ % 100 == 0 || Transaction.count % 100 == 0)
                    {
                        Console.WriteLine("run times:" + count + "," + Transaction.count);
                    }
                }
                db.Dispose();
                Thread.Sleep(1);
            }
        }

        static void test1()
        {
            for (int i =0; i < 10; ++i)
            {
                GC.Collect();
                TestDestruct t = new TestDestruct();
                Console.WriteLine(t.value);
                Thread.Sleep(1000);
            }
        }
    }
}
