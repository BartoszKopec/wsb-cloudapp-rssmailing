using Application.Data;
using Application.Models;
using Application.Resources;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Application.Controllers
{
	[Route(Constants.ROUTE_API_MAILING)]
	[ApiController]
	[Produces(Constants.CONTENTTYPE_JSON)]
	public class MailingController : ControllerBase
	{
		private readonly IDatabase<string> _database;
		private readonly IEmailSender _emailSender;
		private readonly HttpClient _httpClient;

		public MailingController(IDatabase<string> database, IEmailSender emailSender, HttpClient http)
		{
			_database = database;
			_emailSender = emailSender;
			_httpClient = http;
		}

		[HttpGet]
		public async Task<IActionResult> GetPreviewAsync([FromQuery] string email, CancellationToken token = default)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return BadRequest(Error.New(Strings.ERROR_INVALID_VALUE));
			}

			string emailMsg = await GetEmailMessage(email, token);
			if (emailMsg is null)
			{
				return NotFound(Error.New(Strings.ERROR_MAIL_NOTFIGURED));
			}

			return new ContentResult
			{
				Content = emailMsg,
				StatusCode = 200,
				ContentType = Constants.CONTENTTYPE_HTML
			};
		}

		[HttpPost]
		public async Task<IActionResult> SendAsync([FromBody] MailingRequestBody body, CancellationToken token = default)
		{
			if (body is null || !body.IsValid())
			{
				return BadRequest(Error.New(Strings.ERROR_INVALID_VALUE));
			}

			string emailMsg = await GetEmailMessage(body.AdressEmail, token);
			if (emailMsg is null)
			{
				return NotFound(Error.New(Strings.ERROR_MAIL_NOTFIGURED));
			}

			EmailSenderModel email = new()
			{
				FromAddress = "bartoszkopec0@gmail.com",
				FromName = "Test receiver",
				ToAddress = body.AdressEmail,
				ToName = body.AdressEmail,
				Subject = "Test topic",
				Content = emailMsg
			};
			IActionResult sendResult = await _emailSender.SendEmailAsync(email, token);
			return sendResult;
		}

		private async Task<string> GetEmailMessage(string email, CancellationToken token)
		{
			Record<string> record = await _database.GetAsyncBy((r) => r.AddressEmail == email, token);
			if (record is null)
			{
				return null;
			}

			List<HtmlMarkup> htmlTableRows = new();

			//rssUrl: https://www.konflikty.pl/feed/
			foreach (string rssUrl in record.RssSources)
			{
				HttpResponseMessage response = await _httpClient.GetAsync(rssUrl, token);
				if (!response.IsSuccessStatusCode)
				{
					htmlTableRows.Add(HtmlMarkup.New("tr", HtmlMarkup.New("td", $"{rssUrl} {Strings.UNAVAILABLE_FEED}")));
					continue;
				}
				string xml = await response.Content.ReadAsStringAsync(token);
				XmlSerializer serializer = new(typeof(XmlRssRoot));
				using TextReader reader = new StringReader(xml);
				XmlRssRoot result = (XmlRssRoot)serializer.Deserialize(reader);
				htmlTableRows.Add(HtmlMarkup.New("th", result.Channel.Title));
				for (int i = 0; i < result.Channel.Items.Count; i++)
				{
					XmlRssItem item = result.Channel.Items[i];
					htmlTableRows.Add(HtmlMarkup.New("tr", HtmlMarkup.New("td", $"{item.Title}<br/>{item.Description}")));
				}
			}

			HtmlMarkup content = HtmlMarkup.New("div", HtmlMarkup.New("table", htmlTableRows.ToArray()));
			string html = HtmlWriter.WriteHtml(content);
			return html;
		}

	}
}
