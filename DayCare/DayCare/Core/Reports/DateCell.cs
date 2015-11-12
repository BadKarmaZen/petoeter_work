using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	class DateCell  : Cell
	{
		public DateCell(ColumnId id, DateTime dateTime, uint index)
			: this(id.ToString(), dateTime, index)
		{ }

		public DateCell(string id, DateTime dateTime, uint index)
		{
			this.DataType = CellValues.Date;
			this.CellReference = id + index;
			//this.StyleIndex = 10;
			this.CellValue = new CellValue { Text = dateTime.ToOADate().ToString() }; ;
		}
	}
}
