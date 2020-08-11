using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenFTTH.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ServicesConfig.CreateHostBuilder(args).Build().Run();

            Host.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((hostingContext, config) =>
           {
               config.AddJsonFile("appsettings.json", true, true);
               config.AddEnvironmentVariables();
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
           })
           .Build()
           .Run();
        }
    }
}
