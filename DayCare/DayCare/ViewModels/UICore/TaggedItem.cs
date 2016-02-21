using DayCare.Model;
using DayCare.Model.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.UICore
{
	public class TaggedItem <T> : BaseItemUI
		where T: DataObject
	{
		public T Tag { get; set; }
	}
}
