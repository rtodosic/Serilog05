using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Templates;

namespace Serilog05
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog((hostingContect, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContect.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", typeof(Program).Assembly.GetName().Name)
                .WriteTo.Console(new ExpressionTemplate("{ {app_timestamp:@t, message:@m, rendering:@r, level:if @l = 'Debug' then 'DEBUG' else if @l = 'Warning' then 'WARN' else if @l = 'Error' then 'ERR' else if @l = 'Fatal' then 'FTL' else @l, exception:@x, ..@p} }\n"))
            );
    }
}
