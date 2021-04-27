using Application.Models;
using Application.Resources;
using HttpService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ApiClient
	{
		private readonly IHttp _http;
		private readonly string _endpoint_mailing,
			_endpoint_rss;

		public ApiClient(IHttp http)
		{
			_http = http;
			string apiAddress = Environment.GetEnvironmentVariable(Constants.ENVVAR.API_ADDRESS);
			_endpoint_mailing = apiAddress + "/" + Constants.ROUTE_API_MAILING;
			_endpoint_rss = apiAddress + "/" + Constants.ROUTE_API_RSS;
		}

		public string Endpoint_mailing => _endpoint_mailing;
		public string Endpoint_rss => _endpoint_rss;

		public async Task PostSendAsync(MailingRequestBody body, CancellationToken token, Action onSuccess, Action<string> onError)
		{
			HttpRequest request = new()
			{
				Url = Endpoint_mailing,
				Content = Json.Serialize(body)
			};
			HttpResponse response = await _http.PostAsync(request, token);
			if (response.IsSuccessful)
			{
				MailingResponseBody responseBody = Json.Deserialize<MailingResponseBody>(response.Content);
				if (responseBody.Status)
				{
					onSuccess();
				}
				else
				{
					onError(Strings.ERROR_SENDMAIL);
				}
			}
			else
			{
				Error error = Json.Deserialize<Error>(response.Content);
				onError(error.Message);
			}
		}

		public async Task PostSaveAsync(RssRequestBody body, CancellationToken token, Action onSuccess, Action<string> onError)
		{
			HttpRequest request = new()
			{
				Url = Endpoint_rss,
				Content = Json.Serialize(body)
			};
			HttpResponse response = await _http.PostAsync(request, token);
			if (response.IsSuccessful)
			{
				onSuccess();
			}
			else
			{
				Error error = Json.Deserialize<Error>(response.Content);
				onError(error.Message);
			}
		}

		public async Task GetRssUrlsAsync(string email, CancellationToken token, Action<List<string>> onSuccess, Action<string> onError)
		{
			HttpRequest request = new()
			{
				Url = Endpoint_rss + "?email="+email
			};
			HttpResponse response = await _http.GetAsync(request, token);
			if (response.IsSuccessful)
			{
				RssResponseBody rssBody = Json.Deserialize<RssResponseBody>(response.Content);
				onSuccess(rssBody.Urls);
			}
			else
			{
				Error error = Json.Deserialize<Error>(response.Content);
				onError(error.Message);
			}
		}
	}
}
