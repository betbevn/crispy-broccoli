using Discount.Grpc.Repositories;
using System.Reflection;

namespace Discount.Grpc.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddScoped<IDiscountRepository, DiscountRepository>();
			return services;
		}
	}
}