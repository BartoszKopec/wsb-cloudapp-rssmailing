using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Units
{
	public class SenderMock : IEmailSender
	{
		public async Task<IActionResult> SendEmailAsync(EmailSenderModel email, CancellationToken token)
		{
			await Task.Delay(50, token);
			return new OkObjectResult(new MailingResponseBody { Status = true });
		}
	}
}
