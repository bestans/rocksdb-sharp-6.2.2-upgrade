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
            Test8();
            Console.ReadKey();
        }
        static void Test9()
        {
            var dbop = new DbOptions()
                .SetCreateIfMissing(true)
                .SetCreateMissingColumnFamilies(true);

            var columnFamilies = new ColumnFamilies
            {
                { "reverse1", new ColumnFamilyOptions() },
                { "reverse2", new ColumnFamilyOptions() },
            };
            var db = TransactionDB.Open(dbop, new TransactionDBOptions(), "transaction_db_test5", columnFamilies);
            var reverse = db.GetColumnFamily("reverse1");
            var wop = new WriteOptions();

            for (int i = 0; i < 10; i += 2)
            {
                db.Put(i.ToString(), i.ToString(), reverse);
            }
            Console.WriteLine("begin");
            var t1 = Task.Run(async () =>
            {
                var it = db.NewIterator(reverse);
                it.SeekToFirst();
                while (it.Valid())
                {
                    Console.WriteLine("it:key=" + it.StringKey() +",value" + it.StringValue());
                    await Task.Delay(500);
                    it.Next();
                }
            });
            var t2 = Task.Run(async () =>
            {
                for (int i = 1; i < 10; i += 2)
                {
                    await Task.Delay(500);
                    db.Remove(i.ToString());
                    Console.WriteLine("write key=" + i);
                }
            });
            Task.WaitAll(t1, t2);
        }
        static void Test8()
        {
            var dbop = new DbOptions()
                .SetCreateIfMissing(true)
                .SetCreateMissingColumnFamilies(true);

            var columnFamilies = new ColumnFamilies
            {
                { "reverse1", new ColumnFamilyOptions() },
                { "reverse2", new ColumnFamilyOptions() },
            };
            var db = TransactionDB.Open(dbop, new TransactionDBOptions(), "transaction_db_test2", columnFamilies);
            var reverse = db.GetColumnFamily("reverse1");
            var wop = new WriteOptions();
            var txnOp = new TransactionOptions();
            string key = "1112";
            var t1 = Task.Run(async () =>
            {
                var txn = new Transaction(db, wop, txnOp);
                await Task.Delay(700);
                Console.WriteLine("11110:" + txn.Get(key, reverse));
                txn.Put(key, "3002", reverse);
                Console.WriteLine("11111:" + txn.Get(key, reverse));
                await Task.Delay(1000);
                Console.WriteLine("11112:" + txn.Get(key, reverse));
                txn.Put(key, "3002", reverse);
                Console.WriteLine("11113:" + txn.Get(key, reverse) + ",thread=" + Thread.CurrentThread.ManagedThreadId);
                txn.Commit();
            });
            var t2 = Task.Run(async () =>
            {
                await Task.Delay(100);
                db.Put(key, "44444", reverse);
                Console.WriteLine("write1:" + db.Get(key, reverse));
                await Task.Delay(1000);
                db.Put(key, "444442", reverse);
                Console.WriteLine("write2:" + db.Get(key, reverse));
            });
            Task.WaitAll(t1, t2);
            Console.WriteLine("end:" + db.Get("11", reverse));
        }
        static void Test7()
        {
            var dbop = new DbOptions()
                .SetCreateIfMissing(true)
                .SetCreateMissingColumnFamilies(true);

            var columnFamilies = new ColumnFamilies
            {
                { "reverse1", new ColumnFamilyOptions() },
                { "reverse2", new ColumnFamilyOptions() },
            };
            var db = TransactionDB.Open(dbop, new TransactionDBOptions(), "transaction_db_test2", columnFamilies);
            var reverse = db.GetColumnFamily("reverse1");
            var r1 = new Random(1);
            var r2 = new Random(2);
            Task.Run(() =>
            {
                int times = 0;
                while (++times <= 1000)
                {
                    db.Put("11", r1.Next(2) == 0 ? "a11" : "a12", reverse);
                    db.Put("22", r1.Next(2) == 0 ? "a22" : "a23", reverse);
                }
            });
            Task.Run(() =>
            {
                int times = 0;
                while (++times <= 1000)
                {
                    db.Put("22", r2.Next(2) == 0 ? "a22" : "a23", reverse);
                    db.Put("11", r2.Next(2) == 0 ? "a11" : "a12", reverse);
                }
            });
            Console.WriteLine(db.Get("11", reverse));
            Console.WriteLine(db.Get("22", reverse));
        }

        static void test6()
        {
            var dbop = new DbOptions()
                .SetCreateIfMissing(true)
                .SetCreateMissingColumnFamilies(true);

            var columnFamilies = new ColumnFamilies
            {
                { "reverse1", new ColumnFamilyOptions() },
                { "reverse2", new ColumnFamilyOptions() },
            };
            var db = TransactionDB.Open(dbop, new TransactionDBOptions(), "transaction_db_test2", columnFamilies);

            var cfs = db.GetColumnFamilyHandleMap();
            foreach (var it in cfs)
            {
                Console.WriteLine("cf=" + it.Key);
            }
            var reverse = db.GetColumnFamily("reverse1");
            db.Put("uno", "one", cf: reverse);
            db.Put("dos", "two", cf: reverse);
            db.Put("tres", "three", cf: reverse);
            db.Dispose();

            db = TransactionDB.Open(new DbOptions().SetCreateIfMissing(true), new TransactionDBOptions(), "transaction_db_test2", columnFamilies);
            reverse = db.GetColumnFamily("reverse1");
            Console.WriteLine(db.Get("uno", reverse));
            Console.WriteLine(db.Get("dos", reverse));
            Console.WriteLine(db.Get("tres", reverse));
            db.Dispose();
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
