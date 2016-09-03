using ClosedXML.Excel;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Invoicing
{
	public class InvoiceManager
	{
		public string Folder { get; set; }
		public string TemplateFolder { get; set; }

		public InvoiceManager()
		{
			TemplateFolder = "InvoiceTemplates";
			Folder = Directory.CreateDirectory(Properties.Settings.Default.InvoiceFolder).FullName;		//	@"E:\Temp\Invoices";
		}

		public InvoicePriceDetails GetCurrentPrices() 
		{
			return GetPrices(DateTime.Now.Year);
		}

		private InvoicePriceDetails GetPrices(int year)
		{
			var prices = new InvoicePriceDetails();

			var template = Path.Combine(TemplateFolder, string.Format("{0}.xlsx", year));
			if (File.Exists(template) == true)
			{
				var wb = new XLWorkbook(template);

				IXLWorksheet wsYear;
				wb.Worksheets.TryGetWorksheet("Jaaroverzicht", out wsYear);

				prices.FullDay = wsYear.Cell(19, "C").GetValue<double>();
				prices.HalfDay = wsYear.Cell(19, "E").GetValue<double>();
				prices.ExtraMeal = wsYear.Cell(19, "G").GetValue<double>();
				prices.ExtraHour = wsYear.Cell(19, "I").GetValue<double>();
				prices.Diapers = wsYear.Cell(19, "K").GetValue<double>();
				prices.Medication = wsYear.Cell(19, "M").GetValue<double>();
				prices.ToLate = wsYear.Cell(19, "O").GetValue<double>();
				prices.FullDaySick = wsYear.Cell(19, "Q").GetValue<double>();
				prices.HalfDaySick = wsYear.Cell(19, "S").GetValue<double>();

				wb.Dispose();
			}
			return prices;
		}

		public Invoice FindInvoice(Child child, int year) 
		{
			var id = MakeSafeName(child);
			var year_invoice = Directory.CreateDirectory(Path.Combine(Folder, year.ToString())).FullName;
			var invoiceFile = Path.Combine(year_invoice, string.Format("{0}.xlsx", id));
			
			if (!File.Exists(invoiceFile))
			{
				var template = Path.Combine(TemplateFolder, string.Format("{0}.xlsx", year));
				if (File.Exists(template) == true)
				{
					File.Copy(template, invoiceFile);

					var wb = new XLWorkbook(invoiceFile);

					IXLWorksheet wsYear;
					wb.Worksheets.TryGetWorksheet("Jaaroverzicht", out wsYear);

					var cell = wsYear.Cell(12, "G");
					cell.Value = child.GetFullName();
				}
				else
				{
					invoiceFile = string.Empty;
				}
			}
			
			return new Invoice { File = invoiceFile };
		}

		private string MakeSafeName(Child child)
		{
			var invalid = Path.GetInvalidFileNameChars();
			var name = from c in child.GetFullName()
								 select invalid.Contains(c) ? ' ' : c;

			return new string(name.ToArray());
		}
	}
}
