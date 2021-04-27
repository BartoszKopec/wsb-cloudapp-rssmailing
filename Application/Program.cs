using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Application
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CheckEnvironmnetVariables();

			string portStr = Environment.GetEnvironmentVariable(Constants.ENVVAR.PORT);
			if(!int.TryParse(portStr, out int port))
			{
				throw new ArgumentException(portStr);
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

		private static void CheckEnvironmnetVariables()
		{
			Type tEnvVar = typeof(Constants.ENVVAR);
			List<string> envVarsNames = tEnvVar.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
				.Select(x => (string)x.GetRawConstantValue())
				.ToList();
			foreach (string envVarName in envVarsNames)
			{
				string envVar = Environment.GetEnvironmentVariable(envVarName);
				if (string.IsNullOrWhiteSpace(envVar))
				{
					throw new ArgumentException(envVarName);
				}
			}
		}
	}
}
