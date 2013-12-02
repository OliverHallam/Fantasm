using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TestProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var waitName = args[0];

            var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, waitName);

            Console.WriteLine("Jitting method");

            var method = typeof(Program).GetMethod("TestMethod", BindingFlags.Static | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(method.MethodHandle);

            Console.WriteLine("Signalling debugger process");

            waitHandle.Set();
            waitHandle.Reset();

            Console.WriteLine("Waiting for debugger");

            waitHandle.WaitOne();
        }

        private static void TestMethod()
        {
            int sum = 0;
            for (var i = 0; i < 10; i++)
            {
                sum += i;
            }
            Console.WriteLine(sum);
        }
    }
}
