using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
	config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
});

builder.Host.ConfigureLogging((hostingContext, loggingbuilder) =>
{
	loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
	loggingbuilder.AddConsole();
	loggingbuilder.AddDebug();
});

var app = builder.Build();
app.UseOcelot();
app.Run();
