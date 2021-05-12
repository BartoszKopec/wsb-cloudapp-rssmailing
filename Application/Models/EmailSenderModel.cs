namespace Application.Models
{
	public class EmailSenderModel
	{
		public string ToAddress { get; set; }
		public string ToName { get; set; }
		public string FromAddress { get; set; }
		public string FromName { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
	}
}
