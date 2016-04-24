using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
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
			Arrived,
			Left			
		}

		#region Members
		
		private string _name;
		private State _currentState;
		private Presence _presence;
		private List<MemberUI> _resposibles;
		private bool _showConfirmButton;

		#endregion

		#region Properties
		private State CurrentState
		{
			get { return _currentState; }
			set 
			{
				_currentState = value;
				NotifyOfPropertyChange(() => Arrived);
				NotifyOfPropertyChange(() => Left);
				NotifyOfPropertyChange(() => ShowSelection);
			}
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
				var img = ServiceProvider.Instance.GetService<ImageManager>();
				return img.CreateBitmap(img.FindImage(_presence.Child.Id.ToString()));
			}
		}

		#endregion
		
		#region Arriving
		
		private string _arrivalMember;
		private DateTime _arrivalTime;
	
		public bool Arrived
		{
			get { return _currentState == State.Arrived || 
									 _currentState == State.Left; }
		}

		public bool Left
		{
			get { return _currentState == State.Left; }
		}

		public bool ShowSelection
		{
			get { return _currentState != State.Left; }
		}
		
		public string ArrivalMember
		{
			get { return _arrivalMember; }
			set { _arrivalMember = value; NotifyOfPropertyChange(() => ArrivalMember); }
		}
	
		public string ArrivalTime
		{
			get { return _arrivalTime.ToString("HH:mm"); }
		}

		#endregion

		#region Leaving

		private bool _leaving;
		private string _leaveMember;
		private DateTime _leaveTime;

		public bool Leaving
		{
			get { return _leaving; }
			set { _leaving = value; NotifyOfPropertyChange(() => Leaving); }
		}

		public string LeaveTime
		{
			get { return _leaveTime.ToString("HH:mm"); }
		}

		public string LeaveMember
		{
			get { return _leaveMember; }
			set { _leaveMember = value; NotifyOfPropertyChange(()=>LeaveMember);}
		}

		#endregion
				
		public EditPrecenseViewModel(Presence presence)
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			// TODO: Complete member initialization
			this._presence = presence;

			Name = string.Format("{0} {1}", presence.Child.FirstName, presence.Child.LastName);

			if (DateTime.MinValue != presence.LeavingTime)
			{
				CurrentState = State.Left;
				//ArrivalMember = model.GetMembers(m => m.Id == _presence.ArrivalMember_Id).Select(m => string.Format("{0} {1}", m.FirstName, m.LastName)).First();
				SetArrivalTime(_presence.ArrivingTime);

				//LeaveMember = model.GetMember(m => m.Id == _presence.DepartureMember_Id).Select(m => string.Format("{0} {1}", m.FirstName, m.LastName)).First();
				SetLeaveTime(_presence.LeavingTime);
			}
			else if (DateTime.MinValue != presence.ArrivingTime)
			{
				CurrentState = State.Arrived;
				ArrivalMember = _presence.Child.Account.Members.Where(m => m.Id == _presence.Arriving.Id).Select(m => string.Format("{0} {1}", m.FirstName, m.LastName)).First();
				SetArrivalTime(_presence.ArrivingTime);
				UpdateLeaveTime();
			}
			else
			{
				CurrentState = State.NotArrivedYet;
				UpdateArrivalTime();
			}
			
			//var accountID = model.GetChildren().Where(c => c.Id == _presence.Child.Id).Select(c => c.Account_Id).First();

			Resposibles = (from m in _presence.Child.Account.Members // model.GetMember(m => m.Account_Id == accountID && m.Deleted == false)
                     select new MemberUI
                     {
                         Name = string.Format("{0} {1}", m.FirstName, m.LastName),
                         Tag = m
                     }).ToList();
		}

		public void CloseAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
						 new Events.ShowDialog());
		}

		public void ConfirmAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
						 new Events.ShowDialog());

			var model = ServiceProvider.Instance.GetService<Petoeter>();

			if (CurrentState == State.NotArrivedYet)
			{
				_presence.Arriving = SelectedResponsible.Tag;
				_presence.ArrivingTime = _arrivalTime;

				_presence.Updated = true;
				model.Save();
			}
			//else if (CurrentState == State.Arrived)
			//{
			//	_presence.DepartureMember_Id = SelectedResponsible.Tag.Id;
			//	_presence.DepartureTime = _leaveTime;
			//	model.UpdatePresence(_presence);
			//}
		}

		public void SelectResponsible(MemberUI responsible)
		{
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

		public void UpdateArrivalTime()
		{
			_arrivalTime = DateTimeProvider.Now();
			NotifyOfPropertyChange(() => ArrivalTime);
		}

		public void UpdateLeaveTime()
		{
			_leaveTime = DateTimeProvider.Now();
			NotifyOfPropertyChange(() => LeaveTime);
		}

		public void SetArrivalTime(DateTime time)
		{
			_arrivalTime = time;
			NotifyOfPropertyChange(() => ArrivalTime);
		}

		public void SetLeaveTime(DateTime time)
		{
			_leaveTime = time;
			NotifyOfPropertyChange(() => LeaveTime);
		}
	}
}
