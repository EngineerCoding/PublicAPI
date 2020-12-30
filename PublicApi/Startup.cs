using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublicApi.Infrastructure;
using PublicApi.Models;
using PublicApi.Models.Settings;
using PublicApi.Services;

namespace PublicApi
{
	public class Startup
	{
		public const string AdminCorsPolicy = "AdminCors";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			string issuer = Configuration["Jwt:Authority"];

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.Authority = issuer;
				options.Audience = Configuration["Jwt:Audience"];
			});

			services.AddCors(options =>
			{
				options.AddPolicy(
					AdminCorsPolicy,
					builder =>
					{
						builder.WithOrigins(Configuration["CorsClient"]).AllowAnyHeader().AllowAnyMethod();
					});
			});

			string connectionString = new SqliteConnectionStringBuilder
			{
				DataSource = Configuration["SqliteDatabase"]
			}.ToString();

			services.AddDbContext<PublicApiContext>(options => options.UseSqlite(connectionString));

			services.Configure<BlogPostServiceSettings>(Configuration.GetSection("BlogPost"));

			services.AddScoped<IBlogPostService, BlogPostService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();
			app.UseRouting();
			app.UseCors();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
