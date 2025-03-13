using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using DAL;
using DAL.Repositories;
using BL.Services;
using DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace BL
{
    internal class Program
    {
        static void Main(string[] args)
        {
			string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

			var configuration = new ConfigurationBuilder()
				.SetBasePath(projectDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var builder = WebApplication.CreateBuilder(args);

			// Регистрация DbContext
			builder.Services.AddDbContext<MovieDatabaseContext>(options =>
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Регистрация репозиториев
			builder.Services.AddScoped<MovieRepositoryImpl>();

			// Регистрация сервисов (теперь MyMovieService)
			builder.Services.AddScoped<MyMovieService>();

			// Регистрация gRPC-сервисов
			builder.Services.AddGrpc();

			// Явная настройка Kestrel для HTTPS
			builder.WebHost.ConfigureKestrel(options =>
			{
				options.ListenLocalhost(5001, listenOptions =>
				{
					listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
					listenOptions.UseHttps(); // Включаем HTTPS
				});

				// HTTP (опционально)
				options.ListenLocalhost(5000, listenOptions =>
				{
					listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
				});
			});

			var app = builder.Build();

			// Настройка middleware
			app.MapGrpcService<MovieGrpcService>();

			app.Run();
		}
    }
}
