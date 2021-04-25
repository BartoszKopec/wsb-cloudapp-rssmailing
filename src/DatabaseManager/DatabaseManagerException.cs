using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManager
{
	public class DatabaseManagerException : Exception
	{
		public const string SETTINGS_MISSING = "Database settings are missing in configuration file.";

		public DatabaseManagerException(string message) : base(message){}
	}
}
