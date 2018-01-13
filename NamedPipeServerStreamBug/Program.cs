using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeServerStreamBug
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            NamedPipeClientStream pipe = new NamedPipeClientStream("BugTest");
            await pipe.ConnectAsync();
            await pipe.WriteAsync(Encoding.UTF8.GetBytes("hello"), 0, Encoding.UTF8.GetByteCount("hello"));
        }
    }
}
