using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
	public class HttpRequest
	{
		public Dictionary<string, string> Headers { get; set; }
		public string Url { get; set; }
		public string Content { get; set; }
	}
}
