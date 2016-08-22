using Caliburn.Micro;
using DayCare.Model.Lite;
using DayCare.Model.UI;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Database
{
	public class TableInfo : TaggedItemUI<string>
	{
	}

	public class MaintenanceViewModel : ListItemScreen<TableInfo>
	{
		#region Members

		//private List<TableInfo> _tables;
		//private TableInfo _selectedTable;
		
		#endregion

		#region Properties

		//public List<TableInfo> Tables
		//{
		//	get { return _tables; }
		//	set { _tables = value; NotifyOfPropertyChange(() => Tables); }
		//}
		//public TableInfo SelectedTable
		//{
		//	get { return _selectedTable; }
		//	set { _selectedTable = value; NotifyOfPropertyChange(() => SelectedTable); }
		//}

		#endregion

		public MaintenanceViewModel()
		{
		}

		protected override void LoadItems()
		{
			Items = new List<TableInfo> 
			{
				new TableInfo { Name = "Account", Tag = PetoeterDb.TableAccount },
				new TableInfo { Name = "Children", Tag = PetoeterDb.TableChildren},
				new TableInfo { Name = "Members", Tag = PetoeterDb.TableMember },
				new TableInfo { Name = "Holiday", Tag = PetoeterDb.TableHolidays },
				new TableInfo { Name = "Presence", Tag = PetoeterDb.TablePresence },
				new TableInfo { Name = "System", Tag = PetoeterDb.TableSystem }
			};

			base.LoadItems();
		}

		public void OpenAction(TableInfo ui)
		{ 
		}
	}
}
