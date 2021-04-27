using HttpService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Units
{
	public class HttpMock : IHttp
	{
		public string MockContent;
		public int Status = 200;

		public async Task<HttpResponse> GetAsync(HttpRequest request, CancellationToken token)
		{
			await Task.Delay(50, token);
			return new HttpResponse
			{
				Content = MockContent,
				Status = Status
			};
		}

		public Task<HttpResponse> PostAsync(HttpRequest request, CancellationToken token)
		{
			throw new NotImplementedException();
		}
	}
}
