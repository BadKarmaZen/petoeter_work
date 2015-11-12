using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class ColumnId
	{
		private byte[] _value = new byte[] { 0, 0, 0 };

		public ColumnId()
		{
			_value[0] = 1;
		}

		public ColumnId(char value)
		{
			_value[0] = (byte)(1 + (char.ToLower(value) - 'a'));
		}

		public void Increment(int add = 1)
		{
			while (add-- > 0)
			{
				if (_value[0] < 26)
				{
					_value[0]++;
				}
				else
				{
					_value[0] = 1;
					if (_value[1] < 26)
					{
						_value[1]++;
					}
					else
					{
						_value[1] = 1;
						if (_value[2] < 26)
						{
							_value[2]++;
						}
					}
				}
			}
		}


		public override string ToString()
		{
			if (_value[2] == 0 && _value[1] == 0)
				return Convert.ToChar('A' + (_value[0] - 1)).ToString();
			else if (_value[2] == 0)
				return string.Format("{0}{1}", Convert.ToChar('A' + (_value[1] - 1)), Convert.ToChar('A' + (_value[0] - 1)));
			else
				return string.Format("{0}{1}{2}", Convert.ToChar('A' + (_value[2] - 1)), Convert.ToChar('A' + (_value[1] - 1)), Convert.ToChar('A' + (_value[0] - 1)));
		}
	}
}
