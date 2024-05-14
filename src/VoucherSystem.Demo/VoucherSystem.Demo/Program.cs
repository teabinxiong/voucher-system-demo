
using Serilog;
using System.Runtime.ConstrainedExecution;
using VoucherSystem.Demo;
using VoucherSystem.Demo.ApplicationServices;

var builder = new HostBuilder()
         .ConfigureAppConfiguration((hostingContext, config) =>
         {
             config.AddJsonFile("appsettings.json", optional: true);
             config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
             config.AddEnvironmentVariables();
             if (args != null)
             {
                 config.AddCommandLine(args);
             }
         })
        .ConfigureServices(s =>
        {
            s.AddSingleton<VoucherSystem.Demo.ApplicationServices.BackgroundService>();
            s.AddSingleton<ServicesManager>();
        })
         .UseSerilog();


using (IHost host = builder.Build())
{
    
    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(host.Services.GetRequiredService<IConfiguration>())
    .MinimumLevel.Verbose()
    .CreateLogger();

    Global.Logger = Log.Logger;
    Global.Logger.Information("Start....");

    var svc = host.Services.GetRequiredService<VoucherSystem.Demo.ApplicationServices.BackgroundService>();

    host.Start();
    svc.Start();


    host.WaitForShutdown();
    Global.Logger.Information("Received close Signal");
    Console.WriteLine("Received close Signal");
    svc.Stop();

}

Global.Logger.Information("Returning exit code from Program.Main.");
return 123;
