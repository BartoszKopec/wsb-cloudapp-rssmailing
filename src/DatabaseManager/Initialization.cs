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
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			string key = nameof(DbSettings);
			string keyConnStr = nameof(DbSettings.ConnectionString);
			string keyDatabase = nameof(DbSettings.DatabaseName);
			string keyCollection = nameof(DbSettings.CollectionName);
			
			DbSettings dbSettings = new()
			{
				ConnectionString = configuration[$"{key}:{keyConnStr}"],
				DatabaseName = configuration[$"{key}:{keyDatabase}"],
				CollectionName = configuration[$"{key}:{keyCollection}"]
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
