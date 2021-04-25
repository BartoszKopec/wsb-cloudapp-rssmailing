using MailingManager;
using MailingManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Units
{
	public class SenderMock : IEmailSender
	{
		public Task<bool> SendEmailAsync(Email email, CancellationToken token)
		{
			return Task.FromResult(true);
		}
	}
}
