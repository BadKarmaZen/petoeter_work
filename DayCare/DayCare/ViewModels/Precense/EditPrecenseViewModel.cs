using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.Model.Lite;
using DayCare.ViewModels.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.ViewModels.Precense
{
	public class EditPrecenseViewModel : Screen
	{
		enum State
		{
			NotArrivedYet,
			HasArrived,
			HasLeft
		}

		#region Members

		private string _name;
		private State _currentState;
		private Presence _presence;
		private List<MemberUI> _resposibles;
		private bool _showConfirmButton;
		private DateTime _currentTime;

		#endregion

		#region Properties
		private State CurrentState
		{
			get { return _currentState; }
			set
			{
				_currentState = value;
				NotifyOfPropertyChange(() => HasArrived);
				NotifyOfPropertyChange(() => HasLeft);
				NotifyOfPropertyChange(() => ShowSelection);
			}
		}

		public bool NotArrivedYet
		{
			get
			{
				return _currentState == State.NotArrivedYet;
			}
		}
		public bool HasArrived
		{
			get { return _currentState != State.NotArrivedYet; }
		}

		public bool HasLeft
		{
			get
			{
				return _currentState == State.HasLeft;
			}
		}

		public bool ShowSelection
		{
			get { return _currentState != State.HasLeft; }
		}

		public DateTime CurrentTime
		{
			get { return _currentTime; }
			set { _currentTime = value; NotifyOfPropertyChange(() => CurrentTime); }
		}

		public bool ShowConfirmButton
		{
			get { return _showConfirmButton; }
			set { _showConfirmButton = value; NotifyOfPropertyChange(() => ShowConfirmButton); }
		}

		public MemberUI SelectedResponsible { get; set; }
		public List<Members.MemberUI> Resposibles
		{
			get { return _resposibles; }
			set { _resposibles = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}

		public BitmapImage Image
		{
			get
			{
				return PetoeterImageManager.GetImage(_presence.Child.FileId);
			}
		}

		#endregion

		#region Arriving

		private string _broughtByName;
		private DateTime _broughtAt;

		public string BroughtByName
		{
			get { return _broughtByName; }
			set { _broughtByName = value; NotifyOfPropertyChange(() => BroughtByName); }
		}

		public string BroughtAt
		{
			get { return _broughtAt.ToString("HH:mm"); }
		}

		#endregion

		#region Leaving
		private string _takenByName;
		private DateTime _takenAt;

		//public bool Leaving
		//{
		//	get { return _leaving; }
		//	set { _leaving = value; NotifyOfPropertyChange(() => Leaving); }
		//}

		public string TakenAt
		{
			get { return _takenAt.ToString("HH:mm"); }
		}

		public string TakenByName
		{
			get { return _takenByName; }
			set { _takenByName = value; NotifyOfPropertyChange(() => TakenByName); }
		}

		#endregion

		public EditPrecenseViewModel(Presence presence)
		{
			LogManager.GetLog(GetType()).Info("Create");

			this._presence = presence;

			Name = _presence.Child.GetFullName();
			CurrentState = State.NotArrivedYet;

			if (_presence.BroughtBy != null)
			{
				CurrentState = State.HasArrived;
				BroughtByName = _presence.BroughtBy.GetFullName();
				_broughtAt = _presence.BroughtAt;
			}

			if (_presence.TakenBy != null)
			{
				CurrentState = State.HasLeft;
				TakenByName = _presence.TakenBy.GetFullName();
				_takenAt = _presence.TakenAt;
			}

			CurrentTime = DateTime.Now;

			if (HasLeft == false)
			{
				using (var db = new PetoeterDb(PetoeterDb.FileName))
				{
					//	find account
					var account = GetAccount(db.Accounts.FindAll());
					if (account != null)
					{
						Resposibles = (from m in account.Members
													 select new MemberUI
													{
														Name = m.GetFullName(),
														Tag = m
													}).ToList();

						//Resposibles.AddRange(from m in db.Members.Find(mem => ( mem.Phone == "GrandParents" || mem.Phone == "Other"))
						//										select new MemberUI
						//										{
						//											Name = m.FirstName,
						//											Tag = m
						//										});
					}
				}
			}
		}

		private Account GetAccount(IEnumerable<Account> accounts)
		{
			foreach (var account in accounts)
			{
				if (account != null && account.Children != null)
				{
					if (account.Children.Exists(c => c.Id == _presence.Child.Id))
					{
						return account;
					}
				}
			}

			return null;
		}

		#region Actions
		public void CloseAction()
		{
			LogManager.GetLog(GetType()).Info("Close");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());
		}

		#endregion


		public void ConfirmAction()
		{
			LogManager.GetLog(GetType()).Info("Confirm");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				if (CurrentState == State.NotArrivedYet)
				{
					_presence.BroughtBy = SelectedResponsible.Tag;
					_presence.BroughtAt = _broughtAt = CurrentTime;
				}
				else if (CurrentState == State.HasArrived)
				{
					_presence.TakenBy = SelectedResponsible.Tag;
					_presence.TakenAt = _takenAt = CurrentTime;
				}

				_presence.Updated = DateTime.Now;
				db.Presences.Update(_presence);
			}

			//var model = ServiceProvider.Instance.GetService<Petoeter>();

			//	TODO
			//if (CurrentState == State.NotArrivedYet)
			//{
			//	_presence.Arriving = SelectedResponsible.Tag;
			//	_presence.ArrivingTime = _arrivalTime;

			//	_presence.Updated = true;
			//	model.Save();
			//}
			//else if (CurrentState == State.Arrived)
			//{
			//	_presence.DepartureMember_Id = SelectedResponsible.Tag.Id;
			//	_presence.DepartureTime = _leaveTime;
			//	model.UpdatePresence(_presence);
			//}
		}

		public void SelectResponsible(MemberUI responsible)
		{
			LogManager.GetLog(GetType()).Info("Select member");
			if (responsible.Selected == true)
			{
				SelectedResponsible.Selected = false;
				SelectedResponsible = null;
			}
			else
			{
				if (SelectedResponsible != null)
				{
					SelectedResponsible.Selected = false;
				}

				responsible.Selected = true;
				SelectedResponsible = responsible;
			}

			ShowConfirmButton = SelectedResponsible != null;
		}

		//public void UpdateArrivalTime()
		//{
		//	_broughtAt = DateTimeProvider.Now();
		//	NotifyOfPropertyChange(() => BroughtAt);
		//}

		//public void UpdateLeaveTime()
		//{
		//	_takenAt = DateTimeProvider.Now();
		//	NotifyOfPropertyChange(() => TakenAt);
		//}

		//public void SetArrivalTime(DateTime time)
		//{
		//	_broughtAt = time;
		//	NotifyOfPropertyChange(() => ArrivalTime);
		//}

		//public void SetLeaveTime(DateTime time)
		//{
		//	_takenAt = time;
		//	NotifyOfPropertyChange(() => LeaveTime);
		//}
	}
}
