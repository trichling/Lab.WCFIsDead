using JKang.IpcServiceFramework;
using Lab.WCFIsDead.IpcServiceFramework.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace Lab.WCFIsDead.IpcServiceFramework.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // configure DI
            IServiceCollection services = ConfigureServices(new ServiceCollection());

            // build and run service host
            new IpcServiceHostBuilder(services.BuildServiceProvider())
                .AddNamedPipeEndpoint<ICalculatorService>(name: "pipe", pipeName: "pipeName")
                .AddTcpEndpoint<ICalculatorService>(name: "tcp", ipEndpoint: IPAddress.Loopback, port: 45684)
                .Build()
                .Run();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddIpc(builder =>
                {
                    builder
                        .AddTcp()
                        .AddNamedPipe()
                        .AddService<ICalculatorService, CalculatorService>();
                });
        }
    }
}
