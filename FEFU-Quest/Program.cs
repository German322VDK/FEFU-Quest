using FEFU_Quest;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

internal class Program
{
    public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
             webBuilder.UseStartup<Startup>()
        )
        .UseSerilog((host, log) => log
                   .ReadFrom.Configuration(host.Configuration)
                   .MinimumLevel.Information()
                   .Enrich.FromLogContext()
                    .WriteTo.File($"Logs/Info/Full-FEFU_Quest[ {DateTime.Now:yyyy-MM-dd}].log", LogEventLevel.Information)
                    .WriteTo.File($"Logs/Error/FEFU_Quest[ {DateTime.Now:yyyy-MM-dd}].log", LogEventLevel.Error)

            );
}