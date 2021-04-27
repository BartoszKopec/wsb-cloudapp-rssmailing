using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Services
{
	public class Json
	{
		private static JsonSerializerSettings _settings = new JsonSerializerSettings
		{
		};

		public static string Serialize(object obj)
		{
			string json = JsonConvert.SerializeObject(obj, _settings);
			return json;
		}

		public static T Deserialize<T>(string json)
		{
			T obj = JsonConvert.DeserializeObject<T>(json, _settings);
			return obj;
		}
	}
}
