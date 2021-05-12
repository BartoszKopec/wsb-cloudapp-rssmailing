using Application.Models;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
	class SendGridEmailSender : IEmailSender
	{
		private readonly string _apiKey;

		public SendGridEmailSender(string apiKey) => _apiKey = apiKey;

		public async Task<IActionResult> SendEmailAsync(EmailSenderModel email, CancellationToken token)
		{
			SendGridClient client = new(_apiKey);
			EmailAddress from = new(email.FromAddress, email.FromName);
			EmailAddress to = new(email.ToAddress, email.ToName);
			SendGridMessage msg = MailHelper.CreateSingleEmail(
				from, to, email.Subject,
				plainTextContent: null,
				htmlContent: email.Content);
			Response response = await client.SendEmailAsync(msg, token);
			string responseMessage = await response.Body.ReadAsStringAsync(token);
			return new OkObjectResult(new MailingResponseBody
			{
				Status = response.IsSuccessStatusCode,
				Message = responseMessage
			});
		}
	}
}
