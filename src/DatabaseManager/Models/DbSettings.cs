using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManager.Models
{
	public class DbSettings
	{
		public string CollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
}
