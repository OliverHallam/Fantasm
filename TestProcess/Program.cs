using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TestProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var method = typeof(Program).GetMethod("TestMethod", BindingFlags.Static | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(method.MethodHandle);

            Console.ReadLine();

            TestMethod();
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
