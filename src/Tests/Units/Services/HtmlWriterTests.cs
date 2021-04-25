using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlService;
using Xunit;

namespace Tests.Units.Services
{
	public class HtmlWriterTests
	{
		[Fact]
		public void Test_Correctness_Of_Writing_Html()
		{
			//Arrange
			HtmlMarkup htmlMarkups = HtmlMarkup.New(
				"div",
				HtmlMarkup.New("p", "Hello World!"),
				HtmlMarkup.New("table",
					HtmlMarkup.New("tr",
						HtmlMarkup.New("th", "Header1"),
						HtmlMarkup.New("th", "Header2")
						),
					HtmlMarkup.New("tr",
						HtmlMarkup.New("td", "Row1 Header1"),
						HtmlMarkup.New("td", "Row1 Header2")
						),
					HtmlMarkup.New("tr",
						HtmlMarkup.New("td", "Row2 Header1"),
						HtmlMarkup.New("td", "Row2 Header2")
						)
					)
				);
			string expectedHtml = "<div><p>Hello World!</p><table><tr><th>Header1</th><th>Header2</th></tr><tr><td>Row1 Header1</td><td>Row1 Header2</td></tr><tr><td>Row2 Header1</td><td>Row2 Header2</td></tr></table></div>";

			//Act
			string actualHtml = HtmlWriter.WriteHtml(htmlMarkups);

			//Assert
			Assert.Equal(expectedHtml, actualHtml);
		}
	}
}
