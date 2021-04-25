using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Models
{
	public class RssRequestBody : ModelBase
	{
		public string AddressEmail { get; set; }
		public List<string> Urls { get; set; } = new List<string>();

		public override bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(AddressEmail);
		}
	}
}
