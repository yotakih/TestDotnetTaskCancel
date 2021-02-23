using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestThread
{
    class Program
    {
        static CancellationTokenSource cs = null;
        static void Main(string[] args)
        {
            Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
            proc();
            Console.WriteLine("Enter to Task Cancel");
            Console.ReadLine();
            cs.Cancel();
            Console.WriteLine("Please Enter to End");
            Console.ReadLine();
        }

        static void proc()
        {
            cs = new CancellationTokenSource();
            var ct = cs.Token;
            Task.Run(() => {
                Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine($"looping: {i+1}");
                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine($"Task is stopped");
                        ct.ThrowIfCancellationRequested();
                    }
                    Thread.Sleep(1000);
                }
            }, cs.Token);
        }
    }
}
