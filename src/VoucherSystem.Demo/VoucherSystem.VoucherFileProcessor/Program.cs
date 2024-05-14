
using Quartz;
using Serilog;
using StackExchange.Redis;
using System.Runtime.ConstrainedExecution;
using VoucherSystem.VoucherFileProcessor;
using VoucherSystem.VoucherFileProcessor.ApplicationServices;
using VoucherSystem.VoucherFileProcessor.ApplicationServices.WorkerServices;
using VoucherSystem.VoucherFileProcessor.Cache;
using VoucherSystem.VoucherFileProcessor.Schedulers;

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
        .ConfigureServices((hostContext, s) =>
        {
            // Access IConfiguration within ConfigureServices scope
            var configuration = hostContext.Configuration;

            s.AddSingleton<VoucherSystem.VoucherFileProcessor.ApplicationServices.BackgroundService>();
            s.AddSingleton<ServicesManager>();
            s.AddTransient<VoucherConsumerSimulationWorker>();

            s.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString);
            });

            s.AddScoped<IDatabase>(provider =>
            {
                var redis = provider.GetRequiredService<IConnectionMultiplexer>();
                return redis.GetDatabase();
            });

            s.AddScoped<IRedisService, RedisService>();


            s.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessVoucherInputFilesJob));

                configure
                    .AddJob<ProcessVoucherInputFilesJob>(jobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(jobKey).WithCronSchedule(
                            configuration.GetSection("SchedulerCronExpression").Value
                            )
                       );


                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            s.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
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

    var svc = host.Services.GetRequiredService<VoucherSystem.VoucherFileProcessor.ApplicationServices.BackgroundService>();

    host.Start();
    svc.Start();


    host.WaitForShutdown();
    Global.Logger.Information("Received close Signal");
    Console.WriteLine("Received close Signal");
    svc.Stop();

}

Global.Logger.Information("Returning exit code from Program.Main.");
return 123;
