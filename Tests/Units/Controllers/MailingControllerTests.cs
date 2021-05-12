using Application;
using Application.Controllers;
using Application.Data;
using Application.Models;
using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace Tests.Units.Controllers
{
	public class MailingControllerTests : ControllerTestsBase<MailingController>
	{
		private CancellationToken _token;

		public MailingControllerTests()
		{
			_token = new CancellationToken();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("    ")]
		public void GetPreview_With_BadQuery(string email)
		{
			//Arrange
			Type expectedType = typeof(BadRequestObjectResult);
			CommonInit();

			//Act
			IActionResult result = Controller.GetPreviewAsync(email, _token).Result;

			//Assert
			Assert.IsType(expectedType, result);
			BadRequestObjectResult badResult = result as BadRequestObjectResult;
			ControllerAssert.IsValidErrorResult(badResult.Value, Strings.ERROR_INVALID_VALUE);
		}

		[Fact]
		public void GetPreview_WhenMail_NotFigured()
		{
			//Arrange
			string emailThatNotFigured = "foobar";
			Type expectedType = typeof(NotFoundObjectResult);
			CommonInit();

			//Act
			IActionResult result = Controller.GetPreviewAsync(emailThatNotFigured, _token).Result;

			//Assert
			Assert.IsType(expectedType, result);
			NotFoundObjectResult notFound = result as NotFoundObjectResult;
			ControllerAssert.IsValidErrorResult(notFound.Value, Strings.ERROR_MAIL_NOTFIGURED);
		}

		[Fact]
		public void GetPreview_WhenMail_Exists()
		{
			//Arrange
			Record<string> record = new Record<string>
			{
				Id = "1",
				AddressEmail = "foobar",
				RssSources = new List<string> { "http://foo.com/bar" }
			}; 
			CommonInit(records: record);

			Type expectedType = typeof(ContentResult);
			string expectedContent = $"<div><table><tr><td>{record.RssSources[0]} {Strings.UNAVAILABLE_FEED}</td></tr></table></div>";
			int expectedStatus = 200;

			//Act
			IActionResult actionResult = Controller.GetPreviewAsync("foobar", _token).Result;

			//Assert
			Assert.IsType(expectedType, actionResult);
			ContentResult result = actionResult as ContentResult;

			Assert.Equal(expectedStatus, result.StatusCode);
			Assert.Equal(Constants.CONTENTTYPE_HTML, result.ContentType);
			Assert.Equal(expectedContent, result.Content);
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData(null, true)]
		[InlineData("", true)]
		[InlineData("    ", true)]
		public void Send_With_BadQuery(string email, bool useAssignedBody)
		{
			//Arrange
			Type expectedType = typeof(BadRequestObjectResult);
			MailingRequestBody requestBody = new()
			{
				AdressEmail = email
			};
			CommonInit();

			//Act
			IActionResult result = Controller.SendAsync(useAssignedBody ? requestBody : null, _token).Result;

			//Assert
			Assert.IsType(expectedType, result);
			BadRequestObjectResult badResult = result as BadRequestObjectResult;
			ControllerAssert.IsValidErrorResult(badResult.Value, Strings.ERROR_INVALID_VALUE);
		}


		[Fact]
		public void Send_WhenMail_Exists()
		{
			//Arrange
			Record<string> record = new Record<string>
			{
				Id = "1",
				AddressEmail = "foobar",
				RssSources = new List<string> { "http://foo.com/bar" }
			};
			MailingRequestBody requestBody = new MailingRequestBody
			{
				AdressEmail = "foobar"
			};
			Type expectedType = typeof(OkObjectResult);
			CommonInit(records: record);

			//Act
			IActionResult result = Controller.SendAsync(requestBody, _token).Result;

			//Assert
			Assert.IsType(expectedType, result);
		}

		[Fact]
		public void Send_WhenMail_NotExists()
		{
			//Arrange
			Type expectedResultType = typeof(NotFoundObjectResult);
			Type expectedMessageObjectType = typeof(Error);
			CommonInit();

			//Act
			MailingRequestBody requestBody = new MailingRequestBody
			{
				AdressEmail = "foobar"
			};
			IActionResult result = Controller.SendAsync(requestBody, _token).Result;

			//Assert
			Assert.IsType(expectedResultType, result);
			NotFoundObjectResult notFound = result as NotFoundObjectResult;
			Assert.NotNull(notFound.Value);
			Assert.IsType(expectedMessageObjectType, notFound.Value);
			Error error = notFound.Value as Error;
			Assert.Equal(Strings.ERROR_MAIL_NOTFIGURED, error.Message);
		}

		protected override void CommonInit(params Record<string>[] records)
		{
			_database = new DatabaseStringIdMock();
			if (records != null)
			{
				_database.Records = records.ToList();
			}
			_sender = new SenderMock();
			Mock<HttpClient> mockHttp = new Mock<HttpClient>();
			mockHttp.Setup((s) => s.SendAsync(new HttpRequestMessage(HttpMethod.Get, It.IsAny<string>()), It.IsAny<CancellationToken>()).Result)
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(
						"<rss><channel><item><title>Tytul 1</title><description>Opis 1</description></item><item><title>Tytul 2</title><description>Opis 2</description></item><item><title>Tytul 3</title><description>Opis 3</description></item></channel></rss>",
						Encoding.UTF8, Application.Constants.CONTENTTYPE_JSON)
				});
			_http = mockHttp.Object;
			Controller = new MailingController(_database, _sender, _http);
		}
	}
}
