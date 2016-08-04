using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.Model.Lite;
using DayCare.Model.UI;
using DayCare.ViewModels.Children;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DayCare.ViewModels.Precense
{
	public class PresenceUI : TaggedItemUI<Presence>
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
				return PetoeterImageManager.GetImage(Tag.Child.FileId);
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
				if (Tag.BroughtAt == DateTime.MinValue)
				{
					return Brushes.White;
				}
				else
				{
					var time = DateTimeProvider.Now();

					if (Tag.TakenAt != DateTime.MinValue)
						time = Tag.TakenAt;

					var delta = time - Tag.BroughtAt;

					return delta > MaxTime ? Brushes.Salmon : Brushes.LightGreen;
				}
			}
		}
		#endregion


		public void Update()
		{
			if (Tag.BroughtAt == DateTime.MinValue)
			{
				ToLate = false;
			}
			else
			{
				var time = DateTimeProvider.Now();

				if (Tag.TakenAt != DateTime.MinValue)
					time = Tag.TakenAt;

				var delta = time - Tag.BroughtAt;

				ToLate = delta > MaxTime;
			}
		}

		//public string FindImageFile()
		//{
		//	var model = ServiceProvider.Instance.GetService<Petoeter>();
		//	var child = model.GetChildren(c => c.Id == Tag.Child_Id).FirstOrDefault();

		//	if (child != null)
		//	{
		//		return Directory.EnumerateFiles(model.Settings.ImageFolder, string.Format("{0}*", child.Id.ToString())).FirstOrDefault();
		//	}

		//	return string.Empty;
		//}
	}

	public class UpdateTimeEvent
	{
		public DateTime CurrentTime { get; set; }
	}

	public class PresenceMainViewModel : Screen, ICloseScreen, IHandle<UpdateTimeEvent>
	{
		#region Members
		private List<PresenceUI> _presenceList;

		private DispatcherTimer _timer;

		public Events.RegisterMenu AddChildMenu { get; set; }
		#endregion

		#region Properties
		public List<PresenceUI> PresenceList
		{
			get { return _presenceList; }
			set { _presenceList = value; NotifyOfPropertyChange(() => PresenceList); }
		}
		
		#endregion

		public PresenceMainViewModel()
		{
			LogManager.GetLog(GetType()).Info("Create");

			//	add menu
			AddChildMenu = new Events.RegisterMenu
			{
				Caption = "Toeveogen",
				Id = "PresenceMainViewModel.AddChildMenu",
				Add = true,
				Action = () => 
				{
					ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
							new Events.ShowDialog
							{
								Dialog = new PasswordDialogViewModel("856039")
								{
									Yes = () => AddChildAction()
								}
							});
				}
			};

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddChildMenu);

			LoadPresenceData();

			_timer = new DispatcherTimer(DispatcherPriority.Render);
			_timer.Interval = TimeSpan.FromSeconds(5);
			_timer.Tick += (s, e) => { RefreshThumbs(); };

			_timer.Start();

			ServiceProvider.Instance.GetService<EventAggregator>().Subscribe(this);
		}

		private void LoadPresenceData()
		{
			var lst = new List<PresenceUI>();

			var today = DateTimeProvider.Now().Date;

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				if (db.Presences.Find(p => p.Date == today).Count() == 0)
				{
					LogManager.GetLog(GetType()).Info("Create({0})", today.ToShortDateString());
					//	create a presence record for all children for today
					//	var children = (from c in db.Children.FindAll() select c).ToList();

					foreach (var child in from c in db.Children.FindAll()
																where c.Schedule.Exists(d => d.Day == today)
																select c)
					{
						var date = child.Schedule.First(d => d.Day == today);

						if (date.Morning || date.Afternoon)
						{
							var presence = new Presence
							{
								Child = child,
								Date = today,
								Updated = today,
								TimeCode = CalculateTimeCode(date)
							};

							db.Presences.Insert(presence);
						}
					}
				}
				
				//	load
				foreach (var presence in db.Presences.Find(p => p.Date == today))
				{
					lst.Add(new PresenceUI
					{
						Name = presence.Child.GetFullName(),
						Tag = presence,
						MaxTime = new TimeSpan(presence.TimeCode, 0, 0)
					});
				}
			}
			
			LogManager.GetLog(GetType()).Info("Loaded children: {0}", lst.Count);

			PresenceList = lst;
			RefreshThumbs();
		}

		public void AddChildAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog
				{
					Dialog = new AddPresenceDialogViewModel()
					{
						Yes = () =>
						{
							LoadPresenceData();
						}
					}
				});
 
		}
		
		private int CalculateTimeCode(Model.Lite.Date date)
		{
			if (date.Morning || date.Afternoon)
			{
				return 9;	//	hours
			}
			else if (date.Morning || date.Afternoon)
			{
				return 6;	//	hours
			}

			return 0;
		}

		private void RefreshThumbs()
		{
			PresenceList.ForEach(p => p.Update());
		}

		public void SelectChildAction(PresenceUI child)
		{
			LogManager.GetLog(GetType()).Info("Select child");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new EditPrecenseViewModel(child.Tag)
				});
		}

		public void CloseThisScreen()
		{
			AddChildMenu.Add = false;
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddChildMenu);
			_timer.Stop();
		}


		public void Handle(UpdateTimeEvent message)
		{
			RefreshThumbs();
		}
	}
}
