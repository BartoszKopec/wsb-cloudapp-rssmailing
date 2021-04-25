using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingManager
{
	public static class Initialization
	{
		public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
		{
			string key = configuration[$"EmailSender:Key"];
			services.AddScoped<IEmailSender>((sp) => new SendGridImplementation(key));
			return services;
		}
	}
}
