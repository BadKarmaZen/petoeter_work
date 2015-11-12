using Caliburn.Micro;
using DayCare.Core;
using DayCare.Core.Reports;
using DayCare.Model.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Reports
{
	public class MainReportsViewModel : Screen
	{
		public void MonthListAction()
		{
			var file = @"E:\temp\data.xlsx";

			MonthList report = new MonthList();
			//report.LoadData(from c in model.GetChild(c => c.Deleted == false) select c);
			report.Create(file);
			

			//ExcelFileTester.Test();
		}
	}
}
