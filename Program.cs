using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestThread
{
    class Program
    {
        static CancellationTokenSource cs = null;
        static object cslogobj = new object();
        static void Main(string[] args)
        {
            Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
            // var ret = proc().Result;
            proc();
            Console.WriteLine("Enter to Task Cancel");
            Console.ReadLine();
            if (!(cs is null))
                cs.Cancel();
            else
                Console.WriteLine($"Task is already End");
            Console.WriteLine("Please Enter to End");
            Console.ReadLine();
        }

        static async Task<int> proc()
        {
            cs = new CancellationTokenSource();
            try
            {
                await Task.Run(() =>
                {
                    new SubProc(cs).proc();
                }, cs.Token);
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} throw with Message: {e.Message}");
            }
            finally
            {
                cs.Dispose();
                cs = null;
            }
            return 1;
        }
    }
    public class SubProc
    {
        public CancellationTokenSource cs = null;

        public SubProc(CancellationTokenSource _cs)
        {
            this.cs = _cs;
        }

        public void proc()
        {
            Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    Console.WriteLine($"looping: {i + 1}");
                    if (cs.Token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Task is stopped");
                        cs.Token.ThrowIfCancellationRequested();
                    }
                    Thread.Sleep(1000);
                }
                catch (OperationCanceledException e)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"catch the error: {e.Message}");
                }
            }
        }
    }
}
