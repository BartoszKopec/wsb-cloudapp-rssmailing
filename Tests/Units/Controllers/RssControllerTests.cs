using Application.Controllers;
using Application.Data;
using Application.Models;
using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Tests.Units.Controllers
{
	public class RssControllerTests : ControllerTestsBase<RssController>
	{
		private CancellationToken _token;

		public RssControllerTests()
		{
			_token = new CancellationToken();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("    ")]
		public void Get_With_BadQuery(string value)
		{
			//Arrange
			CommonInit();
			Assert.Empty(_database.Records);

			//Act
			IActionResult result = Controller.GetDataAsync(value, _token).Result;

			//Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.NotNull(result);
			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			var responseBody = badRequestObjectResult.Value;
			ControllerAssert.IsValidErrorResult(responseBody, Strings.ERROR_INVALID_VALUE);
		}

		[Fact]
		public void Get_Record_That_NotExists()
		{
			//Arrange
			CommonInit();
			Assert.Empty(_database.Records);

			//Act
			IActionResult result = Controller.GetDataAsync("foobar", _token).Result;

			//Assert
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.NotNull(result);
			NotFoundObjectResult notFoundObjectResult = result as NotFoundObjectResult;
			var responseBody = notFoundObjectResult.Value;

			Assert.NotNull(responseBody);
			Assert.IsType<Error>(responseBody);
			Error body = responseBody as Error;
			Assert.False(string.IsNullOrWhiteSpace(body.Message));
			Assert.Equal(body.Message, Strings.ERROR_MAIL_NOTFIGURED);
		}

		[Fact]
		public void Get_ExistingRecord()
		{
			//Arrange
			Record<string> record = new()
			{
				Id = Guid.NewGuid().ToString(),
				AddressEmail = "foobar",
				RssSources = new List<string> { "url" }
			};
			CommonInit(records: record);
			Assert.Single(_database.Records);

			//Act
			IActionResult result = Controller.GetDataAsync("foobar", _token).Result;

			//Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Single(_database.Records);
			Assert.NotNull(result);
			OkObjectResult okObjectResult = result as OkObjectResult;
			var responseBody = okObjectResult.Value;

			Assert.NotNull(responseBody);
			Assert.IsType<RssResponseBody>(responseBody);
			RssResponseBody body = responseBody as RssResponseBody;
			Assert.False(string.IsNullOrWhiteSpace(body.Id));
			Assert.Equal(body.AddressEmail, record.AddressEmail);
			Assert.Equal(body.Urls, record.RssSources);
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData(null, true)]
		[InlineData("", true)]
		[InlineData("    ", true)]
		public void InsertRss_With_BadQuery(string email, bool useAssignedBody)
		{
			//Arrange
			RssRequestBody body = new RssRequestBody
			{
				AddressEmail = email
				//RssUrls omnitted - might be null or empty
			};
			CommonInit();
			Assert.Empty(_database.Records);

			//Act
			IActionResult result = Controller.InsertOrUpdateAsync(useAssignedBody ? body : null, _token).Result;

			//Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.NotNull(result);
			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			var responseBody = badRequestObjectResult.Value;
			ControllerAssert.IsValidErrorResult(responseBody, Strings.ERROR_INVALID_VALUE);
		}

		[Fact]
		public void InsertRss_With_NewRecord()
		{
			//Arrange
			RssRequestBody body = new()
			{
				AddressEmail = "foobar",
				Urls = new List<string> { "url" }
			};
			CommonInit();
			Assert.Empty(_database.Records);

			//Act
			IActionResult result = Controller.InsertOrUpdateAsync(body, _token).Result;

			//Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Single(_database.Records);
			Record<string> record = _database.Records.FirstOrDefault();
			Assert.False(string.IsNullOrWhiteSpace(record.Id));
			Assert.Equal(body.AddressEmail, record.AddressEmail);
			Assert.Equal(body.Urls, record.RssSources);
		}

		[Fact]
		public void InsertRss_With_ExistingRecord()
		{
			//Arrange
			RssRequestBody body = new()
			{
				AddressEmail = "foobar",
				Urls = new List<string> { "url", "url2" }
			};
			Record<string> recordBeforeUpdate = new()
			{
				Id = Guid.NewGuid().ToString(),
				AddressEmail = "foobar",
				RssSources = new List<string> { "url" }
			};
			CommonInit(records: recordBeforeUpdate);
			Assert.Single(_database.Records);

			//Act
			IActionResult result = Controller.InsertOrUpdateAsync(body, _token).Result;

			//Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Single(_database.Records);
			Record<string> record = _database.Records.FirstOrDefault();
			Assert.Equal(record.Id, recordBeforeUpdate.Id);
			Assert.Equal(body.AddressEmail, record.AddressEmail);
			Assert.Equal(body.Urls, record.RssSources);
		}

		protected override void CommonInit(params Record<string>[] records)
		{
			_database = new();
			if (records != null)
			{
				_database.Records = records.ToList();
			}
			Controller = new RssController(_database);
		}
	}
}
