using System.Collections.Generic;

namespace Application.Models
{
	public class RssResponseBody
	{
		public string Id { get; set; }
		public string AddressEmail { get; set; }
		public List<string> Urls { get; set; }
	}
}
