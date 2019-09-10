using JKang.IpcServiceFramework;
using Lab.WCFIsDead.IpcServiceFramework.Shared;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.IpcServiceFramework.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var pipeClient = new IpcServiceClientBuilder<ICalculatorService>()
                .UseNamedPipe("pipeName") // or .UseTcp(IPAddress.Loopback, 45684) to invoke using TCP
                .Build();

            var result = await pipeClient.InvokeAsync(x => x.Execute(new Calculation()
            {
                Operand1 = 1,
                Operand2 = 2,
                Opertor = "+"
            }));

            Console.WriteLine(result.Result);

            var tcpClient = new IpcServiceClientBuilder<ICalculatorService>()
                .UseTcp(IPAddress.Loopback, 45684)
                .Build();

            result = await tcpClient.InvokeAsync(x => x.Execute(new Calculation()
            {
                Operand1 = 1,
                Operand2 = 2,
                Opertor = "+"
            }));

            Console.WriteLine(result.Result);
        }
    }
}
