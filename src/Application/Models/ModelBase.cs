using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
	public abstract class ModelBase
	{
		public virtual bool IsValid() => false;
	}
}
