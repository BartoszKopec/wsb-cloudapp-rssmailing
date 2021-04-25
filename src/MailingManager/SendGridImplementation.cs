using MailingManager.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailingManager
{
	class SendGridImplementation : IEmailSender
	{
		private readonly string _apiKey;

		public SendGridImplementation(string apiKey) => _apiKey = apiKey;

		public async Task<bool> SendEmailAsync(Email email, CancellationToken token)
		{
			SendGridClient client = new(_apiKey);
			EmailAddress from = new(email.FromAddress, email.FromName);
			EmailAddress to = new(email.ToAddress, email.ToName);
			SendGridMessage msg = MailHelper.CreateSingleEmail(
				from, to, email.Subject,
				plainTextContent: null,
				htmlContent: email.Content);
			Response response = await client.SendEmailAsync(msg, token);
			return response.IsSuccessStatusCode;
		}
	}
}
