using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            FooBar fooBar = new FooBar(5);

            var taskFoo = Task.Run(() => fooBar.Foo(() => Console.WriteLine("foo")));
            var taskBar = Task.Run(() => fooBar.Bar(() => Console.WriteLine("bar")));

            Task.WaitAll(taskFoo, taskBar);
            Console.WriteLine("All threads are complete");
        }
    }

    public class FooBar
    {
        AutoResetEvent _blockThread1 = new AutoResetEvent(false);
        AutoResetEvent _blockThread2 = new AutoResetEvent(true);

        private int n;

        public FooBar(int n)
        {
            this.n = n;
        }

        public void Foo(Action printFoo)
        {

            for (int i = 0; i < n; i++)
            {
                _blockThread2.WaitOne();

                // printFoo() outputs "foo". Do not change or remove this line.
                printFoo();

                _blockThread1.Set();
            }
        }

        public void Bar(Action printBar)
        {

            for (int i = 0; i < n; i++)
            {
                _blockThread1.WaitOne();

                // printBar() outputs "bar". Do not change or remove this line.
                printBar();

                _blockThread2.Set();
            }
        }
    }
}
