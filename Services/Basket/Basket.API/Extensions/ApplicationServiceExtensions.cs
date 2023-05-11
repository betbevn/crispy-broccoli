using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Basket.API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddControllers();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
			});
			services.AddScoped<IBasketRepository, BasketRepository>();

			// Redis Configuration
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = config.GetValue<string>("CacheSettings:ConnectionString");
			});

			// MassTransit-RabbitMQ Configuration
			services.AddMassTransit(configuration => {
				configuration.UsingRabbitMq((ctx, cfg) => {
					cfg.Host(config["EventBusSettings:HostAddress"]);
				});
			});
			services.AddMassTransitHostedService();

			// Grpc Configuration
			services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
				(o => o.Address = new Uri(config["GrpcSettings:DiscountUrl"]));
			services.AddScoped<DiscountGrpcService>();

			return services;
		}
	}
}