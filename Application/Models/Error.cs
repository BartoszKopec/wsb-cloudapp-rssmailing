namespace Application.Models
{
	public class Error
	{
		public static Error New(string message) => new Error { Message = message };

		public string Message { get; set; }
	}
}
