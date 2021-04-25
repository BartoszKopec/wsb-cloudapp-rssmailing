using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
	public class RssResponseBody
	{
		public string Id { get; set; }
		public string AddressEmail { get; set; }
		public List<string> Urls { get; set; }
	}
}
