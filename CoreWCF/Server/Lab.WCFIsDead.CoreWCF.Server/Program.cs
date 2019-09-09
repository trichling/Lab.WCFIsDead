using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Lab.WCFIsDead.CoreWCF.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => { options.ListenLocalhost(8080); })
                .UseUrls("http://localhost:8080")
                .UseNetTcp(8808)
                .UseStartup<Startup>();
    }
}
