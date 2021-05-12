using Newtonsoft.Json;

namespace Application.Services
{
	public class Json
	{
		private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
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
