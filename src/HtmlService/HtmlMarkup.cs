using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlService
{
	public class HtmlMarkup
	{
		public static HtmlMarkup New(string markup, params HtmlMarkup[] children)
		{
			HtmlMarkup element = new HtmlMarkup
			{
				Markup = markup,
				Children = children.ToList()
			};
			return element;
		}
		public static HtmlMarkup New(string markup, string textContent)
		{
			HtmlMarkup element = new HtmlMarkup
			{
				Markup = markup,
				TextContent = textContent
			};
			return element;
		}

		public string Markup { get; set; }
		public string TextContent { get; set; }
		public List<HtmlMarkup> Children { get; set; } = new List<HtmlMarkup>();
		public bool HasTextContent => !string.IsNullOrWhiteSpace(TextContent);
	}
}
