using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Units.Controllers
{
	class ControllerAssert
	{
		public static void IsValidErrorResult(object actualError, string expectedMessage)
		{
			Type expectedErrorType = typeof(Error);
			Type expectedErrorMessageType = typeof(string);
			
			Assert.IsType(expectedErrorType, actualError);
			Error error = actualError as Error;
			Assert.IsType(expectedErrorMessageType, error.Message);
			Assert.Equal(expectedMessage, error.Message);
		}
	}
}
