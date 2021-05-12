namespace Application
{
	public struct Constants
	{
		public struct ENVVAR
		{
			public const string API_ADDRESS = "API_ADDRESS";
			public const string PORT = "PORT";
			public const string MAILING_KEY = "MAILING_KEY";
			public const string CONNECTION_STRING = "CONNECTION_STRING";
		}

		public const string ROUTE_API_FEED = "api/rss";
		public const string ROUTE_API_MAILING = "api/mailing";
		public const string CONTENTTYPE_JSON = "application/json";
		public const string CONTENTTYPE_HTML = "text/html";
	}
}
