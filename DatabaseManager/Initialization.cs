using DatabaseManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManager
{
	public static class Initialization
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
		{
			string[] data = connectionString.Split(';');
			
			DbSettings dbSettings = new()
			{
				ConnectionString = data[0],
				DatabaseName = data[0],
				CollectionName = data[0]
			};
			
			if(string.IsNullOrWhiteSpace(dbSettings.ConnectionString)||
				string.IsNullOrWhiteSpace(dbSettings.DatabaseName) ||
				string.IsNullOrWhiteSpace(dbSettings.CollectionName))
			{
				throw new DatabaseManagerException(DatabaseManagerException.SETTINGS_MISSING);
			}

			services.AddSingleton<IDatabase<string>>((p) => new DatabaseMongo(dbSettings));			
			return services;
		}
	}
}
