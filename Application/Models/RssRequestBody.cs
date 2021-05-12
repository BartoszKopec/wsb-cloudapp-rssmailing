using System.Collections.Generic;

namespace Application.Models
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
