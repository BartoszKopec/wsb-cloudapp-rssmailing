using MailingManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailingManager
{
	public interface IEmailSender
	{
		public Task<bool> SendEmailAsync(Email email, CancellationToken token);
	}
}
