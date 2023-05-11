using EventBus.Messages.Common;
using MassTransit;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using System.Reflection;

namespace Ordering.API.Extensions
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationServicesAPI(this IServiceCollection services, IConfiguration config)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
			});

			services.AddMassTransit(configuration =>
			{
				configuration.AddConsumer<BasketCheckoutConsumer>();
				configuration.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(config["EventBusSettings:HostAddress"]);
					cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
					{
						c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
					});
				});
			});
			services.AddMassTransitHostedService();
			services.AddScoped<BasketCheckoutConsumer>();

			return services;
		}
	}

}
