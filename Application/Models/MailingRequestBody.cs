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
