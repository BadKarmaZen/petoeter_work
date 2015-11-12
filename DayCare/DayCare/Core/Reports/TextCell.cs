using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class TextCell : Cell
	{
		public TextCell(ColumnId column, string text, uint index)
			: this(column.ToString(), text, index)
		{
		}

		public TextCell(string column, string text, uint index)
		{
			this.DataType = CellValues.InlineString;
			this.CellReference = column + index;
			//this.StyleIndex = 1;
			//Add text to the text cell.
			this.InlineString = new InlineString { Text = new Text { Text = text } };
		}
	}
}
