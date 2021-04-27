using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
	internal struct Constants
	{
		public struct ENVVAR
		{
			public const string API_ADDRESS = "API_ADDRESS";
			public const string PORT = "PORT";
			public const string MAILING_KEY = "MAILING_KEY";
			public const string CONNECTION_STRING = "CONNECTION_STRING";
		}

		public const string ROUTE_API_RSS = "api/rss";
		public const string ROUTE_API_MAILING = "api/mailing";
		public const string CONTENTTYPE_JSON = "application/json";
	}
}
