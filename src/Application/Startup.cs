using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DatabaseManager;
using MailingManager;
using HttpService;
using Microsoft.Extensions.Configuration;
using Application.Services;

namespace Application
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttp();
			services.AddDatabase(Configuration);
			services.AddEmailSender(Configuration);
			services.AddSingleton<ApiClient>();

			services.AddControllers();
			services.AddRazorPages((config) =>
			{
				config.RootDirectory = "/Client/Pages";
			});
			services.AddServerSideBlazor();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
