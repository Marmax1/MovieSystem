using MovieSystem.Grpc;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

namespace gRPCWebClient
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Регистрация gRPC-клиента
			builder.Services.AddGrpcClient<MovieService.MovieServiceClient>(options =>
			{
				options.Address = new Uri(builder.Configuration["GrpcServer:Address"]);
			});

			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
