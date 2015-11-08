using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.ViewModels.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private string _name;
		private State _currentState;

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


		private Presence _presence;

		private List<MemberUI> _resposibles;

		private bool _showConfirmButton;


		

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


		public EditPrecenseViewModel(Presence presence)
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			// TODO: Complete member initialization
			this._presence = presence;

			Name = presence.FullName;

			if (DateTime.MinValue != presence.DepartureTime)
			{
				CurrentState = State.Left;
				ArrivalMember = model.GetMember(m => m.Id == _presence.ArrivalMember_Id).Select(m => string.Format("{0} {1}", m.FirstName, m.LastName)).First();
				SetArrivalTime(_presence.ArrivalTime);

				LeaveMember = model.GetMember(m => m.Id == _presence.DepartureMember_Id).Select(m => string.Format("{0} {1}", m.FirstName, m.LastName)).First();
				SetLeaveTime(_presence.DepartureTime);
			}
			else if (DateTime.MinValue != presence.ArrivalTime)
			{
				CurrentState = State.Arrived;
				ArrivalMember = model.GetMember(m => m.Id == _presence.ArrivalMember_Id).Select(m => string.Format("{0} {1}", m.FirstName, m.LastName)).First();
				SetArrivalTime(_presence.ArrivalTime);
				UpdateLeaveTime();
			}
			else
			{
				CurrentState = State.NotArrivedYet;
				UpdateArrivalTime();
			}
			
			var accountID = model.GetChild(c => c.Id == _presence.Child_Id).Select(c => c.Account_Id).First();

			Resposibles = (from m in model.GetMember(m => m.Account_Id == accountID && m.Deleted == false)
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
				_presence.ArrivalMember_Id = SelectedResponsible.Tag.Id;
				_presence.ArrivalTime = _arrivalTime;
				model.UpdatePresence(_presence);
			}
			else if (CurrentState == State.Arrived)
			{
				_presence.DepartureMember_Id = SelectedResponsible.Tag.Id;
				_presence.DepartureTime = _leaveTime;
				model.UpdatePresence(_presence);
			}
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
			_arrivalTime = DateTime.Now;
			NotifyOfPropertyChange(() => ArrivalTime);
		}

		public void UpdateLeaveTime()
		{
			_leaveTime = DateTime.Now;
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
