using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Microsoft.Diagnostics.Runtime;

namespace ReflectAsm
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = Process.Start("TestProcess.exe");
            Thread.Sleep(1000);
            //var proc = Process.GetProcessesByName("TestProcess")[0];
            using (var target = DataTarget.AttachToProcess(proc.Id, 5000))
            {
                var dacLocation = target.ClrVersions[0].TryGetDacLocation();
                var runtime = target.CreateRuntime(dacLocation);

                var module = runtime.AppDomains.SelectMany(ad => ad.Modules).Single(m => m.Name.EndsWith("TestProcess.exe"));
                var program = module.GetTypeByName("TestProcess.Program");
                var method = program.Methods.Single(m => m.Name == "TestMethod");
                var nativeCodeAddress = method.NativeCode;
                var offsetMap = method.ILOffsetMap;
                var finalAddress = offsetMap.Last().startAddress;
                var size = (int)(finalAddress - nativeCodeAddress);

                var bytes = new byte[size];
                int bytesRead;
                target.ReadProcessMemory(nativeCodeAddress, bytes, size, out bytesRead);
                
                for (var i = 0; i < bytesRead; i++)
                {
                    Console.WriteLine(ToHex(bytes[i]));
                }
            }

            proc.Kill();

            Console.ReadLine();
        }

        private static string ToHex(byte b)
        {
            const string hexstring = "0123456789ABCDEF";
            return new string(new [] { hexstring[b / 16], hexstring[b % 16] });
        }
    }
}
