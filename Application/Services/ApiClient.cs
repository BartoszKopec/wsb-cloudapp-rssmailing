using Application.Models;
using Application.Resources;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ApiClient
	{
		private readonly HttpClient _http;
		private readonly string _endpoint_mailing,
			_endpoint_feed;

		public ApiClient(HttpClient http)
		{
			_http = http;
			string apiAddress = Environment.GetEnvironmentVariable(Constants.ENVVAR.API_ADDRESS);
			_endpoint_mailing = apiAddress + "/" + Constants.ROUTE_API_MAILING;
			_endpoint_feed = apiAddress + "/" + Constants.ROUTE_API_FEED;
		}

		public string Endpoint_mailing => _endpoint_mailing;
		public string Endpoint_feed => _endpoint_feed;

		public async Task PostSendAsync(MailingRequestBody body, CancellationToken token, Action onSuccess, Action<string> onError)
		{
			HttpResponseMessage response = await _http.PostAsync(Endpoint_mailing, GetRequestContent(body), token);
			string responseContent = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
			{
				MailingResponseBody responseBody = Json.Deserialize<MailingResponseBody>(responseContent);
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
				Error error = Json.Deserialize<Error>(responseContent);
				onError(error.Message);
			}
		}

		public async Task PostSaveAsync(RssRequestBody body, CancellationToken token, Action onSuccess, Action<string> onError)
		{
			HttpResponseMessage response = await _http.PostAsync(Endpoint_feed, GetRequestContent(body), token);
			if (response.IsSuccessStatusCode)
			{
				onSuccess();
			}
			else
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Error error = Json.Deserialize<Error>(responseContent);
				onError(error.Message);
			}
		}

		public async Task GetRssUrlsAsync(string email, CancellationToken token, Action<List<string>> onSuccess, Action<string> onError)
		{
			HttpResponseMessage response = await _http.GetAsync(Endpoint_feed + "?email=" + email, token);
			string responseContent = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
			{
				RssResponseBody rssBody = Json.Deserialize<RssResponseBody>(responseContent);
				onSuccess(rssBody.Urls);
			}
			else
			{
				Error error = Json.Deserialize<Error>(responseContent);
				onError(error.Message);
			}
		}

		private HttpContent GetRequestContent(object o) => new StringContent(Json.Serialize(o), Encoding.UTF8, Constants.CONTENTTYPE_JSON);
	}
}
