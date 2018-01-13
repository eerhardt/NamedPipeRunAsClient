using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeServerStreamBug.Server
{
    class Server
    {
        static ManualResetEvent waitHandle1 = new ManualResetEvent(false);
        static ManualResetEvent waitHandle2 = new ManualResetEvent(false);
        static void Main()
        {
            MainAsync().Wait();

            Console.WriteLine("Done. Press Enter.");
            Console.ReadLine();
        }
        static async Task MainAsync()
        {
            var backgroundTask = Task.Run((Action)BackgroundThread);
            NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream("BugTest");
            await namedPipeServerStream.WaitForConnectionAsync();

            byte[] buffer = new byte[1024];
            await namedPipeServerStream.ReadAsync(buffer, 0, buffer.Length);
            namedPipeServerStream.RunAsClient(ImpersonationWorker);
            await backgroundTask;
        }

        static void ImpersonationWorker()
        {
            waitHandle1.Set();
            Console.WriteLine($"This should be run as the client. Running as '{Environment.UserName}'.");
            waitHandle2.WaitOne();

            Console.WriteLine("ImpersonationWorker is done.");
        }

        static void BackgroundThread()
        {
            Console.WriteLine($"BackgroundThread started. Running as: '{Environment.UserName}'.");
            waitHandle1.WaitOne();
            Console.WriteLine($"BackgroundThread continued. Running as: '{Environment.UserName}'.");
            waitHandle2.Set();
        }
    }
}
