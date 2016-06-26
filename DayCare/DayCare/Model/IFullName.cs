using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	interface IFullName
	{
		string GetFullName(bool firstName = true);
	}
}
