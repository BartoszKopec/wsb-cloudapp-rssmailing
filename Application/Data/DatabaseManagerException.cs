using System;

namespace Application.Data
{
	public class DatabaseManagerException : Exception
	{
		public const string SETTINGS_MISSING = "Database settings are missing in configuration file.";

		public DatabaseManagerException(string message) : base(message) { }
	}
}
