using System.Collections.Generic;
using System.Xml.Serialization;

namespace Application.Models
{
	[XmlRoot("rss")]
	public class XmlRssRoot
	{
		public class XmlRssChannel
		{
			[XmlElement("title")]
			public string Title { get; set; }
			[XmlElement("item")]
			public List<XmlRssItem> Items { get; set; }
		}

		[XmlElement("channel")]
		public XmlRssChannel Channel { get; set; }
	}

}
