using CoreWCF;
using CoreWCF.Configuration;
using Lab.WCFIsDead.CoreWCF.Shared.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Lab.WCFIsDead.CoreWCF.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseServiceModel(builder =>
            {
                builder.AddService<CalculatorService>();
                builder.AddServiceEndpoint<CalculatorService, ICalculatorService>(new BasicHttpBinding(), "/basichttp");
                builder.AddServiceEndpoint<CalculatorService, ICalculatorService>(new NetTcpBinding(), "/nettcp");
            });
        }

    }
}
