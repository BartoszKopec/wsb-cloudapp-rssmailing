using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Data
{
	public static class DatabaseInitialization
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
		{
			string[] data = connectionString.Split(';');

			DbSettings dbSettings = new()
			{
				ConnectionString = data[0],
				DatabaseName = data[1],
				CollectionName = data[2]
			};

			if (string.IsNullOrWhiteSpace(dbSettings.ConnectionString) ||
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
