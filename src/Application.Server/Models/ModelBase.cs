using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Models
{
	public abstract class ModelBase
	{
		public virtual bool IsValid() => false;
	}
}
