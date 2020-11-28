using GithubSearch.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubSearch.CaptureWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    ApplicationEnvironments.Site = hostContext.Configuration.GetSection("SiteConfig").Get<SiteConfig>();
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        ApplicationEnvironments.Site.MongoDB = ApplicationEnvironments.Site.MongoDB_Develop;
                    }
                    services.AddHostedService<Worker>();
                });
    }
}
