using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Application.Server.Models
{
	[XmlRoot("item")]
	public class XmlRssItem
	{
		[XmlElement("title")]
		public string Title { get; set; }

		[XmlElement("description")]
		public string Description { get; set; }
	}
}
