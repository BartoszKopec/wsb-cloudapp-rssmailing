using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
	public class HttpResponse
	{
		public int Status { get; set; }
		public string Content { get; set; }
		public bool IsSuccessful => Status / 100 == 2;
	}
}
