using Application.Data;
using Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			string dbConnectionString = Environment.GetEnvironmentVariable(Constants.ENVVAR.CONNECTION_STRING);
			string mailingKey = Environment.GetEnvironmentVariable(Constants.ENVVAR.MAILING_KEY);

			services.AddHttpClient();
			services.AddDatabase(dbConnectionString);
			services.AddScoped<IEmailSender>((sp) => new SendGridEmailSender(mailingKey));
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
