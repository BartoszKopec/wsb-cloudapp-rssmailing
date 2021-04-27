using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
	public static class Init
	{
		public static IServiceCollection AddHttp(this IServiceCollection services)
		{
			services.AddSingleton<IHttp, HttpRest>();
			return services;
		}

	}
}
