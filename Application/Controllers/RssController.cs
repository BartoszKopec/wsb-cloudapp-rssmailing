using Application.Data;
using Application.Models;
using Application.Resources;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[Route(Constants.ROUTE_API_FEED)]
	[ApiController]
	[Produces(Constants.CONTENTTYPE_JSON)]
	public class RssController : ControllerBase
	{
		private readonly IDatabase<string> _database;

		public RssController(IDatabase<string> database)
		{
			_database = database;
		}

		[HttpGet]
		public async Task<IActionResult> GetDataAsync([FromQuery] string email, CancellationToken token = default)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return BadRequest(Error.New(Strings.ERROR_INVALID_VALUE));
			}

			Record<string> record = await _database.GetAsyncBy((r) => r.AddressEmail == email, token);
			if (record is null)
			{
				return NotFound(Error.New(Strings.ERROR_MAIL_NOTFIGURED));
			}

			RssResponseBody response = new RssResponseBody
			{
				Id = record.Id,
				AddressEmail = record.AddressEmail,
				Urls = record.RssSources
			};
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> InsertOrUpdateAsync([FromBody] RssRequestBody body, CancellationToken token = default)
		{
			if (body is null || !body.IsValid())
			{
				return BadRequest(Error.New(Strings.ERROR_INVALID_VALUE));
			}

			Record<string> record = await _database.GetAsyncBy((r) => r.AddressEmail == body.AddressEmail, token);
			if (record is null)
			{
				await _database.AddAsync(new Record<string>
				{
					AddressEmail = body.AddressEmail,
					RssSources = body.Urls
				}, token);
			}
			else
			{
				record.RssSources = body.Urls;
				await _database.UpdateAsync(record, token);
			}

			return Ok("OK");
		}
	}
}
