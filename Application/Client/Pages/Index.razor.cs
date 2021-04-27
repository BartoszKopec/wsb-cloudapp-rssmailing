using Application.Models;
using Application.Resources;
using Application.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Application.Client.Pages
{
	public partial class Index
	{
		private const string CSS_CLASS_ERROR = "error";
		private const string CSS_CLASS_SUCCESS = "success";


		private List<string> _feeds = new();
		private CancellationTokenSource _tokenSource;
		private string _email,
			_listing,
			_message = "Waiting for action.",
			_messageType,
			_feed,
			_previewSource = "/html/exampleemail.html";

		[Inject]
		public ApiClient Api { get; set; }

		private void SetListing()
		{
			StringBuilder sb = new();
			foreach (string link in _feeds)
			{
				sb.Append($"<tr><td>{link}</td></tr>");
			}
			_listing = sb.ToString();
		}

		private void ChangeMessage(string message, string styleClass)
		{
			_message = message;
			_messageType = styleClass;
			StateHasChanged();
		}
		private void ShowSuccess(string message) => ChangeMessage(message, CSS_CLASS_SUCCESS);
		private void ShowError(string message) => ChangeMessage(message, CSS_CLASS_ERROR);
		private void ShowWorking() => ChangeMessage("Working...", null);

		private async void GetFeeds()
		{
			ShowWorking();
			_tokenSource = new CancellationTokenSource();
			await Api.GetRssUrlsAsync(_email, _tokenSource.Token,
				onSuccess: (list) =>
				{
					_feeds = list;
					SetListing();
					ShowSuccess(Strings.SUCCESS_GETFEED);
				},
				onError: (err) => ShowError(err));
		}

		private void AddFeed()
		{
			_feeds.Add(_feed);
			SetListing();
		}

		private async void SaveData()
		{
			ShowWorking();
			_tokenSource = new CancellationTokenSource();
			RssRequestBody body = new()
			{
				AddressEmail = _email,
				Urls = _feeds
			};
			await Api.PostSaveAsync(body, _tokenSource.Token,
				onSuccess: () => ShowSuccess(Strings.SUCCESS_SAVEDATA),
				onError: (err) => ShowError(err));
		}

		private void GetPreview()
		{
			_previewSource = null;
			_previewSource = $"{Api.Endpoint_mailing}?email={_email}";
		}

		private async void SendEmail()
		{
			ShowWorking();
			_tokenSource = new CancellationTokenSource();
			MailingRequestBody body = new()
			{
				AdressEmail = _email
			};
			await Api.PostSendAsync(body, _tokenSource.Token,
				onSuccess: () => ShowSuccess(Strings.SUCCESS_SENDMAIL),
				onError: (err) => ShowError(err));
		}
	}
}
