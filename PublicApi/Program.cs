using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublicApi.Models;
using Serilog;
using System;
using System.IO;

namespace PublicApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			IHost host;
			try
			{
				Log.Logger.Information("App is initializing");
				host = CreateHostBuilder(args).Build();
				using (IServiceScope scope = host.Services.CreateScope())
				{
					DbContext context = scope.ServiceProvider.GetService<PublicApiContext>();
					context.Database.Migrate();
				}

				Log.Logger.Information("App is initialized");
			}
			catch (Exception e)
			{
				Log.Logger.Error(e, "Exception occurred during app initialization");
				throw;
			}

			try
			{
				host.Run();
			}
			catch (Exception e)
			{
				Log.Logger.Error(e, "Exception occurred during in app");
				throw;
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureKestrel(action => { })
						.UseStartup<Startup>();
				});
		}
	}
}
