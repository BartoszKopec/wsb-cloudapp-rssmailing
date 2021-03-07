﻿using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[Route(Constants.ROUTE_API_RSS)]
	[ApiController]
	[Produces(Constants.CONTENTTYPE_JSON)]
	public class RssController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetList([FromQuery]string email, CancellationToken token=default)
		{
			return null;
		}

		[HttpPost]
		public async Task<IActionResult> InsertRss([FromBody]object body, CancellationToken token=default)
		{
			return null;
		}
	}
}
