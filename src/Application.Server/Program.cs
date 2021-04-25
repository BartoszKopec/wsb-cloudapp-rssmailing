using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server
{
	/// <summary>
	/// PORT environment variable has to be set
	/// </summary>

	public class Program
	{
		public static void Main(string[] args)
		{
			string portStr = Environment.GetEnvironmentVariable(Constants.ENVVAR_PORT);
			if(!int.TryParse(portStr, out int port))
			{
				throw new ArgumentException("PORT");
			}

			IHostBuilder builder = Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>()
						.UseKestrel((options)=>
						{
							options.AddServerHeader = false;
							options.ListenAnyIP(port);
						});
				});
			builder.Build().Run();
		}
	}
}
