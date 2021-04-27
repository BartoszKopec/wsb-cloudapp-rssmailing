using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpService
{
	public interface IHttp
	{
		Task<HttpResponse> GetAsync(HttpRequest request, CancellationToken token);
		Task<HttpResponse> PostAsync(HttpRequest request, CancellationToken token);
	}
}
