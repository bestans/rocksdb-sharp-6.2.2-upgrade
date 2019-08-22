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
        static void Main(string[] args)
        {
            test2();
        }
        static void test2()
        {
            var db = TransactionDB.Open(new DbOptions().SetCreateIfMissing(true), new TransactionDBOptions(), "transaction_db_test");
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
