using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
	public class MailingRequestBody : ModelBase
	{
		public string AdressEmail { get; set; }

		public override bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(AdressEmail);
		}
	}
}
