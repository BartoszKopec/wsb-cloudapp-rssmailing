using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IEmailSender
	{
		public Task<IActionResult> SendEmailAsync(EmailSenderModel email, CancellationToken token);
	}
}
