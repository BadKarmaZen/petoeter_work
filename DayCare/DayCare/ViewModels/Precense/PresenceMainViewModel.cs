using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.UI;
using DayCare.ViewModels.Children;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DayCare.ViewModels.Precense
{
	/*public class PresenceUI : TaggedItemUI<Presence>
	{
		#region members
		private bool _toLate;
		
		#endregion


		#region Properties
		public TimeSpan MaxTime { get; set; }

		public BitmapImage ImageData
		{
			get
			{
				var img = ServiceProvider.Instance.GetService<ImageManager>();
				return img.CreateBitmap(img.FindImage(Tag.Child_Id.ToString()));
			}
		}

		public bool ToLate
		{
			get
			{
				return _toLate;
			}

			set
			{
				_toLate = value;
				NotifyOfPropertyChange(() => ToLate); 
				NotifyOfPropertyChange(() => BackGround);
			}
		}
		
		public Brush BackGround
		{
			get
			{
				if (Tag.ArrivalTime == DateTime.MinValue)
				{
					return Brushes.White;
				}
				else
				{
					var time = DateTime.Now;

					if (Tag.DepartureTime != DateTime.MinValue)
						time = Tag.DepartureTime;

					var delta = time - Tag.ArrivalTime;

					return delta > MaxTime ? Brushes.Salmon : Brushes.LightGreen;
				}
			}
		}
		#endregion
		

		public void Update()
		{
			if (Tag.ArrivalTime == DateTime.MinValue)
			{
				ToLate = false;
			}
			else
			{
				var time = DateTime.Now;

				if (Tag.DepartureTime != DateTime.MinValue)
					time = Tag.DepartureTime;

				var delta = time - Tag.ArrivalTime;

				ToLate = delta > MaxTime;
			}
		}

		public string FindImageFile()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();
			var child = model.GetChild(c => c.Id == Tag.Child_Id).FirstOrDefault();

			if (child != null)
			{
				return Directory.EnumerateFiles(model.Settings.ImageFolder, string.Format("{0}*", child.Id.ToString())).FirstOrDefault();
			}

			return string.Empty;
		}
	}

	public class PresenceMainViewModel : Screen, ICloseScreen
	{
		private List<PresenceUI> _presenceList;

		private DispatcherTimer _timer;

		public List<PresenceUI> PresenceList
		{
			get { return _presenceList; }
			set { _presenceList = value; NotifyOfPropertyChange(() => PresenceList); }
		}

		public PresenceMainViewModel()
		{
			var today = DateTime.Now;
			var model = ServiceProvider.Instance.GetService<Petoeter>();
			var data = model.GetPresenceData();

			var lst = new List<PresenceUI>();

			foreach (var item in from d in data orderby d.FullName select d)
			{
				//var child = model.GetChild(c => c.Id == item.Child_Id).FirstOrDefault();
				//var currentSchedule = model.GetCurrentSchedule(child, today);

				var ui = new PresenceUI
				{
					Name = item.FullName,
					Tag = item,
					MaxTime = new TimeSpan(item.TimeCode, 0, 0)
				};

				lst.Add(ui);
			}

			PresenceList = lst;
			RefreshThumbs();

			_timer = new DispatcherTimer(DispatcherPriority.Render);
			_timer.Interval = TimeSpan.FromSeconds(5);
			_timer.Tick += (s, e) => { RefreshThumbs(); };

			_timer.Start();
		}

		private void RefreshThumbs()
		{
			PresenceList.ForEach(p => p.Update());
		}

		public void SelectChildAction(PresenceUI child)
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
								new Core.Events.ShowDialog
								{
									Dialog = new EditPrecenseViewModel(child.Tag)
								});
		}

		public void CloseThisScreen()
		{
			_timer.Stop();
		}
	}*/
}
