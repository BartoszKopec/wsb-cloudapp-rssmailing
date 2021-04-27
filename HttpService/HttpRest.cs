using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpService
{
	internal class HttpRest : IHttp
	{
		private readonly HttpClient _client = new();

		public Task<HttpResponse> GetAsync(HttpRequest request, CancellationToken token)
		{
			return SendAsync(HttpMethod.Get, request, token);
		}

		public Task<HttpResponse> PostAsync(HttpRequest request, CancellationToken token)
		{
			return SendAsync(HttpMethod.Post, request, token);
		}

		private async Task<HttpResponse> SendAsync(HttpMethod method, HttpRequest request, CancellationToken token)
		{
			HttpRequestMessage requestMessage = new(method, request.Url);
			if (request.Headers != null)
			{
				foreach ((string headerKey, string headerValue) in request.Headers)
				{
					requestMessage.Headers.Add(headerKey, headerValue);
				}
			}
			if(request.Content != null)
			{
				requestMessage.Content = new StringContent(request.Content, Encoding.UTF8, "application/json");
			}
			HttpResponseMessage responseMessage = await _client.SendAsync(requestMessage, token);
			HttpResponse response = new()
			{
				Status = (int)responseMessage.StatusCode,
				Content = await responseMessage.Content.ReadAsStringAsync(token)
			};
			return response;
		}
	}
}
