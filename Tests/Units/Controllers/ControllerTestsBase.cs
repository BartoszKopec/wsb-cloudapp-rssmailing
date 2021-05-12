using Application.Data;
using System;
using System.Net.Http;

namespace Tests.Units.Controllers
{
	public class ControllerTestsBase<TController>
	{
		protected DatabaseStringIdMock _database;
		protected SenderMock _sender;
		protected HttpClient _http;
		protected TController Controller;

		protected virtual void CommonInit(params Record<string>[] records) =>
			throw new NotImplementedException();
	}
}
