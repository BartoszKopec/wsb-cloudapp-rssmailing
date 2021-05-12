using System.Xml.Serialization;

namespace Application.Models
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
