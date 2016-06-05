using DayCare.Model.Lite;
using DayCare.Model.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Members
{
	public class MemberUI : TaggedItemUI<Member>
	{
		public string Details 
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Tag.Phone))
					return Name;
				else
					return string.Format("{0} ({1})", Name, Tag.Phone);
			}
		}
	}
}
