using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Units.Controllers
{
	public class ControllerTestsBase<TController> : TestsBase
	{
		protected DatabaseStringIdMock _database;
		protected SenderMock _sender;
		protected HttpMock _http;
		protected TController Controller;
	}
}
