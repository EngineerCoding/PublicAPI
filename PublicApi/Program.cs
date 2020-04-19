using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublicApi.Models;

namespace PublicApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IHost host = CreateHostBuilder(args).Build();

			using (IServiceScope scope = host.Services.CreateScope())
			{
				DbContext context = scope.ServiceProvider.GetService<PublicApiContext>();
				context.Database.Migrate();
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureKestrel(serverOptions =>
					{

					})
					.UseStartup<Startup>();
				});
		}
	}
}
