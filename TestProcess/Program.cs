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
            Console.WriteLine(string.Join(";", args));

            var fileName = args[0];
            var typeName = args[1];
            var methodName = args[2];
            var waitName = args[3];

            if (args.Length != 4)
                return;

            var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, waitName);

            Console.WriteLine("Jitting method");

            var assembly = Assembly.LoadFile(fileName);
            var type = assembly.GetType(typeName);
            var method = type.GetMethod(methodName);
            RuntimeHelpers.PrepareMethod(method.MethodHandle);

            Console.WriteLine("Signalling debugger process");

            waitHandle.Set();
            waitHandle.Reset();

            Console.WriteLine("Waiting for debugger");

            waitHandle.WaitOne(5000);
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
