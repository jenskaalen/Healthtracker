using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Healthtracker.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)

                // custom config file
                .AddJsonFile("integrationconfig.json", optional: false, reloadOnChange: false)
                .Build();

            CreateWebHostBuilder(args, configuration).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>();

        
    }
}
