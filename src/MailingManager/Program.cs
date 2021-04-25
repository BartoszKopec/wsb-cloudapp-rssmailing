using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingManager
{
	public class Program
	{
		public static Task Main(string[] args)
		{
			IEmailSender sender = new SendGridImplementation("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");
			return sender.SendEmailAsync(new Models.Email
			{
				FromAddress = "bartoszkopec0@gmail.com",
				FromName = "From test",
				ToAddress = "bartoszkopec0@gmail.com",
				ToName = "To test",
				Subject = "Test",
				Content = "<h1>Test</h1>"
			}, new System.Threading.CancellationToken());
		}
	}
}
