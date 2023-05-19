using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
    })
    .ConfigureLogging((hostingContext, loggingBuilder) =>
    {
        loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });

builder.ConfigureServices(services => {
    services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
}).ConfigureWebHostDefaults(webBuilder => {
         webBuilder.Configure((configureApp) => {
             configureApp.UseOcelot();
         });
     });


var app = builder.Build();


//app.MapGet("/", () => "Hello World!");

app.Run();
